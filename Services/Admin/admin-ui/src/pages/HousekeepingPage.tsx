import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { dashboardApi, hotelsApi, roomsApi } from '../api/client';
import { useState } from 'react';
import { RoomStatus, HousekeepingStatus, RoomStatusLabels, HousekeepingLabels } from '../types';
import { t } from '../lib/i18n';
import { Building2, SparklesIcon, AlertTriangle, SearchCheck, Ban } from 'lucide-react';
import { Card, CardTitle, PageHeader, Badge, KpiCard, Skeleton } from '../components/ui';

function roomBorderClass(status: RoomStatus): string {
  const map: Record<RoomStatus, string> = {
    [RoomStatus.Available]: 'border-emerald/30',
    [RoomStatus.Occupied]: 'border-amber/30',
    [RoomStatus.Reserved]: 'border-blue/30',
    [RoomStatus.OutOfService]: 'border-red/30 bg-red-muted/30',
    [RoomStatus.Maintenance]: 'border-border',
  };
  return map[status] ?? 'border-border';
}

function roomStatusVariant(s: RoomStatus): 'success' | 'warning' | 'info' | 'critical' | 'default' {
  const map = {
    [RoomStatus.Available]: 'success',
    [RoomStatus.Occupied]: 'warning',
    [RoomStatus.Reserved]: 'info',
    [RoomStatus.OutOfService]: 'critical',
    [RoomStatus.Maintenance]: 'default',
  } as const;
  return map[s] || 'default';
}

function hkTextClass(hk: HousekeepingStatus): string {
  const map: Record<HousekeepingStatus, string> = {
    [HousekeepingStatus.Clean]: 'text-emerald',
    [HousekeepingStatus.Dirty]: 'text-red',
    [HousekeepingStatus.Inspected]: 'text-blue',
  };
  return map[hk] ?? 'text-fg-muted';
}

export function HousekeepingPage() {
  const [hotelId, setHotelId] = useState<string>('');
  const qc = useQueryClient();
  const { data: hotels } = useQuery({ queryKey: ['hotels'], queryFn: () => hotelsApi.list() });
  const { data: board, isLoading } = useQuery({
    queryKey: ['housekeeping', hotelId],
    queryFn: () => dashboardApi.housekeeping(hotelId),
    enabled: !!hotelId,
  });

  const hkMut = useMutation({
    mutationFn: ({ id, hk }: { id: string; hk: number }) => roomsApi.changeHousekeeping(id, hk),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['housekeeping', hotelId] }),
  });

  // Auto-select first hotel
  if (!hotelId && hotels?.length) setHotelId(hotels[0].id);

  return (
    <div className="space-y-6">
      <PageHeader
        title={t.housekeeping.title}
        subtitle={t.housekeeping.subtitle}
        actions={
          <select
            value={hotelId}
            onChange={e => setHotelId(e.target.value)}
            className="bg-bg-card text-fg border border-border rounded-lg px-3 py-2 text-sm outline-none"
          >
            {hotels?.map(h => (
              <option key={h.id} value={h.id}>{h.name}</option>
            ))}
          </select>
        }
      />

      {board && (
        <div className="grid grid-cols-2 md:grid-cols-5 gap-3">
          <KpiCard icon={Building2} label={t.housekeeping.total} value={board.summary.totalRooms} color="text-fg" />
          <KpiCard icon={SparklesIcon} label={t.housekeeping.clean} value={board.summary.cleanRooms} color="text-emerald" />
          <KpiCard icon={AlertTriangle} label={t.housekeeping.dirty} value={board.summary.dirtyRooms} color="text-red" />
          <KpiCard icon={SearchCheck} label={t.housekeeping.inspected} value={board.summary.inspectedRooms} color="text-blue" />
          <KpiCard icon={Ban} label={t.housekeeping.outOfService} value={board.summary.outOfServiceRooms} color="text-amber" />
        </div>
      )}

      {isLoading && (
        <div className="space-y-4">
          <Skeleton className="h-64" />
        </div>
      )}

      {board?.floors.map(floor => (
        <Card key={floor.floor}>
          <CardTitle>{t.housekeeping.floor} {floor.floor}</CardTitle>
          <div className="grid grid-cols-4 sm:grid-cols-6 md:grid-cols-8 gap-3 mt-4">
            {floor.rooms.map(room => (
              <div
                key={room.id}
                className={`p-3 rounded-lg border text-center cursor-pointer transition-all hover:scale-105 ${roomBorderClass(room.status)}`}
                onClick={() => {
                  const nextHk =
                    room.housekeepingStatus === HousekeepingStatus.Dirty ? HousekeepingStatus.Clean
                    : room.housekeepingStatus === HousekeepingStatus.Clean ? HousekeepingStatus.Inspected
                    : HousekeepingStatus.Dirty;
                  hkMut.mutate({ id: room.id, hk: nextHk });
                }}
              >
                <p className="text-lg font-bold text-fg">{room.number}</p>
                <Badge variant={roomStatusVariant(room.status)} className="mt-1 text-[10px]">
                  {RoomStatusLabels[room.status]}
                </Badge>
                <p className={`text-[10px] mt-1 font-medium ${hkTextClass(room.housekeepingStatus)}`}>
                  {HousekeepingLabels[room.housekeepingStatus]}
                </p>
              </div>
            ))}
          </div>
        </Card>
      ))}
    </div>
  );
}
