using CalendarService.Models;

namespace CalendarService.Services;

public interface ICalendarTaskRepository
{
    Task<IEnumerable<CalendarTask>> GetCalendarTasksAsync(string username);
    Task<CalendarTask?> GetCalendarTaskAsync(int id);
    Task AddCalendarTaskAsync(CalendarTask task);
    void DeleteCalendarTask(CalendarTask task);
    Task<bool> CalendarTaskExistsAsync(int id);
}