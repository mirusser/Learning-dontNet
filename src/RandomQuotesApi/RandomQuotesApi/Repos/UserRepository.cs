using Microsoft.EntityFrameworkCore;
using RandomQuotesApi.Models;
using RandomQuotesApi.Models.Auth;

namespace RandomQuotesApi.Repos;

public interface IUserRepository
{
    Task<User> GetFirstOrCreateAsync(CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<User?> ValidateCredentialsAsync(string userName, string password, CancellationToken ct = default);
    Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetPermissionNamesAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ValidatePasswordAsync(User user, string password, CancellationToken ct);
    Task<User?> GetByCredentialsAsync(string username, string password, CancellationToken ct);
    Task<IReadOnlyList<string>> GetPermissionsAsync(Guid userId, CancellationToken ct);

    Task<bool> GrantPermissionAsync(Guid userId, string permissionName, CancellationToken ct = default);

    Task<List<Permission>> GetAllPermissions(CancellationToken ct = default);
}

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User> GetFirstOrCreateAsync(CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(ct);

        if (user is null)
        {
            user = new User() { Name = "John Doe" };
            await AddAsync(user, ct);
        }

        return user;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Users
            .Include(u => u.Permissions)
                .ThenInclude(up => up.Permission)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await db.Users.AddAsync(user, ct);
        await db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return db.Users.AnyAsync(q => q.Id == id, ct);
    }

    public Task<User?> ValidateCredentialsAsync(
        string userName,
        string password,
        CancellationToken ct = default)
    {
        // ⚠️ DEMO ONLY – do NOT do this in real apps.
        // Here we just hard-code one demo user.
        if (userName == "demo" && password == "demo123")
        {
            // Return any user from DB, or create a fake one
            var user = db.Users.FirstOrDefault()
                       ?? new User
                       {
                           Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                           Name = "Demo User"
                       };

            return Task.FromResult<User?>(user);
        }

        return Task.FromResult<User?>(null);
    }

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Name == username, ct);

        return user;
    }

    public async Task<IReadOnlyList<string>> GetPermissionNamesAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => up.Permission.Name)
            .ToListAsync(ct);
    }

    public Task<bool> ValidatePasswordAsync(User user, string password, CancellationToken ct)
    {
        return db.Users.AnyAsync(u => u.Id == user.Id, ct);
    }

    public async Task<User?> GetByCredentialsAsync(string username, string password, CancellationToken ct)
    {
        // For now: no hashing — just match (demo only)
        return await db.Users
            .Where(u => u.Name == username /* TODO: validate password/passwordHash here*/)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<string>> GetPermissionsAsync(Guid userId, CancellationToken ct)
    {
        return await db.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => up.Permission.Name)
            .ToListAsync(ct);
    }

    public async Task<bool> GrantPermissionAsync(Guid userId, string permissionName, CancellationToken ct = default)
    {
        var user = await db.Users.FindAsync([userId], ct);
        if (user is null) return false;

        var permission = await db.Permissions
            .FirstOrDefaultAsync(p => p.Name == permissionName, ct);

        if (permission is null)
        {
            permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = permissionName
            };
            await db.Permissions.AddAsync(permission, ct);
        }

        var exists = await db.UserPermissions
            .AnyAsync(up => up.UserId == userId && up.PermissionId == permission.Id, ct);

        if (exists) return true; // already has it

        var userPermission = new UserPermission
        {
            UserId = userId,
            PermissionId = permission.Id
        };
        await db.UserPermissions.AddAsync(userPermission, ct);

        await db.SaveChangesAsync(ct);
        return true;
    }
    
    public async Task<List<Permission>> GetAllPermissions(CancellationToken ct = default)
    {
        return await db.Permissions.ToListAsync(ct);
    }
}