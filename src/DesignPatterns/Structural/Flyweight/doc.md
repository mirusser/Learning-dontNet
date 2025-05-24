# Simple Explanation

Imagine you have to draw 1,000,000 little trees in a forest. If you create a new Tree object for each tree, 
you’ll quickly run out of memory. 
The Flyweight pattern solves this by sharing as much data as possible between similar objects:

- Intrinsic data: The part of an object’s state that is always the same (e.g., the shape or color of a tree).
- Extrinsic data: The part that can change from one object to another (e.g., the tree’s position in the forest).

With Flyweight, you store one copy of the intrinsic data in a shared object, 
and you supply the extrinsic data only when you need to draw or work with that object. 
This dramatically reduces the number of full objects you need to keep in memory.

# Key Components

IFlyweight: Interface declaring operations that can use extrinsic state.

ConcreteFlyweight: Implements IFlyweight and stores intrinsic state.

FlyweightFactory: Manages a pool (cache) of flyweight objects. Returns an existing instance if one with the requested intrinsic state exists; otherwise, creates it.

Client: Supplies extrinsic state when calling operations on flyweights.

# When to Use Flyweight

- High Object Count: When you need to create millions of similar objects.
- Shared State: When large parts of object state can be shared.
- Performance-Critical Applications: Games, graphical editors, document rendering (characters in a text editor), etc.

By applying the Flyweight pattern, you optimize both memory footprint and performance, while keeping your code clean and maintainable.