using System.ComponentModel.DataAnnotations;

namespace CalendarService.Entities.CalendarEvent;

public class CalendarEventForUpdateDto
{
    [Required]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? Location { get; set; }

    public int CalendarId { get; set; }
}