import type { ReactNode } from 'react';
import { ArrowLeft } from 'lucide-react';
import { Link } from 'react-router-dom';

interface PageHeaderProps { title: string; subtitle?: string; backTo?: string; actions?: ReactNode; }

export function PageHeader({ title, subtitle, backTo, actions }: PageHeaderProps) {
  return (
    <div className="flex items-start justify-between mb-8">
      <div className="flex items-start gap-3">
        {backTo && (
          <Link to={backTo} className="mt-1 p-1.5 rounded-lg text-fg-muted hover:text-fg hover:bg-white/5 transition-colors">
            <ArrowLeft size={20} />
          </Link>
        )}
        <div>
          <h1 className="text-2xl font-bold text-fg">{title}</h1>
          {subtitle && <p className="text-sm text-fg-muted mt-0.5">{subtitle}</p>}
        </div>
      </div>
      {actions && <div className="flex items-center gap-3">{actions}</div>}
    </div>
  );
}
