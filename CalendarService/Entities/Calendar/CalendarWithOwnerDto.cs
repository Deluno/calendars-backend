using CalendarService.Models;

namespace CalendarService.Entities.Calendar;

public class CalendarWithOwnerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsPublic { get; set; }
    public AppUserDto? Owner { get; set; }
}