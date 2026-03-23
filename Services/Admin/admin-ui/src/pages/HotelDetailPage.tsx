import { useQuery } from '@tanstack/react-query';
import { useParams, Link } from 'react-router-dom';
import { hotelsApi, roomTypesApi } from '../api/client';
import { MapPin, Clock, Phone, Mail, Bed, DollarSign } from 'lucide-react';
import { AmenityLabels } from '../types';
import { Card, CardHeader, CardTitle, CardContent, PageHeader, Badge, Skeleton } from '../components/ui';
import { t } from '../lib/i18n';

export function HotelDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { data: hotel, isLoading } = useQuery({
    queryKey: ['hotel', id],
    queryFn: () => hotelsApi.get(id!),
  });
  const { data: roomTypes } = useQuery({
    queryKey: ['roomTypes', id],
    queryFn: () => roomTypesApi.list(id!),
    enabled: !!id,
  });

  if (isLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-10 w-1/3" />
        <Skeleton className="h-4 w-1/4" />
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 space-y-6">
            <Skeleton className="h-48 w-full" />
            <Skeleton className="h-40 w-full" />
          </div>
          <div className="space-y-6">
            <Skeleton className="h-32 w-full" />
            <Skeleton className="h-32 w-full" />
          </div>
        </div>
      </div>
    );
  }

  if (!hotel) return <p className="text-fg-muted">{t.common.noData}</p>;

  const starsStr = '★'.repeat(hotel.stars);

  return (
    <div className="space-y-6">
      <PageHeader
        title={hotel.name}
        subtitle={`${starsStr} · ${hotel.city}`}
        backTo="/hotels"
        actions={
          <Badge variant={hotel.isActive ? 'success' : 'critical'}>
            {hotel.isActive ? t.hotels.active : t.hotels.inactive}
          </Badge>
        }
      />

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Left column */}
        <div className="lg:col-span-2 space-y-6">
          {/* Details Card */}
          <Card>
            <CardHeader>
              <CardTitle>{t.hotels.details}</CardTitle>
            </CardHeader>
            <CardContent>
              {hotel.description && (
                <p className="text-sm text-fg-muted">{hotel.description}</p>
              )}
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 text-sm">
                <div className="flex items-center gap-2">
                  <MapPin size={14} className="text-fg-muted shrink-0" />
                  <div>
                    <span className="text-fg-muted">Адрес: </span>
                    <span className="text-fg">{hotel.address}</span>
                  </div>
                </div>
                <div className="flex items-center gap-2">
                  <Phone size={14} className="text-fg-muted shrink-0" />
                  <div>
                    <span className="text-fg-muted">Телефон: </span>
                    <span className="text-fg">{hotel.phone}</span>
                  </div>
                </div>
                <div className="flex items-center gap-2">
                  <Mail size={14} className="text-fg-muted shrink-0" />
                  <div>
                    <span className="text-fg-muted">Email: </span>
                    <span className="text-fg">{hotel.email}</span>
                  </div>
                </div>
                <div className="flex items-center gap-2">
                  <Clock size={14} className="text-fg-muted shrink-0" />
                  <div>
                    <span className="text-fg-muted">{t.hotels.checkIn}: </span>
                    <span className="text-fg">{hotel.checkInTime}</span>
                    <span className="text-fg-muted"> / {t.hotels.checkOut}: </span>
                    <span className="text-fg">{hotel.checkOutTime}</span>
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>

          {/* Room Types Card */}
          <Card>
            <CardHeader>
              <CardTitle>{t.hotels.roomTypes}</CardTitle>
              <Link
                to={`/hotels/${hotel.id}/rooms`}
                className="text-xs text-gold hover:text-gold/80 transition-colors"
              >
                {t.hotels.viewRooms}
              </Link>
            </CardHeader>
            <CardContent>
              {roomTypes?.map(rt => (
                <div
                  key={rt.id}
                  className="flex items-center justify-between p-3 rounded-lg bg-bg-muted"
                >
                  <div>
                    <p className="text-sm font-medium text-fg">
                      {rt.name}{' '}
                      <span className="font-normal text-xs text-fg-muted">({rt.code})</span>
                    </p>
                    <p className="text-xs text-fg-muted">
                      {rt.capacityAdults} {t.hotels.adults} · {rt.capacityChildren} {t.hotels.children} · {rt.baseAreaSqm} м²
                      {rt.bedConfiguration && ` · ${rt.bedConfiguration}`}
                    </p>
                  </div>
                  <Badge variant="gold">{rt.roomCount} номеров</Badge>
                </div>
              ))}
            </CardContent>
          </Card>
        </div>

        {/* Right column */}
        <div className="space-y-6">
          {/* Amenities Card */}
          <Card>
            <CardHeader>
              <CardTitle>{t.hotels.amenities}</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex flex-wrap gap-2">
                {hotel.amenities.map(a => (
                  <Badge
                    key={a.id}
                    variant={a.isAvailable ? 'gold' : 'default'}
                  >
                    {a.customName || AmenityLabels[a.amenityType] || 'Другое'}
                  </Badge>
                ))}
              </div>
            </CardContent>
          </Card>

          {/* Policy Card */}
          {hotel.policy && (
            <Card>
              <CardHeader>
                <CardTitle>{t.hotels.policy}</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-2 text-xs text-fg-muted">
                  {hotel.policy.cancellationPolicyText && (
                    <div>
                      <span className="text-fg-muted font-medium">{t.hotels.cancellationPolicy}: </span>
                      <span className="text-fg">{hotel.policy.cancellationPolicyText}</span>
                    </div>
                  )}
                  {hotel.policy.earlyCheckInNote && (
                    <div>
                      <span className="text-fg-muted font-medium">{t.hotels.earlyCheckIn}: </span>
                      <span className="text-fg">{hotel.policy.earlyCheckInNote}</span>
                    </div>
                  )}
                  {hotel.policy.lateCheckOutNote && (
                    <div>
                      <span className="text-fg-muted font-medium">{t.hotels.lateCheckOut}: </span>
                      <span className="text-fg">{hotel.policy.lateCheckOutNote}</span>
                    </div>
                  )}
                  {hotel.policy.termsAndConditionsText && (
                    <div>
                      <span className="text-fg-muted font-medium">{t.hotels.terms}: </span>
                      <span className="text-fg">{hotel.policy.termsAndConditionsText}</span>
                    </div>
                  )}
                </div>
              </CardContent>
            </Card>
          )}

          {/* Quick Links Card */}
          <Card>
            <CardContent>
              <Link
                to={`/hotels/${hotel.id}/rooms`}
                className="flex items-center gap-2 p-3 rounded-lg text-sm text-fg hover:bg-bg-muted transition-colors"
              >
                <Bed size={16} className="text-gold" />
                {t.hotels.manageRooms}
              </Link>
              <Link
                to={`/hotels/${hotel.id}/rate-plans`}
                className="flex items-center gap-2 p-3 rounded-lg text-sm text-fg hover:bg-bg-muted transition-colors"
              >
                <DollarSign size={16} className="text-gold" />
                {t.hotels.ratePlans}
              </Link>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
