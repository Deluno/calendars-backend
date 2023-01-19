namespace CalendarService.Entities.CalendarEvent;

public class CalendarEventDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Location { get; set; }
    public int CalendarId { get; set; }
}