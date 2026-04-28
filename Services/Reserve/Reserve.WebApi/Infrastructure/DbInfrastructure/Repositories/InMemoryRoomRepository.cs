namespace ReservService
{
    public class InMemoryRoomRepository : IRoomRepository
    {
        private readonly List<RoomReserve> rooms = new List<RoomReserve>();
        public Task AddRoom(RoomReserve room, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            rooms.Add(room);

            return Task.CompletedTask;
        }

        public Task<RoomReserve?> GetRoom(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var room = rooms.FirstOrDefault(r => r.Id == id);

            return Task.FromResult(room);
        }

        public Task<IReadOnlyList<RoomReserve>> GetRooms(Func<RoomReserve, bool> predicate, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            IReadOnlyList<RoomReserve> findRooms = rooms.Where(predicate).ToList();

            return Task.FromResult(findRooms);
        }

        public Task RemoveRoom(RoomReserve room, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (!rooms.Where(r => r.Id == room.Id).Any())
                throw new ArgumentException("Комнаты нет в репозитории!");

            rooms.Remove(room);

            return Task.CompletedTask;
        }
    }
}
