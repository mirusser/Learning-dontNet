namespace BuberDinner.Domain.Entities;

public class Breakfast
{
    //public Breakfast(
    //    Guid id,
    //    string name,
    //    string description,
    //    DateTime startDateTime,
    //    DateTime endDateTime,
    //    List<string> savory,
    //    List<string> sweet)
    //{
    //    Id = id;
    //    Name = name;
    //    Description = description;
    //    StartDateTime = startDateTime;
    //    EndDateTime = endDateTime;
    //    Savory = savory;
    //    Sweet = sweet;
    //}

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public List<string> Savory { get; set; } = null!;
    public List<string> Sweet { get; set; } = null!;
    //public Guid Guid { get; }
    //public DateTime UtcNow { get; }
}