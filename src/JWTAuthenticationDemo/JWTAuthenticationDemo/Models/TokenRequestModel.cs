using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationDemo.Models;

public class TokenRequestModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}