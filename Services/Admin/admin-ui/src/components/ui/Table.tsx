import type { ReactNode, ThHTMLAttributes, TdHTMLAttributes } from 'react';

export function Table({ children, className = '' }: { children: ReactNode; className?: string }) {
  return (
    <div className={`rounded-xl overflow-hidden border border-border ${className}`}>
      <table className="w-full text-sm">{children}</table>
    </div>
  );
}

export function TableHeader({ children }: { children: ReactNode }) {
  return <thead className="bg-bg-card">{children}</thead>;
}

export function TableBody({ children }: { children: ReactNode }) {
  return <tbody className="divide-y divide-border">{children}</tbody>;
}

export function TableRow({ children, className = '' }: { children: ReactNode; className?: string }) {
  return <tr className={`hover:bg-white/[0.02] transition-colors ${className}`}>{children}</tr>;
}

export function TableHead({ children, className = '', ...props }: { children?: ReactNode; className?: string } & ThHTMLAttributes<HTMLTableCellElement>) {
  return <th className={`text-left px-4 py-3 text-xs font-medium uppercase tracking-wider text-fg-muted ${className}`} {...props}>{children}</th>;
}

export function TableCell({ children, className = '', ...props }: { children?: ReactNode; className?: string } & TdHTMLAttributes<HTMLTableCellElement>) {
  return <td className={`px-4 py-3 text-fg ${className}`} {...props}>{children}</td>;
}
