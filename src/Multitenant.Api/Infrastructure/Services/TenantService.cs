using Core.Contracts;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly TenantSettings tenantSettings;
    private readonly HttpContext httpContext;
    private Tenant currentTenant;
    
    public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
    {
        this.tenantSettings = tenantSettings.Value;
        httpContext = contextAccessor.HttpContext;

        if (httpContext == null)
        {
            return;
        }
        
        if (httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
        {
            SetTenant(tenantId);
        }
        else
        {
            throw new Exception("Invalid Tenant!");
        }
    }
    
    private void SetTenant(string tenantId)
    {
        currentTenant = tenantSettings.Tenants.FirstOrDefault(a => a.Id == tenantId);
        
        if (currentTenant == null)
        {
            throw new Exception("Invalid Tenant!");
        }
        
        if (string.IsNullOrEmpty(currentTenant.ConnectionString))
        {
            SetDefaultConnectionStringToCurrentTenant();
        }
    }
    
    private void SetDefaultConnectionStringToCurrentTenant()
    {
        currentTenant.ConnectionString = tenantSettings.Defaults.ConnectionString;
    }
    
    public string GetConnectionString()
    {
        return currentTenant?.ConnectionString;
    }
    
    public string GetDatabaseProvider()
    {
        return tenantSettings.Defaults?.DbProvider;
    }
    
    public Tenant GetTenant()
    {
        return currentTenant;
    }
}