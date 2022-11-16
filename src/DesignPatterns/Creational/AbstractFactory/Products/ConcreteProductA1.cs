namespace AbstractFactory.Products;

// Concrete Products are created by corresponding Concrete Factories.
internal class ConcreteProductA1 : IAbstractProductA
{
    public string UsefulFunctionA()
    {
        return "The result of the product A1.";
    }
}