# CQS

## What is a CQS?

**CQS** (Command Query Seperation) design pattern separates two method of object method that process method called command and return data method called query.

Every method should either be a command that performs an action, or a query that returns data to the caller, but never both.

Methods should return a value only if they create no side effects.

Simply put: a query should never mutate state, while a command can mutate state but should never have a return value.

> "A command (procedure) does something but does not return a result. A query (function or attribute) returns a result but does not change the state"
> ~ Bertrand Meyer

It orignaly talked only about the methods.

Example:

```csharp
interface IStack<T>
{
    void Push(T value); // command
    T Peek(); // query
    T Pop(); // (remove and return a last item) is this a query or a command (?)
}
```

As it turned out this patters is a bit (too) strict and not always practical

# CQRS

## What is a CQRS?

[CQRS article by Martin Fowler](https://www.martinfowler.com/bliki/CQRS.html)

**CQRS** (Command Query responsibility Segragation) is a design pattern that seperates the read and write operations (of a data source).

> "... The fundamental difference is that in CQRS objects are **split into two objects**, one containing the Commands one containing the Queries"
> ~ Greg Young

It's more important to have clear bounderies between what manipulate the data and what doesn't manipulate the data. (With comparision to the CQS)

Example:

```csharp
interface ICommandStack<T>
{
    void Push(T value);
    T Pop();
}

interface IQueryStack<T>
{
    T Peek();
}
```

##### How to separate?

Ask a question?
_Am I changing data?_
If yes, then it's a command, otherwise if only reads data, then its a query.

### Misconceptions

- That it talks about the entire application, and you have to have to applications (one for read and one to write data (from the database))
- That you need (at least) two databases, one for read operations and one for write operations

Both of this approaches can be implemented, but they are not needed for this pattern to work.

### Problem in large systems

There is possibilty of creating (two) 'god like' services that contains all the logic for read and write operations

The solution is to separate the concerns not by services but by the features (use cases).

_(Mediator (and MediatR) comes to the rescue)_

### Pros of CQRS

#### Optimised Data Transfer Objects

- Thanks to the segregated approach of this pattern, we will no longer need those comples model classes within our application. Rather we have one model per data operation that gives us all the flexibility that we could need.

#### Highly scalable

- Having control over the models in accordance with the type of data operations makes your application highly scalable in the long run.

#### Improved performance

- As expierience and practically shows there are always much more **Read Operations** as compared to the **Write Operations**.
  With this pattern you could speed up the performance on your read operations by introducing a cache or NOSQL Db like Redis or Mongo.

#### Secure Parallel Operations

- Since we have dedicated models per oprtation, there is no possibility of data loss while doing parellel operations.

### Cons of CQRS

#### Added Complexity and More Code

- CQRS is a code demanding pattern. In other words, you will end up with at least 3 or 4 times more code-lines than you usually would. But everything comes for a price.
  (This is rather a small price to pay while getting the awesome features and possibilities with the pattern.)
