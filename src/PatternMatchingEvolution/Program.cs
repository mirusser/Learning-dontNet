using PatternMatchingEvolution;

var circle = new Circle(5);
var rectangle = new Rectangle(420, 1337);
var square = new Rectangle(69, 69);

var shapes = new List<Shape>()
{
    circle, rectangle, square
};

var randomShape = shapes[new Random().Next(shapes.Count)];

if (randomShape is Circle cir)
{
    Console.WriteLine($"Circle with area: {cir.Diameter}");
}

switch (randomShape)
{
    case Circle c:
        Console.WriteLine($"This is a cricle with area {c.Area}");
        break;

    case Rectangle r when r.Height == r.Width:
        Console.WriteLine($"This is a squere!");
        break;

    default:
        Console.WriteLine("Nothing special about this.");
        break;
}

if (randomShape is Circle { Radius: 10, Diameter: 46 })
{
    //Do some stuff
}

var shapeDetails = randomShape switch
{
    Circle cir2 => $"This is a circle with area {cir2.Area}",
    Rectangle rec2 when rec2.Height == rec2.Width => "This is a square",
    { Area: 100 } => "This area was 100",
    _ => "This is a default it didn't match anything"
};

Console.WriteLine(shapeDetails);

if (randomShape is not Rectangle)
{
    //do stuff
}

if (randomShape is Circle { Radius: > 100 and < 200, Area: >= 1000 })
{
    //do stuff
}

var shapeDetails2 = randomShape switch
{
    Circle { Radius: > 100 and < 200, Area: >= 1000 } => $"This is my magic circle",
    _ => "This is a default it didn't match anything"
};

var areaDetails = randomShape.Area switch
{
    >= 100 and <= 200 => "bla bla",
    _ => "This is a default it didn't match anything"
};

//since c# 10
if (randomShape is Rectangle { ShapeInShape.Area: 100 })
{
    //do stuff
}

//some other examples
static decimal GetGroupTicketPriceDiscount(int groupSize, DateTime visitDate)
    => (groupSize, visitDate.DayOfWeek) switch
    {
        ( <= 0, _) => throw new ArgumentException("Group size must be positive."),
        (_, DayOfWeek.Saturday or DayOfWeek.Sunday) => 0.0m,
        ( >= 5 and < 10, DayOfWeek.Monday) => 20.0m,
        ( >= 10, DayOfWeek.Monday) => 30.0m,
        ( >= 5 and < 10, _) => 12.0m,
        ( >= 10, _) => 15.0m,
        _ => 0m,
    };

public static class Extensions
{
    public static bool IsLetter(this char c) =>
        c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
}