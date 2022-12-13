
#How to run in Docker without `Dockerfile`

Install package:
```
	Microsoft.NET.Build.Containers
```

run command:
```ps
	dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer
```

---

Note you may add parameters to `.csproj`
```xml
	<PublishProfile>DefaultContainer</PublishProfile>
```

---
You can also specify in `.csproj` (but is not needed):

```xml
<ContainerBaseImage>mcr.microsoft.com/dotnet/aspnet:7.0</ContainerBaseImage>
<ContainerRegistry>...</ContainerRegistry>
<ContainerImageTag>1.2.3-alpha2</ContainerImageTag>
<ContainerImageTags>1.2.3-alpha2;latest</ContainerImageTags>

<ItemGroup>
	<ContainerPort Include="80" Type="tcp />
	<ContainerLabel Include="certification" Value="bla-bla" />
	<ContainerEnvironmentVariable Include="LOGGER_VERBOSITY" Value="Trace" />
</ItemGroup>

<ItemGroup Label="Entrypoint Assignment">
	<ContainerEntrypoint Include="dotnet" />
	<ContainerEntrypoint Include="ef" />
	<ContainerEntrypoint Include="dotnet;ef" />
</ItemGroup>
```