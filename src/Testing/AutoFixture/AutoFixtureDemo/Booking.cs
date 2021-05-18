using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo
{
    public class Booking
    {
        public string BookingReference { get; set; }
        public string CustomerName { get; set; }
        public List<FlightDetails2> Legs { get; set; } = new List<FlightDetails2>();
    }
}
