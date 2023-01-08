# How are types allocated in c#

## Stack

1. Value type declared as variable in method
2. Value type declared as parameter in method
3. ref structs

## Heap

1. Value type declared as a member or a class
2. Reference types

## Depends (Heap or Stack)

1. Value type declared as a member of struct (wherever the struct has been allocated)
