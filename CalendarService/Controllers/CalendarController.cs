using AutoMapper;
using CalendarService.Entities.Calendar;
using CalendarService.Models;
using CalendarService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CalendarController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CalendarController(
        UnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // GET: api/Calendar
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarDto>>> GetCalendars([FromQuery] bool? subscribed = false)
    {
        if (User.Identity?.Name is null) return Unauthorized();

        IEnumerable<Calendar> calendars;

        if (subscribed == true)
            calendars = await _unitOfWork.CalendarRepository.GetSubscribedCalendarsAsync(User.Identity.Name);
        else
            calendars = await _unitOfWork.CalendarRepository.GetCalendarsAsync(User.Identity.Name);

        return Ok(_mapper.Map<IEnumerable<CalendarDto>>(calendars));
    }

    // GET: api/Calendar/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CalendarDto>> GetCalendar(int id)
    {
        var calendar = await _unitOfWork.CalendarRepository.GetCalendarAsync(id);
        if (calendar == null) return NotFound();

        return Ok(_mapper.Map<CalendarDto>(calendar));
    }

    // POST: api/Calendar
    [HttpPost]
    public async Task<ActionResult<CalendarDto>> PostCalendar(CalendarForCreationDto calendar)
    {
        var owner = await _unitOfWork.UserManager.FindByIdAsync(calendar.OwnerId);
        if (owner == null) return BadRequest($"Owner with id: {calendar.OwnerId} not found");

        var calendarEntity = _mapper.Map<Calendar>(calendar);
        await _unitOfWork.CalendarRepository.AddCalendarAsync(calendarEntity);
        await _unitOfWork.CommitChangesAsync();

        return CreatedAtAction(
            nameof(GetCalendar),
            new { id = calendarEntity.Id },
            _mapper.Map<CalendarDto>(calendarEntity)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> PutCalendar(int id, CalendarForUpdateDto calendar)
    {
        var calendarEntity = await _unitOfWork.CalendarRepository.GetCalendarAsync(id);
        if (calendarEntity == null) return NotFound();

        _mapper.Map(calendar, calendarEntity);
        await _unitOfWork.CommitChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:int}/subscribe")]
    public async Task<ActionResult> Subscribe(int id)
    {
        var calendar = await _unitOfWork.CalendarRepository.GetCalendarAsync(id);
        if (calendar == null) return NotFound();

        var user = await _unitOfWork.UserManager.FindByNameAsync(User.Identity?.Name);
        if (user == null) return Unauthorized();

        if (calendar.OwnerId == user.Id) return BadRequest("You can't subscribe to your own calendar");
        if (calendar.SubscribedUsers.Any(s => s.UserId == user.Id))
            return BadRequest("You are already subscribed to this calendar");

        await _unitOfWork.CalendarRepository.SubscribeToCalendarAsync(id, user.UserName);
        await _unitOfWork.CommitChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:int}/unsubscribe")]
    public async Task<ActionResult> Unsubscribe(int id)
    {
        var calendar = await _unitOfWork.CalendarRepository.GetCalendarAsync(id, true);
        if (calendar == null) return NotFound();

        var user = await _unitOfWork.UserManager.FindByNameAsync(User.Identity?.Name);
        if (user == null) return Unauthorized();

        if (calendar.OwnerId == user.Id) return BadRequest("You can't unsubscribe from your own calendar");
        if (calendar.SubscribedUsers.All(s => s.UserId != user.Id))
            return BadRequest("You are not subscribed to this calendar");

        await _unitOfWork.CalendarRepository.UnsubscribeFromCalendarAsync(id, user.UserName);
        await _unitOfWork.CommitChangesAsync();

        return NoContent();
    }

    // DELETE: api/Calendar/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCalendar(int id)
    {
        var calendar = await _unitOfWork.CalendarRepository.GetCalendarAsync(id);
        if (calendar == null) return NotFound();

        _unitOfWork.CalendarRepository.DeleteCalendar(calendar);
        await _unitOfWork.CommitChangesAsync();

        return NoContent();
    }
}