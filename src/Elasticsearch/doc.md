## Docker

```d
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.14.2
```

## Create users

- add bulk:

```curl
curl --request POST \
  --url http://localhost:9200/users/_bulk \
  --header 'Content-Type: application/json' \
  --data '{"index":{}}
{"name": "User1", "age": "30", "address": "123 Street"}
{"index":{}}
{"name": "User2", "age": "32", "address": "23 Street"}
{"index":{}}
{"name": "User3", "age": "34", "address": "532 Street"}
{"index":{}}
{"name": "User4", "age": "35", "address": "234 Street"}
'
```
