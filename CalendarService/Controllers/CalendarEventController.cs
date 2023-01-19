using AutoMapper;
using CalendarService.Entities.CalendarEvent;
using CalendarService.Models;
using CalendarService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CalendarEventController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public CalendarEventController(
        UnitOfWork unitOfWork,
        IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetCalendarEvents()
    {
        if (User.Identity?.Name is null) return Unauthorized();

        var calendarEvents = await _unitOfWork.CalendarEventRepository
            .GetCalendarEventsAsync(User.Identity.Name);

        var calendarEventModels = _mapper.Map<IEnumerable<CalendarEventDto>>(calendarEvents);
        return Ok(calendarEventModels);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CalendarEventDto>> GetCalendarEvent(int id)
    {
        var calendarEvent = await _unitOfWork.CalendarEventRepository.GetCalendarEventAsync(id);
        if (calendarEvent == null) return NotFound();

        var calendarEventModel = _mapper.Map<CalendarEventDto>(calendarEvent);
        return Ok(calendarEventModel);
    }

    [HttpPost]
    public async Task<IActionResult> PostEvent(CalendarEventForCreationDto @event)
    {
        var calendar = await _unitOfWork.CalendarRepository.GetCalendarAsync(@event.CalendarId);
        if (calendar == null) return NotFound();

        var calendarEventEntity = _mapper.Map<CalendarEvent>(@event);
        await _unitOfWork.CalendarEventRepository.AddCalendarEventAsync(calendarEventEntity);
        await _unitOfWork.CommitChangesAsync();

        return CreatedAtAction(
            nameof(GetCalendarEvent),
            new { id = calendarEventEntity.Id },
            _mapper.Map<CalendarEventDto>(calendarEventEntity)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutEvent(int id, CalendarEventForUpdateDto @event)
    {
        var calendarEvent = await _unitOfWork.CalendarEventRepository.GetCalendarEventAsync(id);
        if (calendarEvent == null) return NotFound();

        _mapper.Map(@event, calendarEvent);
        await _unitOfWork.CommitChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        var calendarEvent = await _unitOfWork.CalendarEventRepository.GetCalendarEventAsync(id);
        if (calendarEvent == null) return BadRequest();

        _unitOfWork.CalendarEventRepository.DeleteCalendarEvent(calendarEvent);
        await _unitOfWork.CommitChangesAsync();

        return NoContent();
    }
}