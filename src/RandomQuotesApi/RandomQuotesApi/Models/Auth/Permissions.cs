using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace RandomQuotesApi.Models.Auth;

public static class Permissions
{
    public const string ClaimType = "permission";

    public static class Favorites
    {
        public const string View = "CanViewFavorites";
        public const string Modify = "CanModifyFavorites";
    }
    
    public static class Users
    {
        public const string View = "CanViewUsers";
        public const string Modify = "CanModifyUsers";
    }
    
    public static IReadOnlyList<string> GetAllPermissionValues()
    {
        var topLevelFields = typeof(Permissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f is { IsLiteral: true, IsInitOnly: false } && f.FieldType == typeof(string));

        var result = topLevelFields
            .Select(field => (string?)field.GetRawConstantValue())
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToList();

        var nestedTypes = typeof(Permissions).GetNestedTypes(BindingFlags.Public);
        foreach (var nested in nestedTypes)
        {
            var nestedFields = nested
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f is { IsLiteral: true, IsInitOnly: false } && f.FieldType == typeof(string));

            result.AddRange(nestedFields
                .Select(field => (string?)field.GetRawConstantValue())
                .Where(value => !string.IsNullOrWhiteSpace(value)));
        }

        return result;
    }
}

public static class AuthorizationOptionsExtensions
{
    public static void AddPermissionPolicies(this AuthorizationOptions options)
    {
        var permissions = Permissions.GetAllPermissionValues();

        foreach (var permission in permissions)
        {
            options.AddPolicy(permission, policy =>
                policy.RequireClaim(Permissions.ClaimType, permission));
        }
    }
}
