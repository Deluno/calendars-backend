using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarService.Models;

public class CalendarItemFile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string FileName { get; set; } = null!;

    [Required]
    public string FileExtension { get; set; } = null!;

    [Required]
    public string FileSize { get; set; } = null!;

    [Required]
    public string FileUrl { get; set; } = null!;

    public int CalendarItemId { get; set; }

    [ForeignKey(nameof(CalendarItemId))]
    public CalendarItem CalendarItem { get; set; } = null!;
}