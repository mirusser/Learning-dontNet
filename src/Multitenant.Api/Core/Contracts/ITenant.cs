using System.ComponentModel.DataAnnotations;

namespace Core.Contracts;

public interface ITenant
{
    public string? TenantId { get; set; }
}