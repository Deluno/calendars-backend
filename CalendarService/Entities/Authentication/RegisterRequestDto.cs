using System.ComponentModel.DataAnnotations;

namespace CalendarService.Entities.Authentication;

public class RegisterRequestDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}