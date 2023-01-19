using System.ComponentModel.DataAnnotations;

namespace CalendarService.Entities.Calendar;

public class CalendarForCreationDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public bool IsPublic { get; set; }

    [Required]
    public string OwnerId { get; set; } = null!;
}