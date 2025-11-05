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
        ClassChange_Mutate(holder); // mutate object via the same reference
        Console.WriteLine(holder.Value); // Expected: Hello world
    }

    // Reassigning the *parameter variable* to a new object does not change the caller's variable,
    // because by default we passed the reference by value (a copy of the reference).
    private static void Class_ReassignLocalParameter_DoesNotChangeCaller()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChange_ReassignParameter(holder); // only the local parameter now points to new object
        Console.WriteLine(holder.Value); // Expected: Hello
    }

    // With 'ref', we pass the *variable itself* by reference, so we can mutate the target object
    // and it's the same variable in the caller.
    private static void Class_Ref_MutateThroughReference()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeRef_Mutate(ref holder);
        Console.WriteLine(holder.Value); // Expected: Hello world
    }

    // With 'ref', we can also *reassign* the caller's variable to a different object.
    private static void Class_Ref_ReassignChangesCaller()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeRef_Reassign(ref holder);
        Console.WriteLine(holder.Value); // Expected: Goodbye World
    }

    // 'in' on a class param: the *reference* itself is readonly, but the object is still mutable.
    // So you can mutate object state, but you cannot reassign the parameter to a new object.
    private static void Class_In_MutateObjectButNotReference()
    {
        var holder = new StringHolderClass { Value = "Hello" };
        ClassChangeIn_MutateObject(in holder);
        Console.WriteLine(holder.Value); // Expected: Hello world
    }

    // -------------------------------
    // STRUCT (value type) examples
    // -------------------------------

    // Passing a struct by value copies it. Mutations affect only the copy, not the caller's variable.
    private static void Struct_ByValue_CopyIsMutatedOriginalUnaffected()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChange_Mutate(holder); // mutate the copy
        Console.WriteLine(holder.Value); // Expected: Hello
    }

    private static void Struct_ByValue_ReassignLocalParameter_DoesNotChangeCaller()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChange_ReassignParameter(holder); // reassigns only the local copy
        Console.WriteLine(holder.Value); // Expected: Hello
    }

    // 'ref' with a struct gives access to the caller's variable (no copy),
    // so mutations are applied to the original.
    private static void Struct_Ref_MutateOriginal()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeRef_Mutate(ref holder);
        Console.WriteLine(holder.Value); // Expected: Hello world
    }

    // With 'ref', reassignment replaces the caller's value entirely.
    private static void Struct_Ref_ReassignChangesCaller()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeRef_Reassign(ref holder);
        Console.WriteLine(holder.Value); // Expected: Goodbye World
    }

    // 'in' with a struct is a readonly reference: no mutation through the parameter and no reassignment.
    private static void Struct_In_Readonly_NoMutationNoReassign()
    {
        var holder = new StringHolderStruct { Value = "Hello" };
        StructChangeIn_Readonly(in holder);
        Console.WriteLine(holder.Value); // Expected: Hello
    }

    // -------------------------------
    // REF STRUCT example
    // -------------------------------

    // ref struct can only live on the stack and follows ref semantics below.
    private static void RefStruct_Ref_MutateOriginal()
    {
        var holder = new StringHolderRefStruct { Value = "Hello" };
        RefStructChangeRef_Mutate(ref holder);
        Console.WriteLine(holder.Value); // Expected: Hello world
    }

    // ===== Change helpers (one responsibility each) =====

    // Class helpers
    private static void ClassChange_Mutate(StringHolderClass c)
        => c.Value += " world";

    private static void ClassChange_ReassignParameter(StringHolderClass c)
        => c = new StringHolderClass { Value = "Goodbye World" }; // caller's variable unaffected

    private static void ClassChangeRef_Mutate(ref StringHolderClass c)
        => c.Value += " world";

    private static void ClassChangeRef_Reassign(ref StringHolderClass c)
        => c = new StringHolderClass { Value = "Goodbye World" }; // caller's variable now points to new object

    private static void ClassChangeIn_MutateObject(in StringHolderClass c)
    {
        c.Value += " world"; // allowed: object is mutable; the readonly is on the *reference*, not the object
        // c = new StringHolderClass(); // not allowed: cannot reassign 'in' parameter
    }

    // Struct helpers
    private static void StructChange_Mutate(StringHolderStruct s)
        => s.Value += " world"; // mutates the copy

    private static void StructChange_ReassignParameter(StringHolderStruct s)
        => s = new StringHolderStruct { Value = "Goodbye World" }; // reassigns only the copy

    private static void StructChangeRef_Mutate(ref StringHolderStruct s)
        => s.Value += " world"; // mutates caller's variable

    private static void StructChangeRef_Reassign(ref StringHolderStruct s)
        => s = new StringHolderStruct { Value = "Goodbye World" }; // replaces caller's value

    private static void StructChangeIn_Readonly(in StringHolderStruct s)
    {
        // s.Value += " world"; // not allowed: cannot mutate through 'in' for a struct
        // s = new StringHolderStruct(); // not allowed: cannot reassign an 'in' parameter
        _ = s.ToString(); // safe read-only usage
    }

    // Ref struct helpers
    private static void RefStructChangeRef_Mutate(ref StringHolderRefStruct s)
        => s.Value += " world";
}