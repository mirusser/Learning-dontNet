using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flyweight;

// The Flyweight Factory creates and manages the Flyweight objects. It
// ensures that flyweights are shared correctly. When the client requests a
// flyweight, the factory either returns an existing instance or creates a
// new one, if it doesn't exist yet.
public class FlyweightFactory
{
    private readonly List<(Flyweight flyweight, string key)> flyweights = new();

    public FlyweightFactory(params Car[] args)
    {
        foreach (var elem in args)
        {
            Flyweight flyweight = new (elem);
            string key = GetKey(elem);
            (Flyweight, string) foo = new (flyweight, key);

            flyweights.Add(foo);
        }
    }

    // Returns a Flyweight's string hash for a given state.
    public static string GetKey(Car car)
    {
        List<string> elements = new()
        {
            car.Model,
            car.Color,
            car.Company
        };

        if (car.Owner != null && car.Number != null)
        {
            elements.Add(car.Number);
            elements.Add(car.Owner);
        }

        elements.Sort();

        return string.Join("_", elements);
    }

    // Returns an existing Flyweight with a given state or creates a new
    // one.
    public Flyweight GetFlyweight(Car sharedState)
    {
        string key = GetKey(sharedState);

        if (!flyweights.Where(t => t.key == key).Any())
        {
            Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
            this.flyweights.Add(new (new(sharedState), key));
        }
        else
        {
            Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
        }

        return this.flyweights
            .Where(t => t.key == key)
            .FirstOrDefault()
            .flyweight;
    }

    public void ListFlyweights()
    {
        var count = flyweights.Count;
        Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
        foreach (var flyweight in flyweights)
        {
            Console.WriteLine(flyweight.key);
        }
    }
}
