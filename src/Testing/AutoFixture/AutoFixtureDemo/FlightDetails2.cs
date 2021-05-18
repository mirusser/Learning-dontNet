using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo
{
    public class FlightDetails2
    {
        public FlightDetails2(AirportCode departureAirportCode, AirportCode arrivalAirportCode)
        {
            DepartureAirportCode = departureAirportCode;
            ArrivalAirportCode = arrivalAirportCode;
        }

        public AirportCode DepartureAirportCode { get; }
        public AirportCode ArrivalAirportCode { get; }
        public TimeSpan FlightDuration { get; set; }
        public string AirlineName { get; set; }
    }
}
