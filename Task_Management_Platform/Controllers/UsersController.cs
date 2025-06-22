using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Management_Platform.Data;
using Task_Management_Platform.Models;
using Task_Management_Platform.Models.Dto;

namespace Task_Management_Platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var users = await _context.Users
                .Where(u => u.TenantId == tenantId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role.ToString()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var user = await _context.Users
                .Where(u => u.Id == id && u.TenantId == tenantId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role.ToString()
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto dto)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            if (!Enum.TryParse<UserRole>(dto.Role, out var role))
                return BadRequest("Invalid role.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = role,
                TenantId = tenantId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString()
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto dto)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId);
            if (user == null)
                return NotFound();

            if (!Enum.TryParse<UserRole>(dto.Role, out var role))
                return BadRequest("Invalid role.");

            user.Username = dto.Username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Role = role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.TenantId == tenantId);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}