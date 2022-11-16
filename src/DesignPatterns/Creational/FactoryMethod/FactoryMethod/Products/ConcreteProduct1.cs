namespace FactoryMethod;

// Concrete Products provide various implementations of the Product
// interface.
internal class ConcreteProduct1 : IProduct
{
    public string Operation()
    {
        return "{Result of ConcreteProduct1}";
    }
}