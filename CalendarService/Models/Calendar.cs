using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarService.Models;

public class Calendar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool IsPublic { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public AppUser Owner { get; set; } = null!;

    public ICollection<CalendarUserInvitation> InvitedUsers { get; set; } = new List<CalendarUserInvitation>();
    public ICollection<CalendarUserSubscription> SubscribedUsers { get; set; } = new List<CalendarUserSubscription>();
    public ICollection<CalendarItem> CalendarItems { get; set; } = new List<CalendarItem>();
}