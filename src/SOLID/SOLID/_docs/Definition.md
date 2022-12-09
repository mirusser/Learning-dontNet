# SOLID

The SOLID principles are a set of best practices in object-oriented software design. 
They were first introduced by Robert C. Martin, also known as "Uncle Bob," in his 2000 paper "Design Principles and Design Patterns." 
The principles are intended to make software design more maintainable and scalable.

The SOLID principles are:
1. __Single Responsibility Principle__
    - A class should have only one reason to change.
2. __Open/Closed Principle__
    - A class should be open for extension but closed for modification.
3. __Liskov Substitution Principle__
    - Derived classes should be substitutable for their base classes.
4. __Interface Segregation Principle__
    - Clients should not be forced to depend on methods they do not use.
5. __Dependency Inversion Principle__
    - High-level modules should not depend on low-level modules. Abstractions should not depend on details.
--- 
## Single Responsibility Principle

This principle states that a class should have only one reason to change. This means that a class should have only one responsibility, or one job to do. 

For example, consider a `BankAccount` class. This class might be responsible for managing the balance of a bank account, but it should not be responsible for handling user input or generating user reports. These responsibilities should be handled by separate classes.

### Bad practice example:
Sometimes it seems that a class has many responsibilities, but on a first glance it seems that many of these resposibilities (if not all of them) make sens for given class.

This example may point on a situation in which given class name is to broad or there aren't strict naming conventions implemented yet (e.g `SthHelper`, `SthService`, `SthManager`)


---

## Open/Closed Principle
This principle states that a class should be open for extension but closed for modification. This means that you should be able to extend the behavior of a class without modifying its source code. 

In C#, you can achieve this by using polymorphism and abstract classes. 

For example, consider a `Shape` abstract class with subclasses `Circle`, `Square`, and `Triangle`. You can add a new `Rectangle` class without modifying the `Shape` class.

---

## Liskov Substitution Principle
This principle states that derived classes should be substitutable for their base classes. This means that if a client expects an object of the base class, you should be able to provide an object of the derived class without breaking the client. 

In C#, you can achieve this by using inheritance and making sure that the derived classes adhere to the same contract as the base class. 

For example, consider a `Vehicle` base class and `Car` and `Bicycle` derived classes. A client that expects a `Vehicle` object should be able to use a `Car` or `Bicycle` object without any issues.

---

## Interface Segregation Principle
This principle states that clients should not be forced to depend on methods they do not use. In other words, a class should not implement an interface that contains methods that the class does not use. 

In C#, you can achieve this by using multiple, smaller interfaces that each define a specific set of methods. 

For example, consider an `IComparable` interface that defines a `CompareTo()` method. If a class only needs to compare objects by their length, it should implement an `ILengthComparable` interface that only contains a `CompareToLength()` method.

---

## Dependency Inversion Principle 
This principle states that high-level modules should not depend on low-level modules. Instead, both should depend on abstractions. 

In C#, you can achieve this by using dependency injection and abstractions such as interfaces and abstract classes. 

For example, consider a `NotificationService` class that sends email notifications. This class should not depend on a specific `EmailSender` class, but instead should depend on an `IEmailSender` interface. This way, you can change the implementation of the email sender without affecting the `NotificationService`.