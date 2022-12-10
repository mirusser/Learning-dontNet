//Open/Closed Principle
//A class should be open for extension but closed for modification.

//In general, OCP states that there should be no need to 
//go back to once written and tested code
//with intent to change it

namespace SOLID._2_OCP;

#region e1 v1
public enum Color
{
    Red, Green, Blue
}

public enum Size
{
    Small, Medium, Large, Yuge
}

public class Product
{
    public string Name { get; set; }
    public Color Color { get; set; }
    public Size Size { get; set; }

    public Product(
        string name,
        Color color,
        Size size)
    {
        Name = name;
        Color = color;
        Size = size;
    }
}

public class ProductFilterV1
{
    public IEnumerable<Product> FilterByColor(
        IEnumerable<Product> products,
        Color color)
    {
        foreach (var p in products)
        {
            if (p.Color == color)
            {
                yield return p;
            }
        }
    }
}

public class ProductRepository
{
    public IEnumerable<Product> Get() => new List<Product>
    {
        new Product("Beach ball", Color.Blue, Size.Medium),
        new Product("Shoe wiper", Color.Green, Size.Medium),
        new Product("Fly", Color.Red, Size.Small),
        new Product("Jojo", Color.Green, Size.Small),
        new Product("Pool", Color.Blue, Size.Large),
        new Product("Border wall", Color.Green, Size.Yuge),
        new Product("Tree", Color.Green, Size.Large),
        new Product("Balloon", Color.Red, Size.Medium),
    };
}

public class ProductFilterClientV1 : IClient
{
    public void DoSomething()
    {
        var productFilter = new ProductFilterV1();
        var products = new ProductRepository().Get();

        foreach (var product in productFilter.FilterByColor(products, Color.Green))
        {
            Console.WriteLine($"{product.Name} is green.");
        }
    }
}
#endregion

// New business requirement: filter by size

#region e1 v2

public class ProductFilter_V2
{
    public IEnumerable<Product> FilterByColor(
        IEnumerable<Product> products,
        Color color)
    {
        foreach (var p in products)
        {

            if (p.Color == color)
            {

                yield return p;
            }
        }
    }

    //new kind of filtering is forcing us to
    //modify class responsible for filtering
    public IEnumerable<Product> FilterBySize(
        IEnumerable<Product> products,
        Size size)
    {
        foreach (var p in products)
        {
            if (p.Size == size)
            {
                yield return p;
            }
        }
    }

    //additionally code is almost identical
    //breaking the DRY (Don't Repeat Yourself) rule
}

public class ProductFilterClient_V2 : IClient
{
    public void DoSomething()
    {
        var productFilter = new ProductFilter_V2();
        var products = new ProductRepository().Get();
        var greenProducts = productFilter.FilterByColor(products, Color.Green);
        foreach (var product in productFilter.FilterBySize(greenProducts, Size.Small))
        {
            Console.WriteLine($"{product.Name} is green and small");
        }
    }
}

#endregion

#region e2 v1

public interface ISpecification<T>
{
    bool IsSatisfied(T item);
}

public interface IFilter<T>
{
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}

public class BetterFilter : IFilter<Product>
{
    public IEnumerable<Product> Filter(
        IEnumerable<Product> items,
        ISpecification<Product> spec)
    {
        foreach (var i in items)
        {
            if (spec.IsSatisfied(i))
            {
                yield return i;
            }
        }
    }
}

public class ColorSpecification : ISpecification<Product>
{
    private readonly Color color;

    public ColorSpecification(Color color)
    {
        this.color = color;
    }

    public bool IsSatisfied(Product p)
    {
        return p.Color == color;
    }
}

public class SizeSpecification : ISpecification<Product>
{
    private readonly Size size;

    public SizeSpecification(Size size)
    {
        this.size = size;
    }

    public bool IsSatisfied(Product item)
    {
        return item.Size == size;
    }
}

public class BetterFilterClient_V1 : IClient
{
    public void DoSomething()
    {
        var products = new ProductRepository().Get();
        var productFilter = new BetterFilter();

        var colorSpec = new ColorSpecification(Color.Green);
        var sizeSpec = new SizeSpecification(Size.Small);

        Console.WriteLine("Small and green products:");

        var filteredProducts = productFilter
            .Filter(
                productFilter.Filter(products, colorSpec),
                sizeSpec);

        foreach (var product in filteredProducts)
        {
            Console.WriteLine(product.Name);
        }
    }
}

#endregion p2 v1

// Joining specifications

#region e2 v2

public abstract class Specification<T> : ISpecification<T>
{
    public abstract bool IsSatisfied(T p);

    public static Specification<T> operator &(
        Specification<T> first,
        ISpecification<T> second)
    {
        return new AndSpecification<T>(first, second);
    }
}

public class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> first, second;

    public AndSpecification(Specification<T> first, ISpecification<T> second)
    {
        this.first = first;
        this.second = second;
    }

    public override bool IsSatisfied(T t)
    {
        return first.IsSatisfied(t) && second.IsSatisfied(t);
    }
}

public class NewColorSpecification : Specification<Product>
{
    private readonly Color color;

    public NewColorSpecification(Color color)
    {
        this.color = color;
    }

    public override bool IsSatisfied(Product p)
    {
        return p.Color == color;
    }
}

public class NewSizeSpecification : Specification<Product>
{
    private readonly Size size;

    public NewSizeSpecification(Size size)
    {
        this.size = size;
    }

    public override bool IsSatisfied(Product item)
    {
        return item.Size == size;
    }
}

public class BetterFilterClient_V2 : IClient
{
    public void DoSomething()
    {
        var smallAndGreenSpec = new NewColorSpecification(Color.Green) & new NewSizeSpecification(Size.Small);
        var products = new ProductRepository().Get();

        var productFilter = new BetterFilter();
        var filteredProducts = productFilter
            .Filter(products, smallAndGreenSpec);

        Console.WriteLine("Green and small products");
        foreach (var product in filteredProducts)
        {
            Console.WriteLine(product.Name);
        }
    }
}

#endregion p2 v2
