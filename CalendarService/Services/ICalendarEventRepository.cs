using CalendarService.Models;

namespace CalendarService.Services;

public interface ICalendarEventRepository
{
    Task<IEnumerable<CalendarEvent?>> GetCalendarEventsAsync(string username);
    Task<CalendarEvent?> GetCalendarEventAsync(int id);
    Task AddCalendarEventAsync(CalendarEvent @event);
    void DeleteCalendarEvent(CalendarEvent @event);
    Task<bool> CalendarEventExistsAsync(int id);
}