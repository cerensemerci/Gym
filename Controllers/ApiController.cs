using Basics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basics.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public ApiController(RepositoryContext context)
        {
            _context = context;
        }

        // 1. Tüm antrenörleri listeleme
        [HttpGet("trainers")]
        public async Task<IActionResult> GetTrainers()
        {
            var trainers = await _context.Trainers
                .Include(t => t.Services)
                .Select(t => new {
                    t.TrainerID,
                    t.FullName,
                    t.Specialty,
                    Services = t.Services.Select(s => s.Name)
                })
                .ToListAsync();
            return Ok(trainers);
        }

        // 2. Belirli bir tarihte uygun antrenörleri getirme (LINQ Filtreleme)
        [HttpGet("trainers/available")]
        public async Task<IActionResult> GetAvailableTrainers([FromQuery] DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var availableTrainers = await _context.TrainerAvailabilities
                .Where(a => a.StartTime >= startOfDay && a.StartTime < endOfDay && a.Status == "Available")
                .Include(a => a.Trainer)
                .Select(a => new {
                    a.Trainer.FullName,
                    a.Trainer.Specialty,
                    a.StartTime,
                    a.EndTime
                })
                .Distinct()
                .ToListAsync();

            return Ok(availableTrainers);
        }

        // 3. Üye randevularını getirme
        [HttpGet("appointments/member/{email}")]
        public async Task<IActionResult> GetMemberAppointments(string email)
        {
            var appointments = await _context.TrainerAvailabilities
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .Where(a => a.Employee.Email == email && a.Status != "Available")
                .Select(a => new {
                    Trainer = a.Trainer.FullName,
                    Service = a.Service.Name,
                    Date = a.StartTime,
                    Status = a.Status,
                    Price = a.Price
                })
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
