using AbstractFactory.Products;

namespace AbstractFactory.Factories;

// Each Concrete Factory has a corresponding product variant.
internal class ConcreteFactory2 : IAbstractFactory
{
    public IAbstractProductA CreateProductA()
    {
        return new ConcreteProductA2();
    }

    public IAbstractProductB CreateProductB()
    {
        return new ConcreteProductB2();
    }
}