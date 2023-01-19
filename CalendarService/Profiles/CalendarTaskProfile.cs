using AutoMapper;
using CalendarService.Entities.CalendarTask;
using CalendarService.Models;

namespace CalendarService.Profiles;

public class CalendarTaskProfile : Profile
{
    public CalendarTaskProfile()
    {
        CreateMap<CalendarTaskForCreationDto, CalendarTask>();
        CreateMap<CalendarTask, CalendarTaskDto>();
        CreateMap<CalendarTaskForUpdateDto, CalendarTask>();
        CreateMap<CalendarTask, CalendarTaskForUpdateDto>();
    }
}