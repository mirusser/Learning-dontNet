using Bridge;

Client client = new();

// The client code should be able to work with any pre-configured
// abstraction-implementation combination.
Abstraction abstraction = new(new ConcreteImplementationA());
client.ClientCode(abstraction);

Console.WriteLine();

abstraction = new ExtendedAbstraction(new ConcreteImplementationB());
client.ClientCode(abstraction);