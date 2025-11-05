namespace RefKeywordDemo;

public static class CrudeExamples
{
    public static void Run()
    {
        ClassExample();
        ClassExample2();
        ClassExample3();
        ClassExample4();
        ClassExample5();

        Console.WriteLine();

        StructExample();
        StructExample2();

        Console.WriteLine();

        StructRefExample();
        StructRefExample2();

        Console.WriteLine();

        RefStructExample();

        Console.WriteLine();    
    }
    
    static void ClassExample()
    {
        StringHolderClass holderClass = new();
        holderClass.Value = "Hello";
        ChangeValue(holderClass);

        Console.WriteLine(holderClass.Value); //→ Hello world
        return;

        void ChangeValue(StringHolderClass stringHolderClass)
        {
            stringHolderClass.Value += " world";
        }
    }

    static void ClassExample2()
    {
        StringHolderClass holderClass = new();
        holderClass.Value = "Hello";
        ChangeValue(holderClass);

        Console.WriteLine(holderClass.Value); //→ Hello
        return;

        void ChangeValue(StringHolderClass stringHolderClass)
        {
            stringHolderClass = new() { Value = "Goodbye World" };
        }
    }

    static void ClassExample3()
    {
        StringHolderClass holderClass = new();
        holderClass.Value = "Hello";
        ChangeValue(ref holderClass);

        Console.WriteLine(holderClass.Value); //→ Hello world
        return;

        void ChangeValue(ref StringHolderClass stringHolderClass)
        {
            stringHolderClass.Value += " world";
        }
    }

    static void ClassExample4()
    {
        StringHolderClass holderClass = new();
        holderClass.Value = "Hello";
        ChangeValue(ref holderClass);

        Console.WriteLine(holderClass.Value); //→ Goodbye World
        return;

        void ChangeValue(ref StringHolderClass stringHolderClass)
        {
            stringHolderClass = new() { Value = "Goodbye World" };
        }
    }

    static void ClassExample5()
    {
        StringHolderClass holderClass = new();
        holderClass.Value = "Hello";
        ChangeValue(holderClass);

        Console.WriteLine(holderClass.Value); //→ Hello world
        return;

        void ChangeValue(in StringHolderClass stringHolderClass)
        {
            stringHolderClass.Value += " world";

            // stringHolderClass = new() { Value = "Goodbye World" }; -> can't assign when using 'in' keyword
        }
    }

    static void StructExample()
    {
        StringHolderStruct holder = new();
        holder.Value = "Hello";
        ChangeValue(holder);

        Console.WriteLine(holder.Value); //→ Hello (copy mutated, original unchanged)

        void ChangeValue(StringHolderStruct stringHolderStruct)
        {
            stringHolderStruct.Value += " world";
        }
    }

    static void StructExample2()
    {
        StringHolderStruct holder = new();
        holder.Value = "Hello";
        ChangeValue(holder);

        Console.WriteLine(holder.Value); //→ Hello

        void ChangeValue(StringHolderStruct stringHolderStruct)
        {
            stringHolderStruct = new() { Value = "Goodbye World" };
        }
    }

    static void StructRefExample()
    {
        StringHolderStruct holder = new();
        holder.Value = "Hello";
        ChangeValue(ref holder);

        Console.WriteLine(holder.Value); // → Hello world

        void ChangeValue(ref StringHolderStruct stringHolderStruct)
        {
            stringHolderStruct.Value += " world";
        }
    }

    static void StructRefExample2()
    {
        StringHolderStruct holder = new();
        holder.Value = "Hello";
        ChangeValue(ref holder);

        Console.WriteLine(holder.Value); // → Goodbye World

        void ChangeValue(ref StringHolderStruct stringHolderStruct)
        {
            stringHolderStruct = new() { Value = "Goodbye World" };
        }
    }

    static void StructInExample()
    {
        StringHolderStruct holder = new();
        holder.Value = "Hello";
        ChangeValue(in holder);

        Console.WriteLine(holder.Value); // → Hello

        void ChangeValue(in StringHolderStruct stringHolderStruct)
        {
            // stringHolderStruct.Value += " world"; -> can't modify/mutate
            // stringHolderStruct = new() { Value = "Goodbye World" }; -> can't reassign
        }
    }

    static void RefStructExample()
    {
        StringHolderRefStruct holder = new();
        holder.Value = "Hello";
        ChangeValue(ref holder);

        Console.WriteLine(holder.Value); // → Hello world

        void ChangeValue(ref StringHolderRefStruct stringHolderStruct)
        {
            stringHolderStruct.Value += " world";
        }
    }
}