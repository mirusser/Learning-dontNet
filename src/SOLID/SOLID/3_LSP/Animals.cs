//Liskov Substitution Principle
//Derived classes should be substitutable for their base classes.

namespace SOLID._3_LSP;

// If interface accepts object of type Parent it should also
// accepts object of type Child without any 'harm' to its functionality

#region r1
public interface IPedigreeCat { }

public abstract class Animal { }

public abstract class Cat : Animal { }

public class BengalCat : Cat, IPedigreeCat { }

public class ChampionCatCaretaker
{
    public virtual void Feed(IPedigreeCat cat)
    {
        Console.WriteLine(@"Fill the cat's bowl with 
            the content of sachet labeled 'Top Beef'");
    }
}

//proper
public class ShelterCatCaretaker : ChampionCatCaretaker
{
    public override void Feed(IPedigreeCat cat)
    {
        Feed(cat);
    }

    public void Feed(Cat cat)
    {
        Console.WriteLine(@"Fill the cat's bowl with 
            the content of sachet labeled 'Universal cat's feed'");
    }
}

//wrong
public class BengalCatCaretaker : ChampionCatCaretaker
{
    public override void Feed(IPedigreeCat cat)
    {
        Console.WriteLine(@"Fill the cat's bowl with 
            the content of sachet labeled 'Only Bengal cat's feed'");
    }
}
#endregion

#region r2

//the return type of the subclass method should match (or be) with the return type of base class

public class CatSeller
{
    public class BritishCat : Cat
    {

    }

    public class CrossbreedCat : Cat
    {

    }

    public abstract class Dog : Animal
    {

    }

    public class GermanShepherd : Dog
    {

    }

    public virtual Cat Sell()
    {
        var cats = new Cat[]
        {
            new BengalCat(),
            new BritishCat(),
            new CrossbreedCat()
        };
        Random rand = new(Guid.NewGuid().GetHashCode());
        return cats[rand.Next(0, 2)];
    }
}

// Proper
public class BengalCatSeller : CatSeller
{
    public override Cat Sell()
    {
        return new BengalCat();
    }
}

// Wrong
public class AnimalSeller : CatSeller
{
    public Animal Sell()
    {
        var pets = new Animal[4]
        {
            new BengalCat(),
            new BritishCat(),
            new CrossbreedCat(),
            new GermanShepherd(),
        };

        Random rand = new(Guid.NewGuid().GetHashCode());
        return pets[rand.Next(0, 3)];
    }
}

#endregion