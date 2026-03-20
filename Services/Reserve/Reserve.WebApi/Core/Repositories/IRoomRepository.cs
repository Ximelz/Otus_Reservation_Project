namespace ReservService
{
    public interface IRoomRepository
    {
        public Task AddRoom(RoomReserve room, CancellationToken ct);
        public Task RemoveRoom(RoomReserve room, CancellationToken ct);
        public Task<RoomReserve?> GetRoom(Guid id, CancellationToken ct);
        public Task<IReadOnlyList<RoomReserve>> GetRooms(Func<RoomReserve, bool> predicate, CancellationToken ct);
    }
}
