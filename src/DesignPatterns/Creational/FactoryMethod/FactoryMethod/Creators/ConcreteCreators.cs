namespace FactoryMethod;

//Concrete Creators override the factory method in order to chagne the
//resulting product's type.
internal class ConcreteCreators : Creator
{
    //Note that the signature of the method still uses the abstract product
    //type, even though the concrete product is actually returned from the 
    //method. This way the Creator can stay independent of concrete product classes.
    public override IProduct FactoryMethod()
    {
        return new ConcreteProduct1();
    }
}

class ConcreteCreator2 : Creator
{
    public override IProduct FactoryMethod()
    {
        return new ConcreteProduct2();
    }
}