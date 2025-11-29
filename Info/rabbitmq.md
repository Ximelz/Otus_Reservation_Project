# RabbitMQ: шина событий и подписка
Документ описывает топологию RabbitMQ, формат событий, правила публикации/подписки и полный перечень доменных событий с издателями и подписчиками.

## 1. Общие принципы
- Топология: один topic exchange `hotel.reservation.events` (durable, auto-delete=false). Все сервисы публикуют и подписываются через него.
- Сообщения: `content-type: application/json`, `delivery_mode: persistent`.
- Обязательные свойства/заголовки:
  - `message_id` (UUID) — ключ идемпотентности.
  - `correlation_id` — для трассировки цепочки вызовов.
  - `causation_id` — Id родительского события/команды (если есть).
  - `type` — имя события (например, `ReservationCreated`).
  - `app_id` — имя сервиса-издателя.
- Формат payload (минимум): `{ "eventId": "...", "eventName": "...", "occurredAt": "2024-05-01T10:00:00Z", "source": "ReservationService", "data": { ... } }`.
- Очереди: отдельная очередь на сервис (и, при необходимости, на событие). Паттерн имени: `{service}.{event}` или `{service}.events`.
- Идемпотентность: хендлер сохраняет `message_id`/`eventId` в журнале обработок, повторная доставка игнорируется.
- Ретраи: базовый retry с экспоненциальной задержкой или политика MassTransit; после N неуспехов — DLQ `hotel.dlx`.

## 2. Подписка на события (пример для .NET + MassTransit)
1) Описать контракт события (record) и Consumer:
```csharp
public record ReservationCreated(Guid ReservationId, Guid CustomerId, Guid HotelId,
    DateOnly CheckInDate, DateOnly CheckOutDate, decimal TotalPrice);

public class ReservationCreatedConsumer : IConsumer<ReservationCreated>
{
    public async Task Consume(ConsumeContext<ReservationCreated> context)
    {
        // TODO: проверить идемпотентность по context.MessageId
        // TODO: доменная логика (создать задачи/уведомления)
    }
}
```
2) Зарегистрировать в DI и привязать к очереди:
```csharp
services.AddMassTransit(x =>
{
    x.AddConsumer<ReservationCreatedConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(env.RabbitMqHost, "/", h =>
        {
            h.Username(env.RabbitMqUser);
            h.Password(env.RabbitMqPassword);
        });
        cfg.ReceiveEndpoint("scheduler.ReservationCreated", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind("hotel.reservation.events", s =>
            {
                s.ExchangeType = ExchangeType.Topic;
                s.RoutingKey = "reservation.created";
            });
            e.ConfigureConsumer<ReservationCreatedConsumer>(ctx);
        });
    });
});
```
3) Политика ретраев и идемпотентность: задать `UseMessageRetry` (например, 3 попытки с jitter), писать `MessageId` в таблицу обработок, на повторную доставку выходить без побочных эффектов.
4) Логировать `correlation_id` и бизнес-результат; метрики успешных/ошибочных обработок.

## 3. Публикация событий (пример)
```csharp
public async Task PublishReservationCreated(Reservation reservation, IPublishEndpoint publish)
{
    await publish.Publish(new ReservationCreated(
        reservation.Id, reservation.CustomerId, reservation.HotelId,
        reservation.CheckInDate, reservation.CheckOutDate, reservation.TotalPrice),
        ctx =>
        {
            ctx.SetRoutingKey("reservation.created");
            ctx.MessageId = NewId.NextGuid();
            ctx.CorrelationId = reservation.Id;
        });
}
```

## 4. Перечень событий и подписчиков
| Событие | Публикует | Подписчики | Назначение | Ключевые поля |
| --- | --- | --- | --- | --- |
| UserRegistered (опц.) | IdentityService | CustomerService | Создать профиль клиента | UserId, Email, Roles |
| CustomerProfileUpdated (опц.) | CustomerService | AdminService | Обновление справочника клиентов/анализ | CustomerId, ChangedAt |
| ReservationCreated | ReservationService | SchedulerService, NotificationService, AdminService (сводки) | Планирование задач, уведомление клиента, метрики | ReservationId, CustomerId, HotelId, CheckInDate, CheckOutDate, TotalPrice |
| ReservationCancelled | ReservationService | SchedulerService, NotificationService, AdminService | Снятие задач, уведомление, обновление сводок | ReservationId, Reason, CancelledAt |
| ReservationCheckedIn | ReservationService | AdminService, NotificationService (опц.) | Фиксация заезда, оповещение | ReservationId, CheckInAt |
| ReservationCheckedOut | ReservationService | AdminService | Фиксация выезда | ReservationId, CheckOutAt |
| NoShowMarked | ReservationService (по задаче планировщика) | NotificationService, AdminService | Отметить незаезд, уведомить | ReservationId, MarkedAt |
| ReminderDue | SchedulerService | NotificationService | Запуск напоминания о заезде | ReservationId, CustomerId, HotelId, CheckInDate |
| NotificationFailed (опц.) | NotificationService | AdminService/Monitoring | Отслеживание сбоев уведомлений | ReservationId, Type, Error, Attempt |

## 5. Роутинг и ключи
- Рекомендуемые routing keys:
  - `reservation.created`, `reservation.cancelled`, `reservation.checkedin`, `reservation.checkedout`, `reservation.noshow`.
  - `scheduler.reminder.due`.
  - `notification.failed`.
  - `identity.user.registered`, `customer.profile.updated`.
- Очереди по сервисам:
  - SchedulerService: `scheduler.ReservationCreated`, `scheduler.ReservationCancelled`.
  - NotificationService: `notification.ReservationCreated`, `notification.ReservationCancelled`, `notification.ReminderDue`, `notification.NoShowMarked`.
  - AdminService: `admin.ReservationCreated`, `admin.ReservationCancelled`, `admin.ReservationCheckedIn`, `admin.ReservationCheckedOut`, `admin.NoShowMarked`.
  - CustomerService: `customer.UserRegistered` (опц.).

## 6. Обработка ошибок и DLQ
- При непреодолимой ошибке после N ретраев — `nack` → DLQ `hotel.dlx` (binding по тому же routing key).
- DLQ сообщения должны содержать заголовки `x-first-death-queue`, `x-first-death-reason` для диагностики.
- Рекомендуется метрика dead-letter count и алертинг.

## 7. Минимальные настройки RabbitMQ
- VHost: `/` (по умолчанию) или `hotel` при раздельной установке.
- Пользователь/пароль из переменных окружения (`RABBITMQ__USERNAME`, `RABBITMQ__PASSWORD`).
- Настройки durable=true на exchange/queues; auto-delete=false.
- Prefetch (QoS) подбирать под хендлеры (начать с 10–20).
