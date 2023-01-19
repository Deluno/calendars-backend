using CalendarService.DbContexts;
using CalendarService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Services;

public class CalendarRepository : ICalendarRepository
{
    private readonly CalendarServiceContext _context;

    public CalendarRepository(CalendarServiceContext context)
    {
        _context = context;
    }

    public async Task<Calendar?> GetCalendarAsync(int id, bool includeSubscriptions = false)
    {
        var query = _context.Calendars.Where(c => c.Id == id);

        if (includeSubscriptions)
            query = query
                .Include(c => c.SubscribedUsers);

        return await query.FirstOrDefaultAsync();
    }

    public async Task AddCalendarAsync(Calendar calendar)
    {
        await _context.Calendars.AddAsync(calendar);
    }

    public void DeleteCalendar(Calendar calendar)
    {
        _context.Calendars.Remove(calendar);
    }

    public async Task<bool> CalendarExistsAsync(int id)
    {
        return await _context.Calendars.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Calendar>> GetCalendarsAsync(string? username)
    {
        var calendars = _context.Calendars.AsQueryable<Calendar>();

        if (!string.IsNullOrEmpty(username)) calendars = calendars.Where(c => c.Owner.UserName == username);

        return await calendars.ToListAsync();
    }

    public async Task<IEnumerable<Calendar>> GetSubscribedCalendarsAsync(string? username)
    {
        var calendars = _context.Calendars.AsQueryable<Calendar>();

        calendars = calendars.Where(c => c.SubscribedUsers.Any(s => s.User.UserName == username));

        return await calendars.ToListAsync();
    }

    public async Task SubscribeToCalendarAsync(int id, string username)
    {
        var calendar = await _context.Calendars.FirstOrDefaultAsync(c => c.Id == id);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if (calendar == null || user == null) return;

        await _context.Subscriptions.AddAsync(new CalendarUserSubscription
        {
            Calendar = calendar,
            User = user
        });
    }

    public async Task UnsubscribeFromCalendarAsync(int id, string username)
    {
        var calendar = await _context.Calendars.FirstOrDefaultAsync(c => c.Id == id);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if (calendar == null || user == null) return;

        var subscription =
            await _context.Subscriptions.FirstOrDefaultAsync(s => s.Calendar == calendar && s.User == user);

        if (subscription == null) return;

        _context.Subscriptions.Remove(subscription);
    }
}