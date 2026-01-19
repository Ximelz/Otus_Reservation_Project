# CustomerService (Customer.Api)
## Назначение (продукт)
- Хранит расширенный профиль гостя: ФИО, контакты, предпочтения.
- Доступен менеджеру для связи с гостем и учёта пожеланий.

## Функционал и API
- `GET /api/customers/me` — получить профиль текущего пользователя.
- `PUT /api/customers/me` — обновить профиль (FullName, Email, Phone, Preferences).
- `GET /api/customers/{id}` (Manager/Admin) — поиск клиента по Id.

## Данные
- Таблица `CustomerProfiles`: Id (UUID), UserId, FullName, Email, Phone, Preferences (jsonb), CreatedAt, UpdatedAt.
- Индекс на `UserId` для быстрой выборки.

## События (опционально)
- `CustomerProfileUpdated` (CustomerId, ChangedAt) — для аудита/аналитики.

## Нефункциональные требования
- Авторизация по JWT; `GET/PUT me` доступны только владельцу профиля.
- Маскирование персональных данных в логах.
- Валидация контактов (email/phone) и размера Preferences.

## DoD
- Миграции применены, схема задокументирована.
- Юнит-тесты валидации и интеграционные тесты `GET/PUT me`.
- Роли на эндпоинтах настроены (Manager/Admin только для чужих профилей).
- OpenAPI актуален; `.env.example` содержит строку подключения.

