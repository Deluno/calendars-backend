using System.ComponentModel.DataAnnotations;

namespace CalendarService.Entities.CalendarTask;

public class CalendarTaskForUpdateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public int CalendarId { get; set; }
}