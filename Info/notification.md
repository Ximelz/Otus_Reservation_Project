# NotificationService (Notification.Api)
## Назначение (продукт)
- Отправляет уведомления клиентам: подтверждение брони, напоминание о заезде, уведомление об отмене.
- Каналы: Email (SMTP) и Telegram Bot API.

## Функционал и API
- Обработка событий:
  - `ReservationCreated` → подтверждение.
  - `ReminderDue` → напоминание.
  - `ReservationCancelled` → уведомление об отмене.
- Очередь на отправку:
  - Формирование сообщений по шаблонам.
  - Ретрай при сбоях каналов.
- Вспомогательные эндпоинты (внутренние):
  - `GET /api/notifications/templates` — список шаблонов.
  - `POST /api/notifications/templates` — обновление/создание.
  - `POST /api/notifications/preview` — предпросмотр подстановки данных.

## Данные
- `NotificationTemplates` (Id, Type, Channel, Subject, BodyTemplate, Locale, CreatedAt, UpdatedAt).
- `OutgoingMessages` (Id, Type, Channel, Recipient, PayloadJson, Status, LastError, SentAt, CreatedAt, UpdatedAt) с индексом на Status.

## События
- Потребляет: `ReservationCreated`, `ReservationCancelled`, `ReminderDue`.
- Может публиковать `NotificationFailed` для мониторинга (опционально).

## Нефункциональные требования
- Идемпотентность по (ReservationId, Type) во избежание дубликатов.
- Таймауты и ретраи при вызове SMTP/Telegram; маскирование токенов в логах.
- Локаль шаблонов, поддержка плейсхолдеров: {FullName}, {ReservationId}, {HotelName}, {CheckInDate}, {CheckOutDate}, {HotelAddress}.

## DoD
- Миграции для шаблонов и очереди применены; `.env.example` содержит настройки SMTP/Telegram.
- Интеграционные тесты: генерация контента, обработка событий, ретрай после ошибки.
- Метрики отправок и ошибок; логирование с correlationId.
- OpenAPI для внутренних эндпоинтов актуален.

