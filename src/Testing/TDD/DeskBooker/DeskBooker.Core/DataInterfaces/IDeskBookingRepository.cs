using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterfaces
{
    public interface IDeskBookingRepository
    {
        void Save(DeskBooking deskBooking);
    }
}
