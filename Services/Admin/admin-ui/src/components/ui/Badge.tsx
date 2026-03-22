import type { ReactNode } from 'react';

const badgeVariants = {
  success: 'bg-emerald-muted text-emerald',
  warning: 'bg-amber-muted text-amber',
  critical: 'bg-red-muted text-red',
  info: 'bg-blue-muted text-blue',
  default: 'bg-bg-muted text-fg-muted',
  gold: 'bg-gold/10 text-gold',
} as const;

interface BadgeProps { variant?: keyof typeof badgeVariants; children: ReactNode; className?: string; }

export function Badge({ variant = 'default', children, className = '' }: BadgeProps) {
  return (
    <span className={`inline-flex items-center text-xs px-2.5 py-0.5 rounded-full font-medium ${badgeVariants[variant]} ${className}`}>
      {children}
    </span>
  );
}
