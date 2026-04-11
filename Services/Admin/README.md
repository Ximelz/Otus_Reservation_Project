# Сервис администрирования отелей

Учебный проект в рамках курса OTUS.

**Автор:** Александр Ожерельев
**Почта:** a.ozherelev@urent.city
**Домашние работы:** https://github.com/progazz/homeWork

---

## О сервисе

Сервис администрирования — панель управления каталогом отелей, номерами, тарифами и операционным состоянием гостиниц. Состоит из backend API на ASP.NET Core и SPA-фронтенда на React.

### Возможности

- Управление отелями: создание, редактирование, деактивация
- Управление типами номеров и номерами
- Управление тарифными планами
- Операционный дашборд: KPI, загрузка, заезды и выезды
- Доска хаусекипинга: статусы уборки по этажам
- Лента активности по отелям
- Публикация integration events в RabbitMQ при изменении статусов номеров

---

## Стек

| Слой | Технологии |
|------|-----------|
| Backend | .NET 8, ASP.NET Core, EF Core 8 |
| Frontend | React 18, TypeScript 5, Vite 5, Tailwind CSS, Radix UI |
| БД | PostgreSQL 16 |
| Messaging | RabbitMQ + MassTransit |
| Авторизация | JWT Bearer |

---

## Структура

- **Admin.WebApi** — REST API с JWT-авторизацией и publish-эндпоинтами в RabbitMQ
- **Admin.Application** — слой приложения (бизнес-логика)
- **Admin.Domain** — доменный слой
- **Admin.Infrastructure** — инфраструктура (EF Core, HTTP-клиенты)
- **admin-ui** — SPA на React + Vite

---

## Запуск

### Требования

- .NET 8 SDK
- Node.js 20+
- Docker (PostgreSQL + RabbitMQ)

### 1. Инфраструктура

```bash
docker-compose up -d postgres rabbitmq
```

### 2. Backend

```bash
cd Services/Admin/Admin.WebApi
dotnet run --launch-profile http
```

API: `http://localhost:5009` · Swagger: `http://localhost:5009/swagger`

### 3. Frontend

```bash
cd Services/Admin/admin-ui
npm install
npm run dev
```

UI: `http://localhost:5173`

### 4. Вход

- Логин: `admin`
- Пароль: `change-me`

---

## Конфигурация

Все настройки — в `Admin.WebApi/appsettings.Development.json`:

- JWT: `Admin:Jwt:SigningKey`, `Issuer`, `Audience`
- CORS origins
- Подключение к PostgreSQL
- Подключение к RabbitMQ

---

## Команды разработки

```bash
# Сборка
dotnet build Services/Admin/Admin.WebApi/Admin.WebApi.csproj

# Тесты
dotnet test Services/Admin/Admin.xUnitTests/Admin.xUnitTests.csproj
```
