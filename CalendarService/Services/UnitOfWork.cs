using CalendarService.DbContexts;
using CalendarService.Models;
using Microsoft.AspNetCore.Identity;

namespace CalendarService.Services;

public class UnitOfWork
{
    private readonly CalendarServiceContext _context;

    public UnitOfWork(
        CalendarServiceContext context,
        IConfiguration configuration,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public IConfiguration Configuration { get; }
    public UserManager<AppUser> UserManager { get; }
    public SignInManager<AppUser> SignInManager { get; }
    public RoleManager<AppRole> RoleManager { get; }
    public ICalendarRepository CalendarRepository => new CalendarRepository(_context);
    public ICalendarEventRepository CalendarEventRepository => new CalendarEventRepository(_context);
    public ICalendarTaskRepository CalendarTaskRepository => new CalendarTaskRepository(_context);

    public async Task CommitChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}