export function Skeleton({ className = '' }: { className?: string }) {
  return <div className={`animate-pulse bg-bg-muted rounded-lg ${className}`} />;
}

export function SkeletonCard() {
  return (
    <div className="bg-bg-card border border-border rounded-xl p-5 space-y-4">
      <Skeleton className="h-4 w-1/3" />
      <Skeleton className="h-8 w-1/2" />
      <Skeleton className="h-3 w-2/3" />
    </div>
  );
}
