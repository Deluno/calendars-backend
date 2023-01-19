using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarService.Models;

public class CalendarUserInvitation
{
    [Required]
    public bool CanEdit { get; set; }

    [Required]
    public bool CanView { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;

    public int CalendarId { get; set; }

    [ForeignKey(nameof(CalendarId))]
    public Calendar Calendar { get; set; } = null!;
}