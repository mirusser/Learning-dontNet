﻿using System.Collections.Generic;

namespace Core.Settings
{
    public class TenantSettings
    {
        public Configuration DefaultConfiguration { get; set; }
        public List<Tenant> Tenants { get; set; }
    }

    public class Tenant
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ConnectionString { get; set; }
    }

    public class Configuration
    {
        public string DbProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}