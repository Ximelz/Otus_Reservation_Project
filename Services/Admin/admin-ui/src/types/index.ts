export interface HotelListItem {
  id: string;
  name: string;
  slug: string;
  city: string;
  stars: number;
  isActive: boolean;
  totalRooms: number;
  availableRooms: number;
  createdAt: string;
}

export interface HotelDetails {
  id: string;
  name: string;
  slug: string;
  stars: number;
  description: string;
  city: string;
  countryId: number;
  address: string;
  timezone: string;
  checkInTime: string;
  checkOutTime: string;
  phone: string;
  email: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  amenities: HotelAmenity[];
  policy: HotelPolicy | null;
}

export interface HotelAmenity {
  id: string;
  amenityType: number;
  customName: string | null;
  isAvailable: boolean;
}

export interface HotelPolicy {
  id: string;
  checkInTime: string;
  checkOutTime: string;
  earlyCheckInNote: string | null;
  lateCheckOutNote: string | null;
  cancellationPolicyText: string | null;
  confirmationPendingEnabled: boolean;
  autoConfirmRules: boolean;
  termsAndConditionsText: string | null;
  contactInstructions: string | null;
}

export interface RoomType {
  id: string;
  hotelId: string;
  code: string;
  name: string;
  description: string;
  capacity: number;
  capacityAdults: number;
  capacityChildren: number;
  bedConfiguration: string | null;
  baseAreaSqm: number;
  isActive: boolean;
  roomCount: number;
}

export interface Room {
  id: string;
  hotelId: string;
  hotelName: string;
  typeId: string;
  roomTypeName: string;
  number: string;
  floor: number;
  status: RoomStatus;
  housekeepingStatus: HousekeepingStatus;
  viewType: string | null;
  notes: string | null;
  isActive: boolean;
}

export enum RoomStatus {
  Available = 0,
  Occupied = 1,
  Reserved = 2,
  OutOfService = 3,
  Maintenance = 4,
}

export enum HousekeepingStatus {
  Clean = 0,
  Dirty = 1,
  Inspected = 2,
}

export interface RatePlan {
  id: string;
  hotelId: string;
  roomTypeId: string;
  roomTypeName: string;
  code: string;
  name: string;
  basePrice: number;
  currency: string;
  cancellationPolicyType: number;
  breakfastIncluded: boolean;
  prepaymentRequired: boolean;
  isDefault: boolean;
  isActive: boolean;
}

export interface DashboardSummary {
  totalHotels: number;
  totalRooms: number;
  availableRooms: number;
  occupiedRooms: number;
  reservedRooms: number;
  dirtyRooms: number;
  outOfServiceRooms: number;
  maintenanceRooms: number;
  occupancyRate: number;
  topRoomTypesByOccupancy: RoomTypeOccupancy[];
  hotelComparisons: HotelComparison[];
  recentActivity: ActivityItem[];
  generatedAt: string;
}

export interface RoomTypeOccupancy {
  roomTypeName: string;
  totalRooms: number;
  occupiedRooms: number;
  occupancyRate: number;
}

export interface HotelComparison {
  hotelId: string;
  hotelName: string;
  stars: number;
  totalRooms: number;
  availableRooms: number;
  occupiedRooms: number;
  occupancyRate: number;
  dirtyRooms: number;
  isActive: boolean;
}

export interface HousekeepingBoard {
  hotelId: string;
  hotelName: string;
  floors: FloorGroup[];
  summary: HousekeepingSummary;
}

export interface FloorGroup {
  floor: number;
  rooms: Room[];
}

export interface HousekeepingSummary {
  totalRooms: number;
  cleanRooms: number;
  dirtyRooms: number;
  inspectedRooms: number;
  outOfServiceRooms: number;
}

export interface ActivityItem {
  id: string;
  hotelId: string | null;
  activityType: string;
  description: string;
  entityType: string | null;
  entityId: string | null;
  timestamp: string;
  performedBy: string | null;
}

export interface LoginResponse {
  accessToken: string;
  expiresInSeconds: number;
}

export const RoomStatusLabels: Record<RoomStatus, string> = {
  [RoomStatus.Available]: 'Свободен',
  [RoomStatus.Occupied]: 'Занят',
  [RoomStatus.Reserved]: 'Забронирован',
  [RoomStatus.OutOfService]: 'Не в эксплуатации',
  [RoomStatus.Maintenance]: 'Обслуживание',
};

export const HousekeepingLabels: Record<HousekeepingStatus, string> = {
  [HousekeepingStatus.Clean]: 'Чисто',
  [HousekeepingStatus.Dirty]: 'Грязно',
  [HousekeepingStatus.Inspected]: 'Проверено',
};

export const AmenityLabels: Record<number, string> = {
  0: 'Wi-Fi', 1: 'Парковка', 2: 'Завтрак', 3: 'Фитнес-зал',
  4: 'Спа', 5: 'Трансфер', 6: 'Можно с питомцами', 7: 'Конференц-зал', 99: 'Другое',
};

export const CancellationLabels: Record<number, string> = {
  0: 'Бесплатная', 1: 'Умеренная', 2: 'Строгая', 3: 'Невозвратный',
};
