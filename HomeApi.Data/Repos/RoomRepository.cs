using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        /// <summary>
        ///  Найти комнату по id
        /// </summary>
        public async Task <Room> GetRoomById(Guid id)
        {
            return await _context.Rooms.Where(r=>r.Id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }
        /// <summary>
        ///  Удалить комнату
        /// </summary>
        public async Task DeleteRoom(Room room)
        {
            var entry= _context.Entry(room);
                _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        ///  Обновить данные комнаты
        /// </summary>
        public async Task UpdateRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            // Сохраняем изменения в базе 
            await _context.SaveChangesAsync();
        }

    }
}