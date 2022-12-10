
using System;
using AddPropertiesToExternalClassDemo;
using ExternalLib;

//var obj = new SomeExternalClass();

//obj.GetProperties().Text = "Hello";

//Console.WriteLine(obj.GetProperties().Text);

App app = new();
app.Run();
app.Clean();

GC.Collect();

var objectCount = app.Objects.Count;
var propertiesCount = ExternalObjectExtensions.Data.Count();

Console.WriteLine(objectCount);
Console.WriteLine(propertiesCount);

Console.ReadLine();
