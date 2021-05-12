using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.DataAccess.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        private readonly DeskBookerContext _context;

        public DeskRepository(DeskBookerContext context)
        {
            _context = context;
        }

        public IEnumerable<Desk> GetAll()
        {
            return _context.Desk.ToList();
        }

        public IEnumerable<Desk> GetAvailableDesks(DateTime date)
        {
            var bookedDeskIds =
                _context
                .DeskBookings
                .Where(d => d.Date == date)
                .Select(b => b.DeskId)
                .ToList();

            return
                _context
                .Desk
                .Where(d => !bookedDeskIds.Contains(d.Id))
                .ToList();
        }
    }
}
