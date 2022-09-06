## Spin up the redis container

In the directory with the _docker-compose.yaml_

- to run the container:
  `docker compose up -d`
- to stop the container:
  `docker compose stop`
- to stop and remove container:
  `docker compose down`

## Open Redis CLI (how to run it in a container)

- check for container_id with `docker ps`

`docker exec -it <container_id> /bin/bash`

e.g.:
`docker exec -it 1c6670d2b753 /bin/bash`


## Redis CLI
- open:
`redis-cli`
- exit
`exit`
