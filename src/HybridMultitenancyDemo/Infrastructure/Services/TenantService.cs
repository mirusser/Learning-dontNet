using System;
using System.Linq;
using Core.Interfaces;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private readonly HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;

            if (_httpContext is not null)
            {
                if (!_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                    throw new Exception("Invalid Tenant!");

                SetTenant(tenantId);
            }
        }

        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(a => a.Id == tenantId);

            if (_currentTenant is null) throw new Exception("Invalid Tenant!");

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                _currentTenant.ConnectionString = _tenantSettings.DefaultConfiguration.ConnectionString;
            }
        }

        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSettings.DefaultConfiguration?.DbProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}