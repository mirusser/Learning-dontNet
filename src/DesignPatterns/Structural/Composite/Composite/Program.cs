﻿using Composite;

Client client = new();

// This way the client code can support the simple leaf
// components...
Leaf leaf = new();
Console.WriteLine("Client: I get a simple component:");
client.ClientCode(leaf);

// ...as well as the complex composites.
Composite.Composite tree = new ();
Composite.Composite branch1 = new();
branch1.Add(new Leaf());
branch1.Add(new Leaf());
Composite.Composite branch2 = new();
branch2.Add(new Leaf());
tree.Add(branch1);
tree.Add(branch2);
Console.WriteLine("Client: Now I've got a composite tree:");
client.ClientCode(tree);

Console.Write("Client: I don't need to check the components classes even when managing the tree:\n");
client.ClientCode2(tree, leaf);