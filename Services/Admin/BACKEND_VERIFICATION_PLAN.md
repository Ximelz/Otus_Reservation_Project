# План проверки Backend сервисов

## Цель
Проверить соответствие контрактов, спецификаций, БД и интеграции фронтенда с бэкендом.

## Область проверки

### 1. Backend сервисы
- ✅ Admin.WebApi (порт 5009/7009)
- ✅ Hotels.WebApi (порт 5001)

### 2. Frontend
- ✅ Admin.Ui (порт 5008/7008)

## План проверки

### Этап 1: Проверка API контрактов

#### 1.1 Admin.WebApi контроллеры
- [ ] AdminAuthController
  - [ ] POST /api/admin/auth/login
- [ ] AdminSettingsController
  - [ ] GET /api/admin/settings
  - [ ] GET /api/admin/settings/{key}
  - [ ] PUT /api/admin/settings/{key}
  - [ ] DELETE /api/admin/settings/{key}
- [ ] AdminDashboardController
  - [ ] GET /api/admin/dashboard?countryId={id}&stars={n}
- [ ] AdminHotelsController
  - [ ] GET /api/admin/hotels/{id}
  - [ ] GET /api/admin/hotels/byCountry/{countryId}
  - [ ] GET /api/admin/hotels/byStars/{stars}
  - [ ] POST /api/admin/hotels
  - [ ] DELETE /api/admin/hotels/{id}
- [ ] AdminRoomsController
  - [ ] GET /api/admin/rooms/{id}
  - [ ] GET /api/admin/rooms/hotel/{hotelId}
  - [ ] POST /api/admin/rooms
  - [ ] DELETE /api/admin/rooms/{id}
  - [ ] DELETE /api/admin/rooms/hotel/{hotelId}

#### 1.2 Hotels.WebApi контроллеры
- [ ] HotelsController
  - [ ] GET /api/hotels/{id}
  - [ ] GET /api/hotels/byCountry/{countryId}
  - [ ] GET /api/hotels/byStars/{stars}
  - [ ] POST /api/hotels
  - [ ] DELETE /api/hotels/{id}
- [ ] RoomsController
  - [ ] GET /api/rooms/{id}
  - [ ] GET /api/rooms/hotel/{id}
  - [ ] POST /api/rooms
  - [ ] DELETE /api/rooms/{id}
  - [ ] DELETE /api/rooms/hotel/{hotelId}

### Этап 2: Проверка соответствия Frontend и Backend

#### 2.1 AdminApiClient (UI) → Admin.WebApi
- [ ] Login() → POST /api/admin/auth/login
- [ ] GetSettings() → GET /api/admin/settings
- [ ] UpsertSetting() → PUT /api/admin/settings/{key}
- [ ] DeleteSetting() → DELETE /api/admin/settings/{key}
- [ ] GetDashboard() → GET /api/admin/dashboard
- [ ] GetHotelsByCountry() → GET /api/admin/hotels/byCountry/{countryId}
- [ ] GetHotelsByStars() → GET /api/admin/hotels/byStars/{stars}
- [ ] UpsertHotel() → POST /api/admin/hotels
- [ ] DeleteHotel() → DELETE /api/admin/hotels/{id}
- [ ] GetRoomsByHotel() → GET /api/admin/rooms/hotel/{hotelId}
- [ ] UpsertRoom() → POST /api/admin/rooms
- [ ] DeleteRoom() → DELETE /api/admin/rooms/{id}

#### 2.2 HotelsClient (Admin.WebApi) → Hotels.WebApi
- [ ] GetHotel() → GET /api/hotels/{id}
- [ ] GetHotelsByCountry() → GET /api/hotels/byCountry/{countryId}
- [ ] GetHotelsByStars() → GET /api/hotels/byStars/{stars}
- [ ] UpsertHotel() → POST /api/hotels
- [ ] DeleteHotel() → DELETE /api/hotels/{id}
- [ ] GetRoom() → GET /api/rooms/{id}
- [ ] GetRoomsByHotel() → GET /api/rooms/hotel/{hotelId}
- [ ] UpsertRoom() → POST /api/rooms
- [ ] DeleteRoom() → DELETE /api/rooms/{id}
- [ ] DeleteRoomsByHotel() → DELETE /api/rooms/hotel/{hotelId}

### Этап 3: Проверка DTOs и моделей

#### 3.1 AdminLoginRequest/Response
- [ ] Admin.WebApi.Models vs Admin.Ui.Models

#### 3.2 HotelDto
- [ ] Admin.Application.Contracts vs Admin.Ui.Models
- [ ] Соответствие Hotels.Domain.Entities.Hotel

#### 3.3 RoomDto
- [ ] Admin.Application.Contracts vs Admin.Ui.Models
- [ ] Соответствие Hotels.Domain.Entities.Room

#### 3.4 SystemSetting
- [ ] Admin.Domain.Entities vs Admin.Ui.Models

#### 3.5 AdminDashboardResponse
- [ ] Admin.WebApi.Models vs Admin.Ui.Models

### Этап 4: Проверка БД схем

#### 4.1 Admin.Infrastructure
- [ ] AdminDbContext - SystemSettings
- [ ] Миграции (если есть)

#### 4.2 Hotels.Infrastructure
- [ ] DatabaseContext - Hotels, Rooms
- [ ] Миграции (если есть)

### Этап 5: Проверка конфигурации

#### 5.1 Admin.WebApi
- [ ] appsettings.json
- [ ] appsettings.Development.json
- [ ] Endpoints, CORS, JWT настройки

#### 5.2 Admin.Ui
- [ ] appsettings.json
- [ ] appsettings.Development.json
- [ ] BaseUrl, DevAuth credentials

#### 5.3 Hotels.WebApi
- [ ] appsettings.json
- [ ] appsettings.Development.json
- [ ] DbConfiguration.json

### Этап 6: Проверка спецификаций

#### 6.1 OpenAPI/Swagger
- [ ] Admin.WebApi - Swagger документация
- [ ] Hotels.WebApi - Swagger документация (если есть)

#### 6.2 Архитектурная документация
- [ ] Info/admin.md
- [ ] Info/hotel.md
- [ ] Info/architecture.md

## Критерии успеха

✅ Все эндпоинты соответствуют между слоями
✅ DTOs идентичны между слоями
✅ БД схемы соответствуют доменным моделям
✅ Конфигурация корректна
✅ Документация актуальна
