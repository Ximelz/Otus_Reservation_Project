import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { hotelsApi } from '../api/client';
import { Link } from 'react-router-dom';
import { Search, MapPin, Hotel, Plus } from 'lucide-react';
import { useState } from 'react';
import {
  Card, PageHeader, Badge, Input, SkeletonCard, EmptyState,
  Button, Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter,
} from '../components/ui';
import { t } from '../lib/i18n';

const emptyForm = {
  name: '',
  slug: '',
  city: '',
  address: '',
  stars: 3,
  phone: '',
  email: '',
  description: '',
  timezone: 'Europe/Moscow',
  checkInTime: '14:00',
  checkOutTime: '12:00',
  countryId: 643,
};

export function HotelsPage() {
  const [search, setSearch] = useState('');
  const [createOpen, setCreateOpen] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [error, setError] = useState('');

  const qc = useQueryClient();

  const { data: hotels, isLoading } = useQuery({
    queryKey: ['hotels', search],
    queryFn: () => hotelsApi.list({ search: search || undefined }),
  });

  const createMutation = useMutation({
    mutationFn: (data: typeof emptyForm) => hotelsApi.create(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['hotels'] });
      setCreateOpen(false);
      setForm(emptyForm);
      setError('');
    },
    onError: (err: unknown) => {
      const message = err instanceof Error ? err.message : 'Ошибка при создании отеля';
      setError(message);
    },
  });

  const handleCreate = () => {
    setError('');
    createMutation.mutate(form);
  };

  const set = (field: keyof typeof emptyForm) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) =>
      setForm(f => ({ ...f, [field]: field === 'stars' || field === 'countryId' ? Number(e.target.value) : e.target.value }));

  const count = hotels?.length ?? 0;

  return (
    <div className="space-y-6">
      <PageHeader
        title={t.hotels.title}
        subtitle={`${count} ${count === 1 ? 'отель' : count >= 2 && count <= 4 ? 'отеля' : 'отелей'}`}
        actions={
          <Button icon={Plus} onClick={() => setCreateOpen(true)}>Добавить отель</Button>
        }
      />

      <Input
        icon={Search}
        value={search}
        onChange={e => setSearch(e.target.value)}
        placeholder={t.hotels.search}
      />

      {isLoading ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {[...Array(3)].map((_, i) => (
            <SkeletonCard key={i} />
          ))}
        </div>
      ) : !hotels?.length ? (
        <EmptyState
          icon={Hotel}
          title={t.hotels.noHotels}
          description={t.hotels.noHotelsDesc}
        />
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {hotels.map(hotel => (
            <Link key={hotel.id} to={`/hotels/${hotel.id}`} className="block">
              <Card hover>
                <div className="flex items-start justify-between mb-3">
                  <div>
                    <h3 className="text-lg font-semibold text-fg">{hotel.name}</h3>
                    <div className="flex items-center gap-1 mt-1">
                      <MapPin size={12} className="text-fg-muted" />
                      <span className="text-sm text-fg-muted">{hotel.city}</span>
                    </div>
                  </div>
                  <Badge variant={hotel.isActive ? 'success' : 'critical'}>
                    {hotel.isActive ? t.hotels.active : t.hotels.inactive}
                  </Badge>
                </div>

                <div className="flex items-center gap-0.5 mb-4 text-gold">
                  {[...Array(hotel.stars)].map((_, i) => (
                    <span key={i}>★</span>
                  ))}
                </div>

                <div className="flex items-center justify-between text-sm">
                  <div>
                    <p className="text-xs text-fg-muted">{t.hotels.totalRooms}</p>
                    <p className="font-medium text-fg">{hotel.totalRooms}</p>
                  </div>
                  <div className="text-right">
                    <p className="text-xs text-fg-muted">{t.hotels.available}</p>
                    <p className="font-medium text-emerald">{hotel.availableRooms}</p>
                  </div>
                </div>
              </Card>
            </Link>
          ))}
        </div>
      )}

      {/* Create Hotel Dialog */}
      <Dialog open={createOpen} onOpenChange={setCreateOpen}>
        <DialogContent className="max-w-2xl">
          <DialogHeader>
            <DialogTitle>{t.hotels.addHotel}</DialogTitle>
          </DialogHeader>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm text-fg-muted mb-1">Название</label>
              <Input value={form.name} onChange={set('name')} placeholder="Название отеля" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Код</label>
              <Input value={form.slug} onChange={set('slug')} placeholder="hotel-slug (латиница)" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Город</label>
              <Input value={form.city} onChange={set('city')} placeholder="Город" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Адрес</label>
              <Input value={form.address} onChange={set('address')} placeholder="Адрес" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Звёзды</label>
              <select
                value={form.stars}
                onChange={set('stars')}
                className="w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring"
              >
                {[1, 2, 3, 4, 5].map(s => (
                  <option key={s} value={s}>{s} {s === 1 ? 'звезда' : s >= 2 && s <= 4 ? 'звезды' : 'звёзд'}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Телефон</label>
              <Input value={form.phone} onChange={set('phone')} placeholder="+7 (999) 123-45-67" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Email</label>
              <Input type="email" value={form.email} onChange={set('email')} placeholder="hotel@example.com" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Часовой пояс</label>
              <Input value={form.timezone} onChange={set('timezone')} placeholder="Europe/Moscow" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Время заезда</label>
              <Input value={form.checkInTime} onChange={set('checkInTime')} placeholder="14:00" />
            </div>
            <div>
              <label className="block text-sm text-fg-muted mb-1">Время выезда</label>
              <Input value={form.checkOutTime} onChange={set('checkOutTime')} placeholder="12:00" />
            </div>
            <div className="col-span-2">
              <label className="block text-sm text-fg-muted mb-1">Описание</label>
              <textarea
                value={form.description}
                onChange={set('description')}
                rows={3}
                placeholder="Описание отеля"
                className="w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring"
              />
            </div>
          </div>

          {error && (
            <p className="text-sm text-red mt-3">{error}</p>
          )}

          <DialogFooter>
            <Button variant="secondary" onClick={() => setCreateOpen(false)}>
              {t.common.cancel}
            </Button>
            <Button onClick={handleCreate} loading={createMutation.isPending}>
              {t.common.create}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
