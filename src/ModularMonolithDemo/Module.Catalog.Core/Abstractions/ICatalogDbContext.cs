﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Module.Catalog.Core.Entities;

namespace Module.Catalog.Core.Abstractions;

public interface ICatalogDbContext
{
    public DbSet<Brand> Brands { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

