# How to create custom dotnet tool

1. In `.csproj` file
add the following

```xml
  <PropertyGroup>
    ...
    <PackAsTool>true</PackAsTool>
    <ToolCommandName>Tool_Command_Name</ToolCommandName>
    ...
  </PropertyGroup>
```

2. Pack the project

using dotnet CLI: 
```
dotnet pack -c Release -o <folder>
```

e.g.
```
dotnet pack -c Release -o assets
```

3. Install tool:

```
dotnet tool install --global --add-source <folder>/ <ToolCommandName>
```
e.g.
```
dotnet tool install --global --add-source assets/ SayConf
```

