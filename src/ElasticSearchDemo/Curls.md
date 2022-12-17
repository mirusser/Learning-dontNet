# Curls

To test `Api` running locally

```curl
curl --request GET \
  --url https://localhost:7291/Stock/names \
  --header 'accept: */*'
```

```curl
curl --request GET \
  --url https://localhost:7291/Stock/names/GLW \
  --header 'accept: */*'
```

```curl
curl --request GET \
  --url https://localhost:7291/Stock/volumes/MSFT \
  --header 'accept: */*'
```

```curl
curl --request GET \
  --url https://localhost:7291/Stock/search/MSFT \
  --header 'accept: */*'
```
