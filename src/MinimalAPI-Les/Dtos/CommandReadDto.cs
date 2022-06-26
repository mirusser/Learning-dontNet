using System.ComponentModel.DataAnnotations;

namespace MinimalAPI_Les.Dtos;

public class CommandReadDto
{
    public int Id { get; set; }

    public string? HowTo { get; set; }

    public string? CommandLine { get; set; }
}