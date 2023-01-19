namespace CalendarService.Entities.CalendarTask;

public class CalendarTaskDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public int CalendarId { get; set; }
}