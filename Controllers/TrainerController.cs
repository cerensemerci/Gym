using Basics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basics.Controllers
{
    public class TrainerController : Controller
    {
        private readonly RepositoryContext _context;

        public TrainerController(RepositoryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var trainers = _context.Trainers.Include(t => t.Services).ToList();
            return View(trainers);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateTrainer()
        {
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateTrainer(Trainer model, int[] selectedServices)
        {
            if (ModelState.IsValid)
            {
                if (selectedServices != null)
                {
                    foreach (var serviceId in selectedServices)
                    {
                        var service = _context.Services.Find(serviceId);
                        if (service != null) model.Services.Add(service);
                    }
                }
                _context.Trainers.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Trainers", "Admin");
            }
            ViewBag.Services = _context.Services.ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditTrainer(int id)
        {
            var trainer = _context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.TrainerID == id);
            if (trainer == null) return NotFound();
            ViewBag.Services = _context.Services.ToList();
            return View(trainer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditTrainer(int id, Trainer model, int[] selectedServices)
        {
            if (id != model.TrainerID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var trainerToUpdate = _context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.TrainerID == id);
                    if (trainerToUpdate == null) return NotFound();

                    trainerToUpdate.FirstName = model.FirstName;
                    trainerToUpdate.LastName = model.LastName;
                    trainerToUpdate.Specialty = model.Specialty;

                    trainerToUpdate.Services.Clear();
                    if (selectedServices != null)
                    {
                        foreach (var serviceId in selectedServices)
                        {
                            var service = _context.Services.Find(serviceId);
                            if (service != null) trainerToUpdate.Services.Add(service);
                        }
                    }

                    _context.Update(trainerToUpdate);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Trainers.Any(e => e.TrainerID == model.TrainerID)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Trainers", "Admin");
            }
            ViewBag.Services = _context.Services.ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Trainers", "Admin");
        }

        public JsonResult GetTrainerAvailabilities(int trainerId)
        {
            var isAdmin = User.IsInRole("Admin");

            var availabilities = _context.TrainerAvailabilities
                .Include(a => a.Employee)
                .Where(a => a.TrainerID == trainerId)
                .Select(a => new
                {
                    id = a.AvailabilityID,
                    title = a.Status == "Available" ? "Müsait" : (a.Status == "Booked" ? (isAdmin && a.Employee != null ? $"{a.Employee.FirstName} {a.Employee.LastName}" : "Dolu") : "Kapalı"),
                    start = a.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = a.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = a.Status == "Available" ? "#28a745" : (a.Status == "Booked" ? "#dc3545" : "#6c757d"),
                    extendedProps = new { status = a.Status }
                })
                .ToList();

            return Json(availabilities);
        }

        // Details action for public view if needed
        public IActionResult Details(int id)
        {
             var trainer = _context.Trainers.Find(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }
        public IActionResult GetTrainerServices(int id)
        {
            var trainer = _context.Trainers.Include(t => t.Services).FirstOrDefault(t => t.TrainerID == id);
            if (trainer == null) return NotFound();
            return Json(trainer.Services.Select(s => new { id = s.Id, name = s.Name, price = s.Price }));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BookAppointment([FromBody] BookingRequest request)
        {
            var availability = await _context.TrainerAvailabilities.FindAsync(request.AvailabilityId);
            if (availability == null || availability.Status != "Available")
                return Json(new { success = false, message = "Bu saat dilimi artık müsait değil." });

            var service = await _context.Services.FindAsync(request.ServiceId);
            if (service == null)
                return Json(new { success = false, message = "Geçersiz hizmet seçimi." });

            var userEmail = User.Identity?.Name;
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == userEmail);
            
            if (employee == null)
            {
                // If no profile exists, we might need them to fill the 'Apply' form first
                return Json(new { success = false, message = "Randevu alabilmek için önce 'Başvuru' sayfasından profilinizi oluşturmalısınız." });
            }

            availability.Status = "Booked";
            availability.EmployeeId = employee.Id;
            availability.ServiceId = service.Id;
            availability.Price = service.Price;
            availability.Duration = service.Duration;

            _context.Update(availability);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        public class BookingRequest
        {
            public int AvailabilityId { get; set; }
            public int ServiceId { get; set; }
        }
    }
}
