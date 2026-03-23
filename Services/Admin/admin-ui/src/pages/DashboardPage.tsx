import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../api/client';
import { Building2, DoorOpen, CheckCircle2, Users, Paintbrush, AlertTriangle } from 'lucide-react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { KpiCard, Card, CardHeader, CardTitle, CardContent, PageHeader, Badge, Skeleton } from '../components/ui';
import { Button } from '../components/ui';
import { t } from '../lib/i18n';
import { formatNumber, formatDateTime } from '../lib/format';

function DashboardSkeleton() {
  return (
    <div className="space-y-6">
      <div className="flex items-start justify-between mb-8">
        <div>
          <Skeleton className="h-8 w-48 mb-2" />
          <Skeleton className="h-4 w-64" />
        </div>
        <Skeleton className="h-6 w-32 rounded-full" />
      </div>
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
        {[...Array(6)].map((_, i) => (
          <Skeleton key={i} className="h-24" />
        ))}
      </div>
      <Skeleton className="h-20" />
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Skeleton className="h-72" />
        <Skeleton className="h-72" />
      </div>
      <Skeleton className="h-64" />
    </div>
  );
}

export function DashboardPage() {
  const { data, isLoading, error, refetch } = useQuery({
    queryKey: ['dashboard'],
    queryFn: dashboardApi.summary,
  });

  if (isLoading) return <DashboardSkeleton />;

  if (error || !data) {
    return (
      <div className="flex flex-col items-center justify-center py-20 space-y-4">
        <p className="text-red text-sm">{t.common.error}</p>
        <Button variant="secondary" onClick={() => refetch()}>
          {t.common.retry}
        </Button>
      </div>
    );
  }

  const chartData = data.hotelComparisons.map(h => ({
    name: h.hotelName.split(' ')[0],
    [t.dashboard.available]: h.availableRooms,
    [t.dashboard.occupied]: h.occupiedRooms,
    [t.dashboard.dirty]: h.dirtyRooms,
  }));

  const activityDotColor = (type: string) => {
    if (type.includes('Created')) return 'bg-emerald';
    if (type.includes('Changed')) return 'bg-amber';
    return 'bg-gold';
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <PageHeader
        title={t.dashboard.title}
        subtitle={t.dashboard.subtitle}
        actions={<Badge variant="success">{t.dashboard.systemOnline}</Badge>}
      />

      {/* KPI Grid */}
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
        <KpiCard icon={Building2} label={t.dashboard.hotels} value={formatNumber(data.totalHotels)} color="text-gold" />
        <KpiCard icon={DoorOpen} label={t.dashboard.totalRooms} value={formatNumber(data.totalRooms)} color="text-fg" />
        <KpiCard icon={CheckCircle2} label={t.dashboard.available} value={formatNumber(data.availableRooms)} color="text-emerald" />
        <KpiCard icon={Users} label={t.dashboard.occupied} value={formatNumber(data.occupiedRooms)} color="text-amber" />
        <KpiCard icon={Paintbrush} label={t.dashboard.dirty} value={formatNumber(data.dirtyRooms)} color="text-red" />
        <KpiCard icon={AlertTriangle} label={t.dashboard.outOfService} value={formatNumber(data.outOfServiceRooms)} color="text-red" />
      </div>

      {/* Overall Occupancy */}
      <Card>
        <CardHeader>
          <CardTitle>{t.dashboard.overallOccupancy}</CardTitle>
          <span className="text-2xl font-bold text-gold">{data.occupancyRate}%</span>
        </CardHeader>
        <div className="w-full h-2 rounded-full bg-bg-muted">
          <div
            className="h-2 rounded-full bg-gold transition-all"
            style={{ width: `${data.occupancyRate}%` }}
          />
        </div>
      </Card>

      {/* Two-column: Chart + Hotels Overview */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Bar Chart */}
        <Card>
          <CardHeader>
            <CardTitle>{t.dashboard.hotelComparison}</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-border" />
                <XAxis dataKey="name" className="text-fg-muted" fontSize={12} tick={{ fill: 'currentColor' }} />
                <YAxis className="text-fg-muted" fontSize={12} tick={{ fill: 'currentColor' }} />
                <Tooltip
                  contentStyle={{
                    backgroundColor: 'var(--color-bg-card)',
                    border: '1px solid var(--color-border)',
                    borderRadius: 8,
                    color: 'var(--color-fg)',
                  }}
                />
                <Bar dataKey={t.dashboard.available} fill="#22c55e" name={t.dashboard.available} radius={[4, 4, 0, 0]} />
                <Bar dataKey={t.dashboard.occupied} fill="#f59e0b" name={t.dashboard.occupied} radius={[4, 4, 0, 0]} />
                <Bar dataKey={t.dashboard.dirty} fill="#ef4444" name={t.dashboard.dirty} radius={[4, 4, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Hotels Overview */}
        <Card>
          <CardHeader>
            <CardTitle>{t.dashboard.hotelsOverview}</CardTitle>
          </CardHeader>
          <CardContent>
            {data.hotelComparisons.map(h => (
              <div
                key={h.hotelId}
                className="p-4 rounded-lg bg-bg-muted border border-border flex items-center justify-between"
              >
                <div>
                  <p className="text-sm font-medium text-fg">{h.hotelName}</p>
                  <p className="text-xs text-fg-muted">
                    {'★'.repeat(h.stars)} · {h.totalRooms} {t.dashboard.rooms}
                  </p>
                </div>
                <div className="text-right">
                  <p className="text-lg font-bold text-gold">{h.occupancyRate}%</p>
                  <p className="text-xs text-fg-muted">
                    {h.occupiedRooms}/{h.totalRooms} {t.dashboard.occupiedRooms}
                  </p>
                </div>
              </div>
            ))}
          </CardContent>
        </Card>
      </div>

      {/* Recent Activity */}
      <Card>
        <CardHeader>
          <CardTitle>{t.dashboard.recentActivity}</CardTitle>
        </CardHeader>
        <CardContent>
          {data.recentActivity.slice(0, 8).map(a => (
            <div key={a.id} className="flex items-start gap-3 text-sm">
              <div className={`w-2 h-2 mt-1.5 rounded-full flex-shrink-0 ${activityDotColor(a.activityType)}`} />
              <div className="flex-1">
                <p className="text-fg">{a.description}</p>
                <p className="text-xs text-fg-muted mt-0.5">
                  {formatDateTime(a.timestamp)} · {a.performedBy || 'system'}
                </p>
              </div>
            </div>
          ))}
        </CardContent>
      </Card>
    </div>
  );
}
