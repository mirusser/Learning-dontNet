[Redis documentation](https://redis.io/documentation)

### What is Redis?

- A key/value datastore
- Synonymous with Caching
- Reputation for being incredibly fast due to:
  - Lightweight simple architecture
  - Key/Value retrieval
  - In Memory

### Redis Data Types

- Strings
- Lists
- Hashes
- Sets
- Sorted Sets

More info: [Redis data types tutorial](https://redis.io/docs/data-types/tutorial/)

##### Strings

- Simplest type of value to be associated to a key
- 1 to 1 mapping betwen Key and Value
- Useful for a number of use cases
- Set using **SET** (command)
  - `set <key><somevalue>`
  - e.g. `set platform:10010 Docker`
- Return using **GET**
  - `get <key>`
  - e.g. `get platform:10010`
- Remove (delete) using **DEL**
  - `del <key>`
  - e.g. `del platform:10010`

##### Sets

- Unordered collections of Strins
- 1 to many mapping between key and value
- Set using **SADD**
  - `Sadd myset 1 2 3`
- Returned using **SMEMBERS**
  - `smembers myset`

##### Hashes

- Stores Field/Value pairs (in a value)
  - `<key:<field:value, field:value...>>`
- Suitable for storing structure objects
- Set using **HMSET**
  - `hmset <id><field1><value1>`
- Get all items using **HGETALL**
  - `hgetall <id>`
- Get individual items using **HGET**
  - `hget <id><field>`

### Redis keys

- Binary safe
  - You can use any binary sequence as a key
- Very long keys are not a good idea
  - Memory impact (max size 512MB)
  - Look up performance
- Very short keys are not a good idea either
  - Should be readable with a structured 'schema'
  - object-type: id works well e.g. _user:234432_

### Redis as a Primary DB

- Redis does more than act as a cache, it can also operate as a:
  - Database
  - Message Broker
- Reputation as an 'In Memory' store can mislead...
- Redis **does offer** a number of approaches to data persistence
  - You don't loose your data if Redis restarts etc.

### Which Package?

1. Microsoft.Extensions.Caching.Redis (essentially deprecated)
2. Microsoft.Extensions.Caching.StackExchangeRedis
3. StackExchange.Redis

Boils down to whether you want to use:

- _IDistributedCache_ (restricted datatypes)
- _IConnectionMultiplexer_ (all datatypes available)
