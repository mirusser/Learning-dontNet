namespace Builder;

// The Director is only responsible for executing the building steps in a
// particular sequence. It is helpful when producing products according to a
// specific order or configuration. Strictly speaking, the Director class is
// optional, since the client can control builders directly.
public class Director
{
    private IBuilder builder;

    public IBuilder Builder
    {
        set { builder = value; }
    }

    // The Director can construct several product variations using the same
    // building steps.
    public void BuildMinimalViableProduct()
    {
        this.builder.BuildPartA();
    }

    public void BuildFullFeaturedProduct()
    {
        this.builder.BuildPartA();
        this.builder.BuildPartB();
        this.builder.BuildPartC();
    }
}