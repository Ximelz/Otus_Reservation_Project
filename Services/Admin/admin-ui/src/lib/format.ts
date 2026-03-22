export function formatDate(iso: string): string {
  return new Date(iso).toLocaleDateString('ru-RU', {
    day: '2-digit', month: '2-digit', year: 'numeric',
  });
}

export function formatDateTime(iso: string): string {
  return new Date(iso).toLocaleString('ru-RU', {
    day: '2-digit', month: '2-digit', year: 'numeric',
    hour: '2-digit', minute: '2-digit',
  });
}

export function formatPrice(amount: number, currency: string = 'RUB'): string {
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency', currency, minimumFractionDigits: 0, maximumFractionDigits: 0,
  }).format(amount);
}

export function formatNumber(n: number): string {
  return new Intl.NumberFormat('ru-RU').format(n);
}
