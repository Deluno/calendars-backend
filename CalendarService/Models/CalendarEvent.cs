using System.ComponentModel.DataAnnotations;

namespace CalendarService.Models;

public class CalendarEvent : CalendarItem
{
    public string? Location { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}