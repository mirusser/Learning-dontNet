using ParseAnyType;

var numberAsText = "420";

// 'old' way of parsing
var numberAsInt = int.Parse(numberAsText); // you could use TryParse method here
var numberAsDouble = double.Parse(numberAsText);

Console.WriteLine(numberAsInt);
Console.WriteLine(numberAsDouble);

// fancy new way
numberAsInt = numberAsText.Parse<int>();
numberAsDouble = numberAsText.AsSpan().Parse<double>();

Console.WriteLine(numberAsInt);
Console.WriteLine(numberAsDouble);

// more realistic usage
numberAsText = "6,9";
var point2d = numberAsText.Parse<Point2d>();
Console.WriteLine(point2d);

// with spans
var numberAsSpan = numberAsText.AsSpan();
point2d = Point2d.Parse(numberAsSpan);
Console.WriteLine(point2d);

Console.WriteLine("Hello, World!");