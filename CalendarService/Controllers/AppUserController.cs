using System.Collections;
using AutoMapper;
using CalendarService.Entities.User;
using CalendarService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppUserController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppUserController(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable>> GetUsers(
        [FromQuery] string? username,
        [FromQuery] bool includeCalendars = false,
        [FromQuery] bool includeIdentity = false)
    {
        var usersQuery = _unitOfWork.UserManager.Users;

        if (!string.IsNullOrEmpty(username))
            usersQuery = usersQuery.Where(u => u.UserName.Contains(username));

        if (includeIdentity && User.Identity is not null && User.IsInRole("Admin"))
            usersQuery = usersQuery.Where(u => u.UserName == User.Identity.Name);

        if (includeCalendars)
            usersQuery = User.IsInRole("Admin")
                ? usersQuery.Include(user => user.UserCalendars)
                : usersQuery.Include(user => user.UserCalendars.Where(c => c.IsPublic));

        // Remove user if they don't have any public calendars and they're not an admin
        if (includeCalendars && !User.IsInRole("Admin"))
            usersQuery = usersQuery.Where(user => user.UserCalendars.Any(c => c.IsPublic));

        var users = await usersQuery.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<AppUserWithCalendarsDto>>(users));
    }
}