using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarService.Models;

public abstract class CalendarItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    public int CalendarId { get; set; }

    [ForeignKey(nameof(CalendarId))]
    public Calendar Calendar { get; set; } = null!;
}