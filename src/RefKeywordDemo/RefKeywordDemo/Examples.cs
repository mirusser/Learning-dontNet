namespace RefKeywordDemo;

// Demonstrates how C# parameter passing + type kinds affect mutation and reassignment.
// - Class (reference type): the *reference* is passed by value by default.
// - Struct (value type): the *value* is copied by default.
// - 'ref' passes a variable by reference (you can mutate the target and also reassign it).
// - 'in' passes by readonly reference (you can’t reassign the parameter itself;
//    for structs you also can’t mutate through the parameter; for classes you *can*
//    still mutate the *object’s state*, because the object isn’t readonly—only the reference is).

public static class Examples
{
    public static void Run()
    {
        PrintHeader("CLASS EXAMPLES");
        Class_MutateThroughReference();
        Class_ReassignLocalParameter_DoesNotChangeCaller();
        Class_Ref_MutateThroughReference();
        Class_Ref_ReassignChangesCaller();
        Class_In_MutateObjectButNotReference();

        Console.WriteLine();

        PrintHeader("STRUCT EXAMPLES");
        Struct_ByValue_CopyIsMutatedOriginalUnaffected();
        Struct_ByValue_ReassignLocalParameter_DoesNotChangeCaller();
        Struct_Ref_MutateOriginal();
        Struct_Ref_ReassignChangesCaller();
        Struct_In_Readonly_NoMutationNoReassign();

        Console.WriteLine();

        PrintHeader("REF STRUCT EXAMPLE");
        RefStruct_Ref_MutateOriginal();
    }

    private static void PrintHeader(string title)
    {
        Console.WriteLine(new string('-', 48));
        Console.WriteLine(title);
        Console.WriteLine(new string('-', 48));
    }

    // -------------------------------
    // CLASS (reference type) examples
    // -------------------------------

    // Default param passing for classes == pass the *reference* by value.
    // Mutating the object *through* that reference changes the caller's object.
    private static void Class_MutateThroughReference()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeMutate(holder); // mutate object via the same reference
        Console.WriteLine($"{nameof(Class_MutateThroughReference)} - {holder.Value}"); // Expected: Hello world
        return;

        void ClassChangeMutate(StringHolderClass c)
            => c.Value += " world";
    }

    // Reassigning the *parameter variable* to a new object does not change the caller's variable,
    // because by default we passed the reference by value (a copy of the reference).
    private static void Class_ReassignLocalParameter_DoesNotChangeCaller()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeReassignParameter(holder); // only the local parameter now points to new object
        Console.WriteLine($"{nameof(Class_ReassignLocalParameter_DoesNotChangeCaller)} - {holder.Value}"); // Expected: Hello
        return;

        void ClassChangeReassignParameter(StringHolderClass c)
            => c = new StringHolderClass { Value = "Goodbye World" }; // caller's variable unaffected
    }

    // With 'ref', we pass the *variable itself* by reference, so we can mutate the target object
    // and it's the same variable in the caller.
    private static void Class_Ref_MutateThroughReference()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeRefMutate(ref holder);
        Console.WriteLine($"{nameof(Class_Ref_MutateThroughReference)} - {holder.Value}"); // Expected: Hello world
        return;

        void ClassChangeRefMutate(ref StringHolderClass c)
            => c.Value += " world";
    }

    // With 'ref', we can also *reassign* the caller's variable to a different object.
    private static void Class_Ref_ReassignChangesCaller()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeRefReassign(ref holder);
        Console.WriteLine($"{nameof(Class_Ref_ReassignChangesCaller)} - {holder.Value}"); // Expected: Goodbye World
        return;

        void ClassChangeRefReassign(ref StringHolderClass c)
            => c = new StringHolderClass { Value = "Goodbye World" }; // caller's variable now points to new object
    }

    // 'in' on a class param: the *reference* itself is readonly, but the object is still mutable.
    // So you can mutate object state, but you cannot reassign the parameter to a new object.
    private static void Class_In_MutateObjectButNotReference()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeInMutateObject(in holder);
        Console.WriteLine($"{nameof(Class_In_MutateObjectButNotReference)} - {holder.Value}"); // Expected: Hello world
        return;

        void ClassChangeInMutateObject(in StringHolderClass c)
        {
            c.Value += " world"; // allowed: object is mutable; the readonly is on the *reference*, not the object
            // c = new StringHolderClass(); // not allowed: cannot reassign 'in' parameter
        }
    }

    // -------------------------------
    // STRUCT (value type) examples
    // -------------------------------

    // Passing a struct by value copies it. Mutations affect only the copy, not the caller's variable.
    private static void Struct_ByValue_CopyIsMutatedOriginalUnaffected()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeMutate(holder); // mutate the copy
        Console.WriteLine($"{nameof(Struct_ByValue_CopyIsMutatedOriginalUnaffected)} - {holder.Value}"); // Expected: Hello
        return;

        void StructChangeMutate(StringHolderStruct s)
            => s.Value += " world"; // mutates the copy
    }

    private static void Struct_ByValue_ReassignLocalParameter_DoesNotChangeCaller()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeReassignParameter(holder); // reassigns only the local copy
        Console.WriteLine($"{nameof(Struct_ByValue_ReassignLocalParameter_DoesNotChangeCaller)} - {holder.Value}"); // Expected: Hello
        return;

        void StructChangeReassignParameter(StringHolderStruct s)
            => s = new StringHolderStruct { Value = "Goodbye World" }; // reassigns only the copy
    }

    // 'ref' with a struct gives access to the caller's variable (no copy),
    // so mutations are applied to the original.
    private static void Struct_Ref_MutateOriginal()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeRefMutate(ref holder);
        Console.WriteLine($"{nameof(Struct_Ref_MutateOriginal)} - {holder.Value}"); // Expected: Hello world
        return;

        void StructChangeRefMutate(ref StringHolderStruct s)
            => s.Value += " world"; // mutates caller's variable
    }

    // With 'ref', reassignment replaces the caller's value entirely.
    private static void Struct_Ref_ReassignChangesCaller()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeRefReassign(ref holder);
        Console.WriteLine($"{nameof(Struct_Ref_ReassignChangesCaller)} - {holder.Value}"); // Expected: Goodbye World
        return;

        void StructChangeRefReassign(ref StringHolderStruct s)
            => s = new StringHolderStruct { Value = "Goodbye World" }; // replaces caller's value
    }

    // 'in' with a struct is a readonly reference: no mutation through the parameter and no reassignment.
    private static void Struct_In_Readonly_NoMutationNoReassign()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeInReadonly(in holder);
        Console.WriteLine($"{nameof(Struct_In_Readonly_NoMutationNoReassign)} - {holder.Value}"); // Expected: Hello
        return;

        void StructChangeInReadonly(in StringHolderStruct s)
        {
            // s.Value += " world"; // not allowed: cannot mutate through 'in' for a struct
            // s = new StringHolderStruct(); // not allowed: cannot reassign an 'in' parameter
            _ = s.ToString(); // safe read-only usage
        }
    }

    // -------------------------------
    // REF STRUCT example
    // -------------------------------

    // ref struct can only live on the stack and follows ref semantics below.
    private static void RefStruct_Ref_MutateOriginal()
    {
        var holder = new StringHolderRefStruct { Value = "Hello" };
        RefStructChangeRefMutate(ref holder);
        Console.WriteLine($"{nameof(RefStruct_Ref_MutateOriginal)} - {holder.Value}"); // Expected: Hello world
        return;

        void RefStructChangeRefMutate(ref StringHolderRefStruct s)
            => s.Value += " world";
    }
}