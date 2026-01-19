# IdentityService (Auth.Api)
## Назначение (продукт)
- Регистрация и аутентификация пользователей.
- Выдача/обновление JWT, хранение refresh-токенов.
- Управление ролями `Customer`, `Manager`, `Admin`.

## Функционал и API
- `POST /api/auth/register` — регистрация (email/пароль), создаёт пользователя с ролью `Customer` по умолчанию.
- `POST /api/auth/login` — логин, выдаёт access/refresh.
- `POST /api/auth/refresh` — обновление пары токенов.
- `GET /api/auth/me` — профиль текущего пользователя (Id, Email, Roles).
- Опционально: `POST /api/auth/roles/{userId}` (Admin) — назначение ролей.

## Данные
- ASP.NET Identity таблицы: `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, `AspNetUserTokens`, `AspNetRefreshTokens`.
- Пользователь связывается с профилем в CustomerService по `UserId`.

## События (опционально)
- `UserRegistered` (UserId, Email, Roles) — для последующей инициализации профиля.

## Нефункциональные требования
- JWT с настройками времени жизни access/refresh.
- Пароли хэшируются; запрет хранения секретов в коде.
- Логирование входов/ошибок с correlationId.

## DoD
- Миграции Identity применены; refresh-токены persist в БД.
- Конфигурация JWT вынесена в settings и `.env.example`.
- Валидация DTO и корректные коды ошибок (`400/401/409` при дубликате email).
- Интеграционный тест логина/refresh.
- OpenAPI актуален; базовые роли заданы seed-данными.

