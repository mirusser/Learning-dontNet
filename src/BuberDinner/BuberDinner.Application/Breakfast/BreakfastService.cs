namespace BuberDinner.Application.Breakfast;

public interface IBreakfastService
{
    void CreateBreakfast(Domain.Entities.Breakfast breakfast);

    Domain.Entities.Breakfast? GetBreakfast(Guid id);
}

public class BreakfastService : IBreakfastService
{
    public void CreateBreakfast(Domain.Entities.Breakfast breakfast)
    {
        throw new NotImplementedException();
    }

    public Domain.Entities.Breakfast? GetBreakfast(Guid id)
    {
        throw new NotImplementedException();
    }
}