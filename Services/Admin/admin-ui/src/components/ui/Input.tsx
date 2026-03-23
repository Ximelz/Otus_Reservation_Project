import type { InputHTMLAttributes } from 'react';
import type { LucideIcon } from 'lucide-react';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  icon?: LucideIcon;
}

export function Input({ label, error, icon: Icon, className = '', ...props }: InputProps) {
  return (
    <div className="space-y-1.5">
      {label && <label className="block text-xs font-medium text-fg-muted">{label}</label>}
      <div className="relative">
        {Icon && <Icon size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-fg-muted" />}
        <input
          className={`w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring transition-colors ${Icon ? 'pl-9' : ''} ${error ? 'border-red' : ''} ${className}`}
          {...props}
        />
      </div>
      {error && <p className="text-xs text-red">{error}</p>}
    </div>
  );
}
