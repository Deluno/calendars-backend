using System.ComponentModel.DataAnnotations;

namespace CalendarService.Entities.Authentication;

public class LoginRequestDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}