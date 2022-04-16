using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;

namespace BilliardClub.Data.DBModels.Repository
{
    public class StatusRepository
    {
        private readonly Context _context;

        public StatusRepository(Context context)
        {
            _context = context;
        }

        public Status GetStatusInCart()
        {
            return _context.Status.First(x => x.name == "В корзине");
        }

        public Status GetStatusReserved()
        {
            return _context.Status.First(x => x.name == "Забронирован");
        }
    }
}
