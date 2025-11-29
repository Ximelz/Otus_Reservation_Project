# SchedulerService (Scheduler.Api / Worker)
## Назначение (продукт)
- Управляет отложенными действиями: напоминания о заезде, отметка NoShow, автоотмена.
- Позволяет планировать/отменять задачи, исполняет их по времени.

## Функционал и API
- Планирование задач (внутренний вызов или подписка на события):
  - На `ReservationCreated` создаёт `SendCheckInReminder` (-24h) и `MarkNoShow` (вечер дня заезда).
- Внутренние/технические эндпоинты (пример):
  - `POST /api/scheduler/jobs` — создать/обновить задачу.
  - `POST /api/scheduler/jobs/{id}/cancel` — отменить задачу.
- Worker:
  - Каждую минуту выбирает due-задачи со статусом `Pending`, ставит `InProgress`, публикует событие, помечает `Completed` или увеличивает `RetryCount/LastError`.

## Данные
- `ScheduledJobs`: Id, Type, Payload (jsonb), ExecuteAt (timestamptz), Status (Pending/InProgress/Completed/Failed/Cancelled), RetryCount, LastError, CreatedAt, UpdatedAt.
- Индекс на (Status, ExecuteAt).

## События
- Потребляет: `ReservationCreated`, `ReservationCancelled`.
- Публикует: `ReminderDue` (ReservationId, CustomerId, HotelId, CheckInDate), инициирует `MarkNoShow`.

## Нефункциональные требования
- Идемпотентность: JobId как ключ дедупликации при публикации.
- Ретрай с джиттером; защита от «залипания» InProgress (ревалидировать по таймеру).
- Healthchecks `/live`, `/ready`.

## DoD
- Миграции для `ScheduledJobs` применены, индексы созданы.
- Интеграционные тесты воркера: happy path, retry, cancel по событию отмены.
- Конфиги таймингов/ретраев вынесены в settings и `.env.example`.
- Метрики по количеству задач, ошибкам, времени выполнения; логирование JobId.
- OpenAPI для технических эндпоинтов актуален.

