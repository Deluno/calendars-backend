using CalendarService.Entities.Calendar;
using CalendarService.Models;

namespace CalendarService.Entities.User;

public class AppUserWithCalendarsDto : AppUserDto
{
    public IEnumerable<CalendarDto> UserCalendars { get; set; } = new List<CalendarDto>();
}