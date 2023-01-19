using AutoMapper;
using CalendarService.Entities.CalendarEvent;
using CalendarService.Models;

namespace CalendarService.Profiles;

public class CalendarEventProfile : Profile
{
    public CalendarEventProfile()
    {
        CreateMap<CalendarEventForCreationDto, CalendarEvent>();
        CreateMap<CalendarEvent, CalendarEventDto>();
        CreateMap<CalendarEventForUpdateDto, CalendarEvent>();
    }
}