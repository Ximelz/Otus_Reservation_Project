import { createContext, useContext, useState, useCallback, type ReactNode } from 'react';
import * as ToastPrimitive from '@radix-ui/react-toast';
import { X, CheckCircle2, AlertCircle, Info } from 'lucide-react';

interface ToastData { id: number; title: string; variant?: 'success' | 'error' | 'info'; }

const ToastContext = createContext<{ toast: (title: string, variant?: ToastData['variant']) => void }>({ toast: () => {} });

export function useToast() { return useContext(ToastContext); }

let toastId = 0;

export function ToastProvider({ children }: { children: ReactNode }) {
  const [toasts, setToasts] = useState<ToastData[]>([]);

  const toast = useCallback((title: string, variant: ToastData['variant'] = 'success') => {
    const id = ++toastId;
    setToasts(prev => [...prev, { id, title, variant }]);
    setTimeout(() => setToasts(prev => prev.filter(t => t.id !== id)), 4000);
  }, []);

  const icons = { success: CheckCircle2, error: AlertCircle, info: Info };
  const colors = { success: 'text-emerald', error: 'text-red', info: 'text-blue' };

  return (
    <ToastContext.Provider value={{ toast }}>
      <ToastPrimitive.Provider swipeDirection="right">
        {children}
        {toasts.map(t => {
          const Icon = icons[t.variant || 'success'];
          return (
            <ToastPrimitive.Root key={t.id} className="bg-bg-card border border-border rounded-xl p-4 shadow-2xl shadow-black/40 flex items-center gap-3 data-[state=open]:animate-in data-[state=open]:slide-in-from-right data-[state=closed]:animate-out data-[state=closed]:fade-out-0">
              <Icon size={18} className={colors[t.variant || 'success']} />
              <ToastPrimitive.Title className="text-sm text-fg flex-1">{t.title}</ToastPrimitive.Title>
              <ToastPrimitive.Close className="text-fg-muted hover:text-fg"><X size={14} /></ToastPrimitive.Close>
            </ToastPrimitive.Root>
          );
        })}
        <ToastPrimitive.Viewport className="fixed bottom-4 right-4 z-[100] flex flex-col gap-2 w-96" />
      </ToastPrimitive.Provider>
    </ToastContext.Provider>
  );
}
