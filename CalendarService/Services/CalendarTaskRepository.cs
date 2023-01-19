using CalendarService.DbContexts;
using CalendarService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Services;

public class CalendarTaskRepository : ICalendarTaskRepository
{
    private readonly CalendarServiceContext _context;

    public CalendarTaskRepository(CalendarServiceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CalendarTask>> GetCalendarTasksAsync(string username)
    {
        var user = await _context.AppUsers
            .Include(u => u.UserCalendars)
            .Include(u => u.SubscribedCalendars)
            .FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null) return ArraySegment<CalendarTask>.Empty;

        var tasks = new List<CalendarTask>();
        foreach (var calendar in user.UserCalendars)
            tasks.AddRange(await _context.CalendarTasks
                .Include(t => t.Calendar)
                .Where(t => t.CalendarId == calendar.Id)
                .ToListAsync());

        foreach (var calendar in user.SubscribedCalendars)
            tasks.AddRange(await _context.CalendarTasks
                .Include(t => t.Calendar)
                .Where(t => t.CalendarId == calendar.CalendarId)
                .ToListAsync());

        return tasks;
    }

    public async Task<CalendarTask?> GetCalendarTaskAsync(int id)
    {
        return await _context.CalendarTasks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddCalendarTaskAsync(CalendarTask task)
    {
        await _context.CalendarTasks.AddAsync(task);
    }

    public void DeleteCalendarTask(CalendarTask task)
    {
        _context.CalendarTasks.Remove(task);
    }

    public async Task<bool> CalendarTaskExistsAsync(int id)
    {
        return await _context.CalendarTasks.AnyAsync(x => x.Id == id);
    }
}