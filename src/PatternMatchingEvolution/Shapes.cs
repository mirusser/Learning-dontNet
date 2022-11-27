namespace PatternMatchingEvolution;

public abstract class Shape
{
    public abstract double Area { get; }

    public Shape ShapeInShape { get; set; }
}

public class Rectangle : Shape, ISquare
{
    public Rectangle(double height, double width)
    {
        Height = height;
        Width = width;
    }

    public double Height { get; set; }
    public double Width { get; set; }

    public override double Area => Height * Width;
}

public class Circle : Shape
{
    private const double PI = Math.PI;

    public Circle(double diameter)
    {
        Diameter = diameter;
    }

    public double Diameter { get; set; }
    public double Radius => Diameter / 2.0;
    public override double Area => PI * Radius * Radius;
}

public interface ISquare
{
    double Height { get; set; }
    double Width { get; set; }
}