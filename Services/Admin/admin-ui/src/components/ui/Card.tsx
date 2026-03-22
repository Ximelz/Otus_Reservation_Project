import type { ReactNode } from 'react';

interface CardProps { children: ReactNode; className?: string; hover?: boolean; }
export function Card({ children, className = '', hover }: CardProps) {
  return (
    <div className={`bg-bg-card border border-border rounded-xl p-5 ${hover ? 'transition-all duration-200 hover:scale-[1.01] hover:shadow-lg hover:shadow-black/20 cursor-pointer' : ''} ${className}`}>
      {children}
    </div>
  );
}

interface SectionProps { children: ReactNode; className?: string; }
export function CardHeader({ children, className = '' }: SectionProps) {
  return <div className={`flex items-center justify-between mb-4 ${className}`}>{children}</div>;
}
export function CardTitle({ children, className = '' }: SectionProps) {
  return <h3 className={`text-sm font-semibold text-fg uppercase tracking-wider ${className}`}>{children}</h3>;
}
export function CardContent({ children, className = '' }: SectionProps) {
  return <div className={`space-y-3 ${className}`}>{children}</div>;
}
