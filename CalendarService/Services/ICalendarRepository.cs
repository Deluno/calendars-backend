using CalendarService.Models;

namespace CalendarService.Services;

public interface ICalendarRepository
{
    Task<IEnumerable<Calendar>> GetCalendarsAsync(string? username = null);
    Task<IEnumerable<Calendar>> GetSubscribedCalendarsAsync(string? username);
    Task<Calendar?> GetCalendarAsync(int id, bool includeSubscriptions = false);
    Task AddCalendarAsync(Calendar calendar);
    Task SubscribeToCalendarAsync(int id, string username);
    Task UnsubscribeFromCalendarAsync(int id, string username);
    void DeleteCalendar(Calendar calendar);
    Task<bool> CalendarExistsAsync(int id);
}