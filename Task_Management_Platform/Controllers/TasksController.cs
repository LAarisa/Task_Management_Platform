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
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(
           [FromQuery] string? status,
           [FromQuery] string? assignedTo,
           [FromQuery] string? search)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var query = _context.TaskItems
                .Where(t => t.TenantId == tenantId)
                .Include(t => t.AssignedToUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<Models.TaskStatus>(status, true, out var statusEnum))
            {
                query = query.Where(t => t.Status == statusEnum);
            }

            if (!string.IsNullOrWhiteSpace(assignedTo))
            {
                query = query.Where(t => t.AssignedToUser.Username.Contains(assignedTo));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            var tasks = await query
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    AssignedToUsername = t.AssignedToUser.Username,
                    Status = t.Status.ToString()
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            if (!Enum.TryParse<Models.TaskStatus>(dto.Status, out var statusEnum))
            {
                return BadRequest("Invalid task status.");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = statusEnum,
                AssignedToUserId = dto.AssignedToUserId,
                TenantId = tenantId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateDto dto)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == tenantId);

            if (task == null)
                return NotFound();

            if (!Enum.TryParse<Models.TaskStatus>(dto.Status, out var statusEnum))
                return BadRequest("Invalid status.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = statusEnum;
            task.AssignedToUserId = dto.AssignedToUserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            int tenantId = int.Parse(User.FindFirst("TenantId")?.Value ?? "0");

            var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.TenantId == tenantId);
            if (task == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
