﻿using Adapter;

Adaptee adaptee = new();
ITarget target = new Adapter.Adapter(adaptee);

Console.WriteLine("Adaptee interface is incompatible with the client.");
Console.WriteLine("But with adapter client can call it's method.");

Console.WriteLine(target.GetRequest());