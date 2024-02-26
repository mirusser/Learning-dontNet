using GrpcToRestDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcToRestDemo.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
}
