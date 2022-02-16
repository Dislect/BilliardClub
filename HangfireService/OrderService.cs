using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace BilliardClub.HangfireService
{
    public class OrderService
    {
        private readonly Context _context;

        public OrderService(Context context)
        {
            _context = context;
        }

        public async Task ReservationForSelectedDateJob(int tableId)
        {
            var table = _context.PoolTables
                            .Include(x => x.statusTables)
                            .ThenInclude(x => x.status)
                            .FirstOrDefault(x => x.id == tableId) 
                        ?? throw new Exception("Не найден стол для бронирования");

            var statusReserve = _context.Status.FirstOrDefault(x => x.name == "Забронирован")
                                ?? throw new Exception("Не найден статус в БД");

            if (table.statusTables.Any())
            {
                table.statusTables.Last().dateEnd = DateTime.Now;
            }

            table.statusTables.Add(new StatusTable()
            {
                dateStart = DateTime.Now,
                status = statusReserve
            });

            await _context.SaveChangesAsync();
        }

        public async Task ReleaseOnSelectedDateJob(int tableId)
        {
            var table = _context.PoolTables
                            .Include(x => x.statusTables)
                            .ThenInclude(x => x.status)
                            .FirstOrDefault(x => x.id == tableId)
                        ?? throw new Exception("Не найден стол для бронирования");

            var statusFree = _context.Status.First(x => x.name == "Свободен")
                             ?? throw new Exception("Не найден статус в БД");

            if (table.statusTables.Any())
            {
                table.statusTables.Last().dateEnd = DateTime.Now;
            }
            table.statusTables.Add(new StatusTable() { dateStart = DateTime.Now, status = statusFree });

            await _context.SaveChangesAsync();
        }
    }
}
