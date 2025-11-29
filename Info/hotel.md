# HotelService (Hotel.Api)
## Назначение (продукт)
- Справочник отелей и номерного фонда.
- Базовые тарифы и простые сезонные коэффициенты для расчёта цены.

## Функционал и API
- CRUD для отелей/номеров/тарифов (Admin/Manager):
  - `POST/PUT/GET/DELETE /api/hotels`
  - `POST/PUT/GET/DELETE /api/hotels/{id}/room-types`
  - `POST/PUT/GET/DELETE /api/hotels/{id}/rooms`
  - `POST/PUT/GET/DELETE /api/hotels/{id}/rate-plans`
  - `POST/PUT/GET/DELETE /api/hotels/{id}/season-prices`
- Публичные чтения:
  - `GET /api/hotels` — список отелей.
  - `GET /api/hotels/{id}` — отель с номерным фондом и базовыми тарифами.
  - `GET /api/hotels/{id}/availability?checkIn=&checkOut=&guests=` — доступность/цены (используется UI и ReservationService).

## Данные
- `Hotels` (Id, Name, Address, City, Timezone, CheckInTime, CheckOutTime, Description, CreatedAt, UpdatedAt).
- `RoomTypes` (Id, HotelId, Name, Capacity, Description, AmenitiesJson, CreatedAt, UpdatedAt).
- `Rooms` (Id, HotelId, RoomTypeId, RoomNumber, Floor, Status, CreatedAt, UpdatedAt) с уникальным `(HotelId, RoomNumber)`.
- `RatePlans` (Id, HotelId, RoomTypeId, Name, BasePrice, Currency, CancellationPolicyType, CreatedAt, UpdatedAt).
- `SeasonPrices` (Id, HotelId, RoomTypeId, DateFrom, DateTo, PriceOverride, Multiplier, CreatedAt, UpdatedAt).

## События (опционально)
- `HotelUpdated` / `RoomInventoryChanged` для кешей и аналитики.

## Нефункциональные требования
- Индексы: (HotelId, RoomTypeId) на тарифы, (HotelId, RoomTypeId, DateFrom, DateTo) на сезонные цены.
- Проверка пересечения периодов сезонных цен; валидация уникальности номера.
- Возможность кэширования справочников по отелю.

## DoD
- Все миграции применены; справочные данные seed по необходимости.
- Интеграционные тесты CRUD и публичных GET; проверка доступности с датами.
- Роли на write-операциях (Admin/Manager), публичные GET без авторизации.
- OpenAPI актуален; `.env.example` содержит строку подключения.

