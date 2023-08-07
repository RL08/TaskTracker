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

namespace TaskTrackerProject.Webapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [AllowAnonymous]
    public class ListController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly TaskTrackerContext _db;

        public ListController(IMapper mapper, TaskTrackerContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        /// <summary>
        /// GET Request /api/list/
        /// Returns a list of all lists with base information.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllLists()
        {
            var lists = _db.Lists
                .Select(a => new
                {
                    a.Guid,
                    a.Id,
                    a.Name,
                    UserGuid = a.User.Guid,
                    a.UserId,
                    UserUsername = a.User.Username,
                    UserEmail = a.User.Email,
                }).ToList();
            return Ok(lists);
        }

        /// <summary>
        /// GET Request /api/list/guid
        /// </summary>
        [HttpGet("{guid:Guid}")]
        public IActionResult GetListsDetail(Guid guid) // from list
        {
            var list = _db.Lists
                .Where(a => a.Guid == guid)
                .Select(a => new
                {
                    a.Guid,
                    a.Id,
                    a.Name,
                    UserGuid = a.User.Guid,
                    a.UserId,
                    UserUsername = a.User.Username,
                    UserEmail = a.User.Email,
                    a.Tasks
                })
                .FirstOrDefault(a => a.Guid == guid);
            if (list is null) { return NotFound(); }
            return Ok(list);
        }

        /// <summary>
        /// POST Request /api/list/addlist with JSON body
        /// Creates a new list in the database. 
        /// </summary>
        /// <param name="listDto"></param>
        /// <returns></returns>
        [HttpPost("addlist")]
        public async Task<IActionResult> AddList(UserListDto listDto)
        {
            var list = _mapper.Map<UserList>(listDto,
                opt => opt.AfterMap((dto, entity) =>
                {
                    entity.User = _db.Users.First(a => a.Guid == listDto.UserGuid);
                }));
            await _db.Lists.AddAsync(list);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return Ok(_mapper.Map<UserList, UserListDto>(list));
        }

        /// <summary>
        /// DELETE Request /api/news/guid with JSON body
        /// Deletes a list in the database.
        /// Use Cascade to delete all child entities when parent entity is deleted
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{guid:Guid}")]
        public async Task<IActionResult> DeleteArticleAsync(Guid guid)
        {
            var lists = _db.Lists.FirstOrDefault(a => a.Guid == guid);
            if (lists is null) { return NotFound(); }
            _db.Lists.Remove(lists);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return NoContent();
        }
    }
}
