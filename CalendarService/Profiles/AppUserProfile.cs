using AutoMapper;
using CalendarService.Entities.User;
using CalendarService.Models;

namespace CalendarService.Profiles;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUser, AppUserDto>();
        CreateMap<AppUser, AppUserWithCalendarsDto>();
    }
}