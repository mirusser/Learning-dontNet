using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileDemo.Models.Context
{
    public interface IApplicationDbContext
    {
        DbSet<FileOnFileSystemModel> FilesOnFileSystem { get; set; }
        DbSet<FileOnDatabaseModel> FilesOnDatabaseModel { get; set; }

        Task<int> SaveChangesAsync();
    }
}
