# Queries

## Searching
```json
GET stock-demo-v1/_search 
{
  "track_total_hits": true
}
```

```json
GET stock-demo/_search
```

### Filtering
```json
POST stock-demo/_search
{
  "size": 10,
  "query": {
    "constant_score": {
      "filter": {
        "term": {
          "name.keyword": {
            "value": "MSFT"
          }
        }
      }
    }
  }
}
```

## Delete

### Delete index
```json
DELETE /stock-demo-v1
```

```json
DELETE /stock-demo-v2
```

## Additional info

### Mapping
```json
GET stock-demo-v1/_mapping
```