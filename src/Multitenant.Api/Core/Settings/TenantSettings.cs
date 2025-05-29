namespace Core.Settings;

public class TenantSettings
{
    public Configuration Defaults { get; set; } = null!;
    public List<Tenant> Tenants { get; set; } = null!;
}

public class Configuration
{
    public string DbProvider { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
}

public class Tenant
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
}
