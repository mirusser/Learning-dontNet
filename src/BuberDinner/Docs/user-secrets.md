## user secrets

### init

`dotnet user-secrets init --project .\BuberDinner.Api\`

### set

`dotnet user-secrets set --project .\BuberDinner.Api\ "JwtSettings.Secret" "super-secret-key-from-user-secrets"`

### list (show all stored secrets)

`dotnet user-secrets list --project .\BuberDinner.Api\ `
