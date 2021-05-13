using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeskBooker.Web.Pages
{
    public class DeskBookingsModel : PageModel
    {
        private readonly IDeskBookingRepository _deskBookingRepository;

        public IEnumerable<DeskBooking> DeskBookings { get; set; }

        public DeskBookingsModel(IDeskBookingRepository deskBookingRepository)
        {
            _deskBookingRepository = deskBookingRepository;
        }

        public void OnGet()
        {
            DeskBookings = _deskBookingRepository.GetAll();
        }
    }
}
