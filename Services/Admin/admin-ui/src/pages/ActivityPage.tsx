import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../api/client';
import { Activity, Building2, DoorOpen, Paintbrush, DollarSign, LayoutGrid } from 'lucide-react';
import { t } from '../lib/i18n';
import { formatDateTime } from '../lib/format';
import { Card, PageHeader, Badge, EmptyState, Skeleton } from '../components/ui';

interface ActivityStyle {
  icon: React.ElementType;
  bgClass: string;
  iconClass: string;
}

function getActivityStyle(type: string): ActivityStyle {
  switch (type) {
    case 'HotelCreated':
    case 'HotelUpdated':
      return { icon: Building2, bgClass: 'bg-emerald-muted', iconClass: 'text-emerald' };
    case 'RoomStatusChanged':
      return { icon: DoorOpen, bgClass: 'bg-amber-muted', iconClass: 'text-amber' };
    case 'HousekeepingChanged':
      return { icon: Paintbrush, bgClass: 'bg-blue-muted', iconClass: 'text-blue' };
    case 'RatePlanCreated':
      return { icon: DollarSign, bgClass: 'bg-gold/10', iconClass: 'text-gold' };
    case 'RoomTypeCreated':
      return { icon: LayoutGrid, bgClass: 'bg-emerald-muted', iconClass: 'text-emerald' };
    default:
      return { icon: Activity, bgClass: 'bg-bg-muted', iconClass: 'text-fg-muted' };
  }
}

export function ActivityPage() {
  const { data: activities, isLoading } = useQuery({
    queryKey: ['activity'],
    queryFn: () => dashboardApi.activity({ limit: 100 }),
  });

  return (
    <div className="space-y-6">
      <PageHeader
        title={t.activity.title}
        subtitle={t.activity.subtitle}
      />

      {isLoading ? (
        <div className="space-y-3">
          {[...Array(8)].map((_, i) => (
            <Skeleton key={i} className="h-16" />
          ))}
        </div>
      ) : !activities?.length ? (
        <EmptyState icon={Activity} title={t.activity.noActivity} description={t.activity.noActivityDesc} />
      ) : (
        <div className="space-y-2">
          {activities.map(a => {
            const style = getActivityStyle(a.activityType);
            const Icon = style.icon;
            return (
              <Card key={a.id} className="!p-4">
                <div className="flex items-start gap-4">
                  <div className={`p-2 rounded-lg flex-shrink-0 ${style.bgClass}`}>
                    <Icon size={16} className={style.iconClass} />
                  </div>
                  <div className="flex-1 min-w-0">
                    <p className="text-sm text-fg">{a.description}</p>
                    <p className="text-xs text-fg-muted mt-1">{formatDateTime(a.timestamp)}</p>
                  </div>
                  <div className="flex flex-col items-end gap-1 flex-shrink-0">
                    <Badge>
                      {t.activity.types[a.activityType] ?? a.activityType}
                    </Badge>
                    {a.performedBy && (
                      <span className="text-xs text-fg-muted">{a.performedBy}</span>
                    )}
                  </div>
                </div>
              </Card>
            );
          })}
        </div>
      )}
    </div>
  );
}
