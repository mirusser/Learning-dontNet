using Core.Settings;
using Microsoft.Extensions.Primitives;

namespace Core.Contracts;

public interface ITenantService
{
    public string GetDatabaseProvider();
    public string GetConnectionString();
    public Tenant GetTenant();
}