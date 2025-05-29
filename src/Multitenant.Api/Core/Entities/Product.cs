using System.ComponentModel.DataAnnotations;
using Core.Contracts;

namespace Core.Entities;

public class Product(
    string name,
    string description,
    int rate) : BaseEntity, ITenant
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public int Rate { get; private set; } = rate;

    public string? TenantId { get; set; }
}