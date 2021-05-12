﻿using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.DataAccess.Repositories
{
    public class DeskBookingRepository : IDeskBookingRepository
    {
        private readonly DeskBookerContext _context;

        public DeskBookingRepository(DeskBookerContext context)
        {
            _context = context;
        }

        public IEnumerable<DeskBooking> GetAll()
        {
            return 
                _context
                .DeskBookings
                .OrderBy(d => d.Date)
                .ToList();
        }

        public void Save(DeskBooking deskBooking)
        {
            _context.DeskBookings.Add(deskBooking);
            _context.SaveChanges();
        }
    }
}
