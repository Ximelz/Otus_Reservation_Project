import type { LucideIcon } from 'lucide-react';
import type { ReactNode } from 'react';

interface EmptyStateProps { icon: LucideIcon; title: string; description?: string; action?: ReactNode; }

export function EmptyState({ icon: Icon, title, description, action }: EmptyStateProps) {
  return (
    <div className="flex flex-col items-center justify-center py-16 text-center">
      <div className="p-4 rounded-2xl bg-bg-muted mb-4">
        <Icon size={32} className="text-fg-muted" />
      </div>
      <h3 className="text-lg font-medium text-fg mb-1">{title}</h3>
      {description && <p className="text-sm text-fg-muted max-w-sm mb-4">{description}</p>}
      {action}
    </div>
  );
}
