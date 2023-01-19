namespace CalendarService.Entities.Calendar;

public class CalendarDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsPublic { get; set; }
    public int OwnerId { get; set; }
}