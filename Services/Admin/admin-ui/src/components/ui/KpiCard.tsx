import type { LucideIcon } from 'lucide-react';

interface KpiCardProps { icon: LucideIcon; label: string; value: string | number; color?: string; }

export function KpiCard({ icon: Icon, label, value, color = 'text-fg' }: KpiCardProps) {
  return (
    <div className="bg-bg-card border border-border rounded-xl p-4 flex items-center gap-4">
      <div className="p-2.5 rounded-lg bg-bg-muted">
        <Icon size={20} className={color} />
      </div>
      <div>
        <p className="text-xs font-medium text-fg-muted uppercase tracking-wider">{label}</p>
        <p className={`text-2xl font-bold ${color}`}>{value}</p>
      </div>
    </div>
  );
}
