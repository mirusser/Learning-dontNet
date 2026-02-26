using System.Reflection;
using RandomQuotesApi.Models;
using RandomQuotesApi.Models.Auth;
using RandomQuotesApi.Repos;
using Path = System.IO.Path;

namespace RandomQuotesApi.Data;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public static class DataSeeder
{
    public static async Task SeedPermissionsAsync(
        AppDbContext db,
        IWebHostEnvironment env,
        CancellationToken ct = default)
    {
        var allPermissionNames = GetAllPermissionNames().ToList();
        if (allPermissionNames.Count == 0)
            return;

        var existingNames = await db.Permissions
            .Where(p => allPermissionNames.Contains(p.Name))
            .Select(p => p.Name)
            .ToListAsync(ct);

        var existingSet = existingNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missingNames = allPermissionNames
            .Where(name => !existingSet.Contains(name))
            .ToList();

        if (missingNames.Count == 0)
            return;

        var newPermissions = missingNames
            .Select(name => new Permission
            {
                Name = name,
            })
            .ToList();

        await db.Permissions.AddRangeAsync(newPermissions, ct);
        await db.SaveChangesAsync(ct);

        Console.WriteLine($"Seeded {newPermissions.Count} permissions into the database.");
    }
    
    private static IEnumerable<string> GetAllPermissionNames()
    {
        return GetPermissionNamesFromType(typeof(Permissions))
            .Where(name => name != Permissions.ClaimType) // exclude claim type if needed
            .Distinct();
    }

    private static IEnumerable<string> GetPermissionNamesFromType(Type type)
    {
        const BindingFlags flags =
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        // Public const string fields
        foreach (var field in type.GetFields(flags))
        {
            if (field.FieldType == typeof(string) &&
                field.IsLiteral && !field.IsInitOnly)
            {
                var value = (string?)field.GetRawConstantValue();
                if (!string.IsNullOrWhiteSpace(value))
                    yield return value!;
            }
        }

        // Recurse into nested types (e.g., Permissions.Favorites)
        foreach (var nested in type.GetNestedTypes(flags))
        {
            foreach (var name in GetPermissionNamesFromType(nested))
            {
                yield return name;
            }
        }
    }

    public static async Task SeedQuotesAsync(
        AppDbContext db,
        IWebHostEnvironment env,
        CancellationToken ct = default)
    {
        // If there are already quotes, do nothing
        if (await db.Quotes.AnyAsync(ct))
        {
            return;
        }

        var filePath = Path.Combine(env.ContentRootPath, "Data", "Quotes.json");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Seed file not found: {filePath}");
            return;
        }

        var json = await File.ReadAllTextAsync(filePath, ct);

        var seedQuotes = JsonSerializer.Deserialize<List<QuoteSeedDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (seedQuotes is null || seedQuotes.Count == 0)
        {
            Console.WriteLine("Seed file is empty or could not be deserialized.");
            return;
        }

        var entities = seedQuotes.Select(q => new Quote
        {
            Id = Guid.NewGuid(),
            Text = q.Text,
            Author = q.Author,
            Origin = q.Origin,
            Categories = q.Categories ?? []
        });

        await db.Quotes.AddRangeAsync(entities, ct);
        await db.SaveChangesAsync(ct);

        Console.WriteLine($"Seeded {seedQuotes.Count} quotes into the database.");
    }
}