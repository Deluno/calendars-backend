using Microsoft.AspNetCore.Identity;

namespace CalendarService.Models;

public class AppUser : IdentityUser<int>
{
    public ICollection<Calendar> UserCalendars { get; set; } = new List<Calendar>();
    public ICollection<CalendarUserInvitation> SharedCalendars { get; set; } = new List<CalendarUserInvitation>();

    public ICollection<CalendarUserSubscription> SubscribedCalendars { get; set; } =
        new List<CalendarUserSubscription>();
}