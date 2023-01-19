using CalendarService.DbContexts;
using CalendarService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Services;

public class CalendarEventRepository : ICalendarEventRepository
{
    private readonly CalendarServiceContext _context;

    public CalendarEventRepository(CalendarServiceContext context)
    {
        _context = context;
    }

    public async Task<CalendarEvent?> GetCalendarEventAsync(int id)
    {
        return await _context.CalendarEvents.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddCalendarEventAsync(CalendarEvent @event)
    {
        await _context.CalendarEvents.AddAsync(@event);
    }

    public void DeleteCalendarEvent(CalendarEvent @event)
    {
        _context.CalendarEvents.Remove(@event);
    }

    public async Task<bool> CalendarEventExistsAsync(int id)
    {
        return await _context.CalendarEvents.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CalendarEvent?>> GetCalendarEventsAsync(string username)
    {
        var user = await _context.AppUsers
            .Include(u => u.UserCalendars)
            .Include(u => u.SubscribedCalendars)
            .FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null) return ArraySegment<CalendarEvent?>.Empty;

        var events = new List<CalendarEvent?>();
        foreach (var calendar in user.UserCalendars)
            events.AddRange(await _context.CalendarEvents
                .Where(e => e.CalendarId == calendar.Id)
                .ToListAsync());

        foreach (var calendar in user.SubscribedCalendars)
            events.AddRange(await _context.CalendarEvents
                .Where(e => e.CalendarId == calendar.CalendarId)
                .ToListAsync());

        return events;
    }
}