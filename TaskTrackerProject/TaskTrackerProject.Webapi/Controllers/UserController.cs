using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using TaskTrackerProject.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TaskTrackerProject.Application.Dto;
using TaskTrackerProject.Application.Model;
using System.Net;

namespace TaskTrackerProject.Webapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        public record CredentialsDto(string username, string password, string email);

        private readonly IConfiguration _config;
        private readonly bool _isDevelopment;
        private readonly TaskTrackerContext _db;

        public UserController(IConfiguration config, IHostEnvironment _env, TaskTrackerContext db)
        {
            _config = config;
            _isDevelopment = _env.IsDevelopment();
            _db = db;
        }

        /// <summary>
        /// POST /api/user/login
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] CredentialsDto credentials)
        {
            var lifetime = TimeSpan.FromHours(3);
            var searchuser = _config["Searchuser"];
            var searchpass = _config["Searchpass"];
            var secret = Convert.FromBase64String(_config["Secret"]);
            var localAdmins = _config["LocalAdmins"].Split(",");

            using var service = _isDevelopment && !string.IsNullOrEmpty(searchuser)
                ? AdService.Login(searchuser, searchpass, credentials.username)
                : AdService.Login(credentials.username, credentials.password);
            var currentUser = service.CurrentUser;
            if (currentUser is null) { return Unauthorized(); }
            var role = localAdmins.Contains(currentUser.Cn)
                            ? AdUserRole.Management.ToString() : currentUser.Role.ToString();
            var group = (currentUser.Role, currentUser.Classes.Length > 0) switch
            {
                (AdUserRole.Pupil, true) => currentUser.Classes[0],
                (AdUserRole.Pupil, false) => "Unknown class",
                (AdUserRole.Teacher, _) => AdUserRole.Teacher.ToString(),
                (AdUserRole.Management, _) => AdUserRole.Teacher.ToString(),
                (_, _) => AdUserRole.Administration.ToString()
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Payload for our JWT.
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, currentUser.Cn),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim("Group", group)
                }),
                Expires = DateTime.UtcNow + lifetime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                Username = currentUser.Cn,
                Role = role,
                Group = group,
                Token = tokenHandler.WriteToken(token)
            });
        }

        /// <summary>
        /// POST /api/user/register
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] CredentialsDto credentials)
        {
            var user = _db.Users.FirstOrDefault(a => a.Username == credentials.username);
            if (user is null)
            {
                user = new User(credentials.username, credentials.password, credentials.email, Userrole.User);
                _db.Users.Add(user);
                try { _db.SaveChanges(); }
                catch (DbUpdateException) { return BadRequest(); }
            }
            else { return Ok("User is in database"); }
            if (!user.CheckPassword(credentials.password)) { return Unauthorized(); }
            return Ok(user);
        }

        /// <summary>
        /// GET /api/user/me
        /// Gets information about the current (authenticated) user.
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetUserdata()
        {
            // No username is set in HttpContext? Should never occur because we added the
            // Authorize annotation. But the properties are nullable, so we have to check.
            var username = HttpContext?.User.Identity?.Name;
            if (username is null) { return Unauthorized(); }

            // Valid token, but no user match in the database (maybe deleted by an admin).
            var user = _db.Users.FirstOrDefault(a => a.Username == username);
            if (user is null) { return Unauthorized(); }
            return Ok(new
            {
                user.Username,
                user.Email,
                user.Role
            });
        }

        /// <summary>
        /// GET /api/user/all
        /// List all users.
        /// Only for users which has the role admin in the claim of the JWT.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var user = _db.Users
                .Select(u => new
                {
                    u.Username,
                    u.Email,
                    u.Role
                })
                .ToList();
            if (user is null) { return BadRequest(); }
            return Ok(user);
        }
    }
}
