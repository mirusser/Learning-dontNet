# Mediator

[Mediator definition (Refactoring guru)](https://refactoring.guru/design-patterns/mediator)

## What is a mediator?

**Mediator** is a design (behavioral) pattern that reduces dependencies (the coupling) between various components of an application (mostly objects).

It restricts direct communications between the objects and forces them to communicate indirectly only via a (special) mediator (object/layer).

It essentialy create the layer in between so that the interaction between objects happen via the layer and thus helping in implementing lose-coupling between objects.

Is mostly just a code organisation pattern.

> With the mediator pattern, communication between objects is encapsulated within a mediator object. Objects no longer communicate directly with each other, but instead communicate through the mediator. This reduces the dependencies between communicating objects, thereby reducing coupling.
> ~ Wikipedia

## Real world analogy

> Pilots of aircraft that approach or depart the airport control area don’t communicate directly with each other. Instead, they speak to an air traffic controller, who sits in a tall tower somewhere near the airstrip

### What problems this pattern solve?

- Tight coupling between a set of interacting with each other objects should be avoided.

Interaction of the objects with other objects directly make changes of the interaction hard or impossible to change and at the same time it makes the said objects not reusable and hard to test. (Single responsibility principle)

## Problem (con) of using Mediator

- Over time a mediator can evolve into a **[God Object](https://en.wikipedia.org/wiki/God_object)**. (MediatR package to the rescue (?))

# MediatR Pipeline Behaviour

In normal approach request is validated only after it has reached inside the application.

In approach this approach you wire up the validation logics withing the pipeline, so that the flow becomes like user sends request through pipeline (if request is valid, hit the application logics, else throws a valid exception).

### Examples

- Validation
- Logging
- Caching
- etc.
