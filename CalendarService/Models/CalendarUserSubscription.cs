using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarService.Models;

public class CalendarUserSubscription
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;

    public int CalendarId { get; set; }

    [ForeignKey(nameof(CalendarId))]
    public Calendar Calendar { get; set; } = null!;
}