using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using TaskTrackerProject.Application.Infrastructure;
using System.Linq;
using AutoMapper;
using TaskTrackerProject.Application.Dto;
using TaskTrackerProject.Application.Model;
using Bogus.Bson;
using System.Xml.Linq;

namespace TaskTrackerProject.Webapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [AllowAnonymous]
    public class TaskController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TaskTrackerContext _db;

        public TaskController(IMapper mapper, TaskTrackerContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        /// <summary>
        /// GET Request /api/task
        /// Returns a list of all tasks with base information.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _db.Tasks
                .Select(a => new
                {
                    a.Guid,
                    a.Id,
                    a.Name,
                    a.Status,
                    a.Priority,
                    a.IsFavorite,
                    a.Date,
                    ListGuid = a.List.Guid,
                    a.ListId,
                    ListName = a.List.Name
                }).ToListAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// GET Request /api/task/guid
        /// Returns a task with base information.
        /// </summary>
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> GetTaskDetailAsync(Guid guid) // from Task
        {
            var task = await _db.Tasks
                .Where(a => a.Guid == guid)
                .Select(a => new
                {
                    a.Guid,
                    a.Id,
                    a.Name,
                    a.Status,
                    a.Priority,
                    a.IsFavorite,
                    a.Date,
                    ListGuid = a.List.Guid,
                    a.ListId,
                    ListName = a.List.Name
                })
                .FirstOrDefaultAsync(a => a.Guid == guid);
            if (task is null) { return NotFound(); }
            return Ok(task);
        }

        /// <summary>
        /// POST Request /api/Task/addTask with JSON body
        /// Creates a new task in the database. 
        /// </summary>
        /// <param name="taskDto"></param>
        /// <returns></returns>
        [HttpPost("addtask")]
        public async Task<IActionResult> AddTask(ListTaskDto taskDto)
        {
            var task = _mapper.Map<ListTask>(taskDto,
                opt => opt.AfterMap((dto, entity) =>
                {
                    entity.List = _db.Lists.First(a => a.Guid == taskDto.ListGuid);
                }));
            await _db.Tasks.AddAsync(task);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return Ok(_mapper.Map<ListTask, ListTaskDto>(task));
        }

        /// <summary>
        /// DELETE Request /api/news/guid with JSON body
        /// Deletes a task in the database.
        /// Use Cascade to delete all child entities when parent entity is deleted
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete("{guid:Guid}")]
        public async Task<IActionResult> DeleteTask(Guid guid)
        {
            var tasks = await _db.Tasks.FirstOrDefaultAsync(a => a.Guid == guid);
            if (tasks is null) { return NotFound(); }
            _db.Tasks.Remove(tasks);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return NoContent();
        }

        /// <summary>
        /// PUT Request /api/news/(guid) with JSON body
        /// Updates a task in the database. 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="taskDto"></param>
        /// <returns></returns>
        [HttpPut("{guid:Guid}")]
        public async Task<IActionResult> EditTask(Guid guid, ListTaskDto taskDto)
        {
            if (guid != taskDto.Guid) { return BadRequest(); }
            var task = await _db.Tasks.FirstOrDefaultAsync(a => a.Guid == guid);
            if (task is null) { return NotFound(); }
            _mapper.Map(taskDto, task,
                opt => opt.AfterMap((dto, entity) =>
                {
                    entity.List = _db.Lists.First(a => a.Guid == taskDto.ListGuid);
                }));
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return NoContent();
        }
    }
}

