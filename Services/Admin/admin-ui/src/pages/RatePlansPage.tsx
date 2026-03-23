import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import { ratePlansApi, hotelsApi } from '../api/client';
import { CheckCircle2, X, DollarSign } from 'lucide-react';
import { CancellationLabels } from '../types';
import { t } from '../lib/i18n';
import { formatPrice } from '../lib/format';
import {
  Table, TableHeader, TableBody, TableRow, TableHead, TableCell,
  PageHeader, Badge, EmptyState, Skeleton,
} from '../components/ui';

function cancellationVariant(policyType: number): 'success' | 'warning' | 'critical' {
  const map: Record<number, 'success' | 'warning' | 'critical'> = {
    0: 'success',
    1: 'warning',
    2: 'critical',
    3: 'critical',
  };
  return map[policyType] ?? 'critical';
}

export function RatePlansPage() {
  const { hotelId } = useParams<{ hotelId: string }>();
  const { data: hotel } = useQuery({ queryKey: ['hotel', hotelId], queryFn: () => hotelsApi.get(hotelId!) });
  const { data: plans, isLoading } = useQuery({ queryKey: ['ratePlans', hotelId], queryFn: () => ratePlansApi.list(hotelId!) });

  return (
    <div className="space-y-6">
      <PageHeader
        title={t.ratePlans.title}
        subtitle={`${hotel?.name ?? ''} · ${plans?.length ?? 0} тарифов`}
        backTo={`/hotels/${hotelId}`}
      />

      {isLoading ? (
        <div className="space-y-2">
          {[...Array(5)].map((_, i) => (
            <Skeleton key={i} className="h-14" />
          ))}
        </div>
      ) : !plans?.length ? (
        <EmptyState icon={DollarSign} title={t.ratePlans.noPlans} description={t.ratePlans.noPlansDesc} />
      ) : (
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>{t.ratePlans.code}</TableHead>
              <TableHead>{t.ratePlans.name}</TableHead>
              <TableHead>{t.ratePlans.roomType}</TableHead>
              <TableHead>{t.ratePlans.price}</TableHead>
              <TableHead>{t.ratePlans.cancellation}</TableHead>
              <TableHead>{t.ratePlans.breakfast}</TableHead>
              <TableHead>{t.ratePlans.prepayment}</TableHead>
              <TableHead>{t.ratePlans.default}</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {plans.map(plan => (
              <TableRow key={plan.id}>
                <TableCell className="text-gold font-mono text-xs">{plan.code}</TableCell>
                <TableCell className="font-medium">{plan.name}</TableCell>
                <TableCell className="text-fg-muted">{plan.roomTypeName || '\u2014'}</TableCell>
                <TableCell className="font-medium">{formatPrice(plan.basePrice, plan.currency)}</TableCell>
                <TableCell>
                  <Badge variant={cancellationVariant(plan.cancellationPolicyType)}>
                    {CancellationLabels[plan.cancellationPolicyType]}
                  </Badge>
                </TableCell>
                <TableCell>
                  {plan.breakfastIncluded
                    ? <CheckCircle2 size={16} className="text-emerald" />
                    : <X size={16} className="text-fg-muted" />}
                </TableCell>
                <TableCell>
                  {plan.prepaymentRequired
                    ? <CheckCircle2 size={16} className="text-emerald" />
                    : <X size={16} className="text-fg-muted" />}
                </TableCell>
                <TableCell>
                  {plan.isDefault ? <Badge variant="gold">{t.ratePlans.default}</Badge> : null}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}
    </div>
  );
}
