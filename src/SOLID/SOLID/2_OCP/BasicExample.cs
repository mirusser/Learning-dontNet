//Open/Closed Principle
//A class should be open for extension but closed for modification.

namespace SOLID._2_OCP;

public abstract class Shape
{
    // This class is open for extension (we can add new shapes)
    // but closed for modification (we don't need to modify the
    // existing code to add new shapes).

    public abstract double Area();
}

public class Circle : Shape
{
    private readonly double radius;

    public Circle(double radius)
    {
        this.radius = radius;
    }

    public override double Area()
    {
        return Math.PI * radius * radius;
    }
}

public class Square : Shape
{
    private readonly double side;

    public Square(double side)
    {
        this.side = side;
    }

    public override double Area()
    {
        return side * side;
    }
}