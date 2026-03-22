import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import { roomsApi, hotelsApi, roomTypesApi } from '../api/client';
import { BedDouble, Plus, LayoutGrid } from 'lucide-react';
import { RoomStatus, HousekeepingStatus, RoomStatusLabels, HousekeepingLabels } from '../types';
import type { Room } from '../types';
import { t } from '../lib/i18n';
import {
  Table, TableHeader, TableBody, TableRow, TableHead, TableCell,
  PageHeader, Badge, EmptyState, Skeleton,
  Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter,
  Button, Input,
} from '../components/ui';

function roomStatusVariant(s: RoomStatus): 'success' | 'warning' | 'info' | 'critical' | 'default' {
  const map = {
    [RoomStatus.Available]: 'success',
    [RoomStatus.Occupied]: 'warning',
    [RoomStatus.Reserved]: 'info',
    [RoomStatus.OutOfService]: 'critical',
    [RoomStatus.Maintenance]: 'default',
  } as const;
  return map[s] || 'default';
}

function hkStatusVariant(s: HousekeepingStatus): 'success' | 'critical' | 'info' {
  const map = {
    [HousekeepingStatus.Clean]: 'success',
    [HousekeepingStatus.Dirty]: 'critical',
    [HousekeepingStatus.Inspected]: 'info',
  } as const;
  return map[s] || 'info';
}

