using System.ComponentModel.DataAnnotations;

namespace CalendarService.Entities.Calendar;

public class CalendarForUpdateDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public bool IsPublic { get; set; }
}