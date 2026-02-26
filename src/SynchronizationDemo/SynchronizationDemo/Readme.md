
## Mutex and Semaphore - simple explanation

Think of a bathroom key and a bag of tickets.

**Mutex** is the bathroom key:
- There is only one key.
- If you have the key, you can go in.
- Nobody else can go in until you give the key back.

**Semaphore** is the bag of tickets:
- There are, for example, 3 tickets.
- Up to 3 people can do the thing at the same time.
- If all tickets are taken, the next person must wait until someone returns one.

Both are synchronization tools used when multiple threads/tasks want access to something.
> Use a mutex when exactly one thread should access a resource at a time.

> Use a semaphore when up to N threads can access a resource at a time.

They solve different concurrency problems.

## Mutex
A mutex (mutual exclusion) ensures exclusive access.

Only one thread/process can own it at once.

When to use it:

- Protecting a shared resource that must not be used concurrently
- Ensuring only one instance of some critical operation runs at a time
- Cross-process coordination (important distinction: Mutex can be system-wide if named)

## Semaphore
A semaphore keeps a counter.

If the count is 3:
- first 3 threads can enter
- the 4th waits
- when one leaves, another can enter

When to use it:
- Limit concurrency
- Control access to a pool of resources
- Throttle work (e.g. only 5 API calls at once)

---
In .NET for synchronization usually are used:
- `lock` / `Monitor`
- `Mutex`
- `Semaphore`
- `SemaphoreSlim`

---

## Key differences

### Ownership
- Mutex has the concept of an owner thread.
- Semaphore does not “belong” to one thread in the same way; it just tracks available slots.

### Capacity
- Mutex = 1
- Semaphore = N

### Scope
- Mutex can be named and shared across processes.
- lock/Monitor cannot.
- Semaphore can also be named across processes; SemaphoreSlim is in-process only.

---

## Practical “which one should I use?”

- Protect shared in-memory state in one process → lock
- Need async-friendly throttling → SemaphoreSlim
- Need to allow up to N concurrent operations → SemaphoreSlim or Semaphore
- Need cross-process exclusive access → Mutex
- Need cross-process limited slots → Semaphore