export function RoomsPage() {
  const { hotelId } = useParams<{ hotelId: string }>();
  const qc = useQueryClient();
  const { data: hotel } = useQuery({ queryKey: ['hotel', hotelId], queryFn: () => hotelsApi.get(hotelId!) });
  const { data: rooms, isLoading } = useQuery({ queryKey: ['rooms', hotelId], queryFn: () => roomsApi.list(hotelId!) });

  const [editingRoom, setEditingRoom] = useState<Room | null>(null);
  const [editForm, setEditForm] = useState({ number: '', floor: 0, typeId: '', viewType: '', notes: '' });
  const [editError, setEditError] = useState('');

  const { data: roomTypes } = useQuery({
    queryKey: ['roomTypes', hotelId],
    queryFn: () => roomTypesApi.list(hotelId!),
    enabled: !!hotelId,
  });

  const [createOpen, setCreateOpen] = useState(false);
  const [createForm, setCreateForm] = useState({ number: '', floor: 1, typeId: '', viewType: '', notes: '' });
  const [createError, setCreateError] = useState('');

  const [typeOpen, setTypeOpen] = useState(false);
  const [typeForm, setTypeForm] = useState({ code: '', name: '', description: '', capacity: 2, capacityAdults: 2, capacityChildren: 0, bedConfiguration: '1 Double', baseAreaSqm: 22 });
  const [typeError, setTypeError] = useState('');

  const createTypeMut = useMutation({
    mutationFn: (data: typeof typeForm) => roomTypesApi.create(hotelId!, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['roomTypes', hotelId] });
      setTypeOpen(false);
      setTypeForm({ code: '', name: '', description: '', capacity: 2, capacityAdults: 2, capacityChildren: 0, bedConfiguration: '1 Double', baseAreaSqm: 22 });
      setTypeError('');
    },
    onError: (err: unknown) => {
      setTypeError(err instanceof Error ? err.message : 'Ошибка при создании типа');
    },
  });

  const createMut = useMutation({
    mutationFn: (data: { number: string; floor: number; typeId: string; viewType: string; notes: string }) =>
      roomsApi.create(hotelId!, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['rooms', hotelId] });
      setCreateOpen(false);
      setCreateForm({ number: '', floor: 1, typeId: '', viewType: '', notes: '' });
      setCreateError('');
    },
    onError: (err: unknown) => {
      setCreateError(err instanceof Error ? err.message : 'Ошибка при создании');
    },
  });

  const statusMut = useMutation({
    mutationFn: ({ id, status }: { id: string; status: number }) => roomsApi.changeStatus(id, status),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['rooms', hotelId] }),
  });

  const hkMut = useMutation({
    mutationFn: ({ id, hk }: { id: string; hk: number }) => roomsApi.changeHousekeeping(id, hk),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['rooms', hotelId] }),
  });

  const updateMut = useMutation({
    mutationFn: (data: { number: string; floor: number; typeId: string; viewType: string; notes: string }) =>
      roomsApi.update(editingRoom!.id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['rooms', hotelId] });
      setEditingRoom(null);
      setEditError('');
    },
    onError: (err: unknown) => {
      const msg = err instanceof Error ? err.message : 'Ошибка при сохранении';
      setEditError(msg);
    },
  });

  function openEditDialog(room: Room) {
    setEditForm({
      number: room.number,
      floor: room.floor,
      typeId: room.typeId,
      viewType: room.viewType ?? '',
      notes: room.notes ?? '',
    });
    setEditError('');
    setEditingRoom(room);
  }

  function handleEditSave() {
    updateMut.mutate({
      number: editForm.number,
      floor: editForm.floor,
      typeId: editForm.typeId,
      viewType: editForm.viewType,
      notes: editForm.notes,
    });
  }

  return (
    <div className="space-y-6">
      <PageHeader
        title={t.rooms.title}
        subtitle={`${hotel?.name ?? ''} · ${rooms?.length ?? 0} номеров`}
        backTo={`/hotels/${hotelId}`}
        actions={<Button icon={Plus} onClick={() => setCreateOpen(true)}>{t.rooms.addRoom}</Button>}
      />

      {isLoading ? (
        <div className="space-y-2">
          {[...Array(5)].map((_, i) => (
            <Skeleton key={i} className="h-14" />
          ))}
        </div>
      ) : !rooms?.length ? (
        <EmptyState icon={BedDouble} title={t.rooms.noRooms} description={t.rooms.noRoomsDesc} />
      ) : (
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>{t.rooms.room}</TableHead>
              <TableHead>{t.rooms.floor}</TableHead>
              <TableHead>{t.rooms.type}</TableHead>
              <TableHead>{t.rooms.status}</TableHead>
              <TableHead>{t.rooms.housekeeping}</TableHead>
              <TableHead>{t.rooms.view}</TableHead>
              <TableHead>{t.rooms.actions}</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {rooms.map(room => (
              <TableRow key={room.id}>
                <TableCell className="font-medium">{room.number}</TableCell>
                <TableCell className="text-fg-muted">{room.floor}</TableCell>
                <TableCell className="text-fg-muted">{room.roomTypeName}</TableCell>
                <TableCell>
                  <select
                    value={room.status}
                    onChange={e => statusMut.mutate({ id: room.id, status: +e.target.value })}
                    className="appearance-none bg-transparent outline-none cursor-pointer"
                  >
                    {Object.entries(RoomStatusLabels).map(([k, v]) => (
                      <option key={k} value={k}>{v}</option>
                    ))}
                  </select>
                  <Badge variant={roomStatusVariant(room.status)} className="ml-2 pointer-events-none">
                    {RoomStatusLabels[room.status]}
                  </Badge>
                </TableCell>
                <TableCell>
                  <select
                    value={room.housekeepingStatus}
                    onChange={e => hkMut.mutate({ id: room.id, hk: +e.target.value })}
                    className="appearance-none bg-transparent outline-none cursor-pointer"
                  >
                    {Object.entries(HousekeepingLabels).map(([k, v]) => (
                      <option key={k} value={k}>{v}</option>
                    ))}
                  </select>
                  <Badge variant={hkStatusVariant(room.housekeepingStatus)} className="ml-2 pointer-events-none">
                    {HousekeepingLabels[room.housekeepingStatus]}
                  </Badge>
                </TableCell>
                <TableCell className="text-fg-muted text-xs">{room.viewType || '\u2014'}</TableCell>
                <TableCell>
                  <button
                    onClick={() => openEditDialog(room)}
                    className="text-xs px-2 py-1 rounded text-fg-muted hover:bg-white/5 transition-colors"
                  >
                    {t.rooms.edit}
                  </button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}

      <Dialog open={!!editingRoom} onOpenChange={(open) => { if (!open) { setEditingRoom(null); setEditError(''); } }}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>{t.rooms.editRoom}</DialogTitle>
            <DialogDescription>{t.rooms.editRoomDesc}</DialogDescription>
          </DialogHeader>

          {editingRoom && (
            <div className="space-y-4">
              {/* Current status badges (read-only) */}
              <div className="flex items-center gap-2">
                <Badge variant={roomStatusVariant(editingRoom.status)}>
                  {RoomStatusLabels[editingRoom.status]}
                </Badge>
                <Badge variant={hkStatusVariant(editingRoom.housekeepingStatus)}>
                  {HousekeepingLabels[editingRoom.housekeepingStatus]}
                </Badge>
              </div>

              <Input
                label={t.rooms.roomNumber}
                value={editForm.number}
                onChange={e => setEditForm(f => ({ ...f, number: e.target.value }))}
              />

              <Input
                label={t.rooms.floor}
                type="number"
                value={editForm.floor}
                onChange={e => setEditForm(f => ({ ...f, floor: +e.target.value }))}
              />

              <div className="space-y-1.5">
                <label className="block text-xs font-medium text-fg-muted">{t.rooms.type}</label>
                <select
                  value={editForm.typeId}
                  onChange={e => setEditForm(f => ({ ...f, typeId: e.target.value }))}
                  className="w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring"
                >
                  <option value="">{t.rooms.selectType}</option>
                  {roomTypes?.map(rt => (
                    <option key={rt.id} value={rt.id}>{rt.name} ({rt.code})</option>
                  ))}
                </select>
              </div>

              <Input
                label={t.rooms.viewType}
                value={editForm.viewType}
                onChange={e => setEditForm(f => ({ ...f, viewType: e.target.value }))}
                placeholder="Город, Сад, Бассейн..."
              />

              <div className="space-y-1.5">
                <label className="block text-xs font-medium text-fg-muted">{t.rooms.notes}</label>
                <textarea
                  value={editForm.notes}
                  onChange={e => setEditForm(f => ({ ...f, notes: e.target.value }))}
                  rows={3}
                  className="w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring"
                  placeholder={t.rooms.notesPlaceholder}
                />
              </div>

              {editError && (
                <p className="text-sm text-red">{editError}</p>
              )}
            </div>
          )}

          <DialogFooter>
            <Button variant="ghost" onClick={() => { setEditingRoom(null); setEditError(''); }}>
              {t.common.cancel}
            </Button>
            <Button onClick={handleEditSave} disabled={updateMut.isPending}>
              {updateMut.isPending ? t.common.loading : t.common.save}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Create room dialog */}
      <Dialog open={createOpen} onOpenChange={(open) => { if (!open) { setCreateOpen(false); setCreateError(''); } }}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>{t.rooms.addRoom}</DialogTitle>
            <DialogDescription>Заполните данные нового номера</DialogDescription>
          </DialogHeader>

          <div className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <Input
                label={t.rooms.roomNumber}
                value={createForm.number}
                onChange={e => setCreateForm(f => ({ ...f, number: e.target.value }))}
                placeholder="101"
              />
              <Input
                label={t.rooms.floor}
                type="number"
                value={createForm.floor}
                onChange={e => setCreateForm(f => ({ ...f, floor: +e.target.value }))}
              />
            </div>

            <div className="space-y-1.5">
              <div className="flex items-center justify-between">
                <label className="block text-xs font-medium text-fg-muted">{t.rooms.type}</label>
                <button
                  type="button"
                  onClick={() => setTypeOpen(true)}
                  className="text-xs text-gold hover:text-gold/80 transition-colors"
                >
                  + Добавить тип
                </button>
              </div>
              {!roomTypes?.length ? (
                <div className="rounded-lg border border-border bg-bg-muted p-4 text-center">
                  <LayoutGrid size={20} className="mx-auto text-fg-muted mb-2" />
                  <p className="text-sm text-fg-muted mb-2">Сначала создайте тип номера</p>
                  <Button size="sm" onClick={() => setTypeOpen(true)} icon={Plus}>Создать тип номера</Button>
                </div>
              ) : (
                <select
                  value={createForm.typeId}
                  onChange={e => setCreateForm(f => ({ ...f, typeId: e.target.value }))}
                  className="w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring"
                >
                  <option value="">{t.rooms.selectType}</option>
                  {roomTypes.map(rt => (
                    <option key={rt.id} value={rt.id}>{rt.name} ({rt.code})</option>
                  ))}
                </select>
              )}
            </div>

            <Input
              label={t.rooms.viewType}
              value={createForm.viewType}
              onChange={e => setCreateForm(f => ({ ...f, viewType: e.target.value }))}
              placeholder="Город, Сад, Бассейн..."
            />

            <div className="space-y-1.5">
              <label className="block text-xs font-medium text-fg-muted">{t.rooms.notes}</label>
              <textarea
                value={createForm.notes}
                onChange={e => setCreateForm(f => ({ ...f, notes: e.target.value }))}
                rows={2}
                className="w-full px-3 py-2 rounded-lg text-sm bg-bg-input text-fg border border-border outline-none focus:ring-2 focus:ring-ring"
                placeholder={t.rooms.notesPlaceholder}
              />
            </div>

            {createError && <p className="text-sm text-red">{createError}</p>}
          </div>

          <DialogFooter>
            <Button variant="ghost" onClick={() => { setCreateOpen(false); setCreateError(''); }}>
              {t.common.cancel}
            </Button>
            <Button onClick={() => createMut.mutate(createForm)} disabled={createMut.isPending || !createForm.number || !createForm.typeId}>
              {createMut.isPending ? t.common.loading : t.common.create}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Create room type dialog */}
      <Dialog open={typeOpen} onOpenChange={(open) => { if (!open) { setTypeOpen(false); setTypeError(''); } }}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Создать тип номера</DialogTitle>
            <DialogDescription>Укажите параметры нового типа номера</DialogDescription>
          </DialogHeader>

          <div className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <Input
                label="Название"
                value={typeForm.name}
                onChange={e => setTypeForm(f => ({ ...f, name: e.target.value }))}
                placeholder="Standard"
              />
              <Input
                label="Код"
                value={typeForm.code}
                onChange={e => setTypeForm(f => ({ ...f, code: e.target.value.toUpperCase() }))}
                placeholder="STD"
              />
            </div>

            <Input
              label="Описание"
              value={typeForm.description}
              onChange={e => setTypeForm(f => ({ ...f, description: e.target.value }))}
              placeholder="Стандартный номер с видом на город"
            />

            <div className="grid grid-cols-3 gap-4">
              <Input
                label="Взрослых"
                type="number"
                value={typeForm.capacityAdults}
                onChange={e => setTypeForm(f => ({ ...f, capacityAdults: +e.target.value, capacity: +e.target.value + f.capacityChildren }))}
              />
              <Input
                label="Детей"
                type="number"
                value={typeForm.capacityChildren}
                onChange={e => setTypeForm(f => ({ ...f, capacityChildren: +e.target.value, capacity: f.capacityAdults + +e.target.value }))}
              />
              <Input
                label="Площадь (м²)"
                type="number"
                value={typeForm.baseAreaSqm}
                onChange={e => setTypeForm(f => ({ ...f, baseAreaSqm: +e.target.value }))}
              />
            </div>

            <Input
              label="Конфигурация кроватей"
              value={typeForm.bedConfiguration}
              onChange={e => setTypeForm(f => ({ ...f, bedConfiguration: e.target.value }))}
              placeholder="1 Double, 1 King + Sofa..."
            />

            {typeError && <p className="text-sm text-red">{typeError}</p>}
          </div>

          <DialogFooter>
            <Button variant="ghost" onClick={() => { setTypeOpen(false); setTypeError(''); }}>
              {t.common.cancel}
            </Button>
            <Button onClick={() => createTypeMut.mutate(typeForm)} disabled={createTypeMut.isPending || !typeForm.name || !typeForm.code}>
              {createTypeMut.isPending ? t.common.loading : t.common.create}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
