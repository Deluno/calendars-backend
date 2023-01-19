using AutoMapper;
using CalendarService.Entities.CalendarTask;
using CalendarService.Models;
using CalendarService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CalendarTaskController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CalendarTaskController(
        UnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarTaskDto>>> GetCalendarTasks()
    {
        if (User.Identity?.Name is null) return Unauthorized();

        var calendarTasks = await _unitOfWork.CalendarTaskRepository
            .GetCalendarTasksAsync(User.Identity.Name);
        return Ok(_mapper.Map<IEnumerable<CalendarTaskDto>>(calendarTasks));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CalendarTaskDto>> GetCalendarTask(int id)
    {
        var calendarTask = await _unitOfWork.CalendarTaskRepository.GetCalendarTaskAsync(id);
        if (calendarTask == null) return NotFound();
        return Ok(_mapper.Map<CalendarTaskDto>(calendarTask));
    }

    [HttpPost]
    public async Task<ActionResult<CalendarTaskDto>> CreateCalendarTask(
        CalendarTaskForCreationDto task)
    {
        if (!await _unitOfWork.CalendarRepository.CalendarExistsAsync(task.CalendarId)) return NotFound();

        var calendarTaskEntity = _mapper.Map<CalendarTask>(task);
        await _unitOfWork.CalendarTaskRepository.AddCalendarTaskAsync(calendarTaskEntity);
        await _unitOfWork.CommitChangesAsync();
        return CreatedAtAction(
            nameof(GetCalendarTask),
            new { id = calendarTaskEntity.Id },
            _mapper.Map<CalendarTaskDto>(calendarTaskEntity)
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CalendarTaskDto>> UpdateCalendarTask(
        int id,
        CalendarTaskForUpdateDto task)
    {
        var calendarTaskEntity = await _unitOfWork.CalendarTaskRepository.GetCalendarTaskAsync(id);
        if (calendarTaskEntity == null) return NotFound();

        _mapper.Map(task, calendarTaskEntity);
        await _unitOfWork.CommitChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCalendarTask(int id)
    {
        var calendarTask = await _unitOfWork.CalendarTaskRepository.GetCalendarTaskAsync(id);
        if (calendarTask == null) return NotFound();
        _unitOfWork.CalendarTaskRepository.DeleteCalendarTask(calendarTask);
        await _unitOfWork.CommitChangesAsync();
        return NoContent();
    }
}