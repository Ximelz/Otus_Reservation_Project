# ReservationService (Reserve.Api)
## Назначение (продукт)
- Принимает, хранит и управляет бронированиями.
- Гарантирует отсутствие овербукинга за счёт уникальности `(RoomId, StayDate)`.
- Обрабатывает отмены, check-in, check-out, статус NoShow.

## Функционал и API
- Создание брони: `POST /api/reservations` (HotelId, RoomTypeId/RatePlanId, CheckInDate, CheckOutDate, GuestsCount).
- Получение: `GET /api/reservations/{id}`, `GET /api/reservations/my`.
- Изменение статусов:
  - `POST /api/reservations/{id}/cancel`
  - `POST /api/reservations/{id}/check-in`
  - `POST /api/reservations/{id}/check-out`
- Взаимодействие:
  - Запрос доступности/номеров в HotelService.
  - Публикация событий в RabbitMQ.

## Данные
- `Reservations` (Id, CustomerId, HotelId, Status, CheckInDate, CheckOutDate, GuestsCount, TotalPrice, Currency, CreatedAt, UpdatedAt).
- `ReservationRooms` (Id, ReservationId, RoomId, StayDate, CreatedAt, UpdatedAt) с уникальным индексом `(RoomId, StayDate)`.

## События
- `ReservationCreated` (ReservationId, CustomerId, HotelId, Dates, TotalPrice).
- `ReservationCancelled` (ReservationId, Reason, CancelledAt).
- `ReservationCheckedIn`, `ReservationCheckedOut`.
- `NoShowMarked` (по задаче планировщика).

## Нефункциональные требования
- Транзакционное создание: вставка `ReservationRooms`, откат при конфликте уникального индекса.
- Корреляция сообщений по ReservationId; идемпотентные паблишеры/хендлеры.
- Маппинг доменных ошибок в HTTP (`400/404/409`), логирование бизнес-событий.

## DoD
- Миграции и уникальные индексы применены.
- Интеграционные тесты: успешное бронирование, конфликт 409 при занятом слоте, отмена освобождает слот, check-in/out статусные переходы.
- События публикуются в RabbitMQ с идемпотентным ключом.
- OpenAPI актуален; `.env.example` содержит строку подключения и настройки RabbitMQ.

