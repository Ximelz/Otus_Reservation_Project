import axios from 'axios';
import type { DashboardSummary, HotelDetails, HotelListItem, HousekeepingBoard, LoginResponse, RatePlan, Room, RoomType, ActivityItem } from '../types';

const api = axios.create({ baseURL: '/api/admin' });

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(err);
  }
);

export const authApi = {
  login: (username: string, password: string) =>
    api.post<LoginResponse>('/auth/login', { username, password }).then(r => r.data),
};

export const hotelsApi = {
  list: (params?: { search?: string; isActive?: boolean; stars?: number }) =>
    api.get<HotelListItem[]>('/hotels', { params }).then(r => r.data),
  get: (id: string) => api.get<HotelDetails>(`/hotels/${id}`).then(r => r.data),
  create: (data: Record<string, unknown>) => api.post<HotelDetails>('/hotels', data).then(r => r.data),
  update: (id: string, data: Record<string, unknown>) => api.put<HotelDetails>(`/hotels/${id}`, data).then(r => r.data),
  deactivate: (id: string) => api.patch(`/hotels/${id}/deactivate`),
  activate: (id: string) => api.patch(`/hotels/${id}/activate`),
  addAmenity: (hotelId: string, data: Record<string, unknown>) => api.post(`/hotels/${hotelId}/amenities`, data),
  removeAmenity: (hotelId: string, amenityId: string) => api.delete(`/hotels/${hotelId}/amenities/${amenityId}`),
  upsertPolicy: (hotelId: string, data: Record<string, unknown>) => api.put(`/hotels/${hotelId}/policy`, data),
};

export const roomTypesApi = {
  list: (hotelId: string) => api.get<RoomType[]>(`/hotels/${hotelId}/room-types`).then(r => r.data),
  create: (hotelId: string, data: Record<string, unknown>) => api.post<RoomType>(`/hotels/${hotelId}/room-types`, data).then(r => r.data),
  update: (hotelId: string, id: string, data: Record<string, unknown>) => api.put<RoomType>(`/hotels/${hotelId}/room-types/${id}`, data).then(r => r.data),
  delete: (hotelId: string, id: string) => api.delete(`/hotels/${hotelId}/room-types/${id}`),
};

export const roomsApi = {
  list: (hotelId: string, params?: Record<string, unknown>) => api.get<Room[]>(`/hotels/${hotelId}/rooms`, { params }).then(r => r.data),
  get: (id: string) => api.get<Room>(`/rooms/${id}`).then(r => r.data),
  create: (hotelId: string, data: Record<string, unknown>) => api.post<Room>(`/hotels/${hotelId}/rooms`, data).then(r => r.data),
  update: (id: string, data: Record<string, unknown>) => api.put<Room>(`/rooms/${id}`, data).then(r => r.data),
  changeStatus: (id: string, status: number) => api.patch(`/rooms/${id}/status`, { status }),
  changeHousekeeping: (id: string, housekeepingStatus: number) => api.patch(`/rooms/${id}/housekeeping`, { housekeepingStatus }),
  bulkHousekeeping: (roomIds: string[], housekeepingStatus: number) => api.patch('/rooms/bulk-housekeeping', { roomIds, housekeepingStatus }),
  delete: (id: string) => api.delete(`/rooms/${id}`),
};

export const ratePlansApi = {
  list: (hotelId: string) => api.get<RatePlan[]>(`/hotels/${hotelId}/rate-plans`).then(r => r.data),
  create: (hotelId: string, data: Record<string, unknown>) => api.post<RatePlan>(`/hotels/${hotelId}/rate-plans`, data).then(r => r.data),
  update: (hotelId: string, id: string, data: Record<string, unknown>) => api.put<RatePlan>(`/hotels/${hotelId}/rate-plans/${id}`, data).then(r => r.data),
  delete: (hotelId: string, id: string) => api.delete(`/hotels/${hotelId}/rate-plans/${id}`),
};

export const dashboardApi = {
  summary: () => api.get<DashboardSummary>('/dashboard').then(r => r.data),
  housekeeping: (hotelId: string) => api.get<HousekeepingBoard>('/dashboard/housekeeping', { params: { hotelId } }).then(r => r.data),
  activity: (params?: { hotelId?: string; limit?: number }) => api.get<ActivityItem[]>('/dashboard/activity', { params }).then(r => r.data),
};
