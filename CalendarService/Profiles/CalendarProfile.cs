using AutoMapper;
using CalendarService.Entities.Calendar;
using CalendarService.Models;

namespace CalendarService.Profiles;

public class CalendarProfile : Profile
{
    public CalendarProfile()
    {
        CreateMap<Calendar, CalendarDto>();
        CreateMap<CalendarForCreationDto, Calendar>();
        CreateMap<Calendar, CalendarWithOwnerDto>();
        CreateMap<CalendarForUpdateDto, Calendar>();
    }
}