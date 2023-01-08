
using System.Collections;

int someNumber = 420;

//implicit conversion (boxing)
object someNumberObject = someNumber;

// unboxing
int unboxed = (int)someNumberObject;

var arrayOfInts = Enumerable.Range(69, 420).ToArray();

var arrayList = new ArrayList(arrayOfInts);