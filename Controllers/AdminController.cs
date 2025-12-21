using Basics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basics.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly RepositoryContext _context;

        public AdminController(UserManager<IdentityUser> userManager, IPasswordHasher<IdentityUser> passwordHasher, RepositoryContext context)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Trainers()
        {
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }

        // --- Gym Management ---
        public IActionResult Gyms()
        {
            var gyms = _context.Gyms.ToList();
            return View(gyms);
        }

        public IActionResult CreateGym()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGym(Gym model)
        {
            if (ModelState.IsValid)
            {
                _context.Gyms.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Gyms");
            }
            return View(model);
        }

        public IActionResult EditGym(int id)
        {
            var gym = _context.Gyms.Find(id);
            if (gym == null) return NotFound();
            return View(gym);
        }

        [HttpPost]
        public IActionResult EditGym(Gym model)
        {
            if (ModelState.IsValid)
            {
                _context.Gyms.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Gyms");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteGym(int id)
        {
            var gym = _context.Gyms.Find(id);
            if (gym != null)
            {
                _context.Gyms.Remove(gym);
                _context.SaveChanges();
            }
            return RedirectToAction("Gyms");
        }

        // --- Service Management ---
        public IActionResult Services()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        public IActionResult CreateService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateService(Service model)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Services");
            }
            return View(model);
        }

        public IActionResult EditService(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public IActionResult EditService(Service model)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Services");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
            return RedirectToAction("Services");
        }

        // --- User / Admin Management ---
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
        // ... (Existing User Actions) ...

        // --- Employee / Member Management ---
        public IActionResult Employees()
        {
            return View(_context.Employees.ToList());
        }

        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Employees");
            }
            return View(model);
        }

        public IActionResult EditEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Employees");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    // Find associated identity user by email if exists
                    var user = await _userManager.FindByEmailAsync(employee.Email);
                    if (user != null)
                    {
                        var result = await _userManager.DeleteAsync(user);
                        if (!result.Succeeded)
                        {
                            TempData["Error"] = "Kullanıcı hesabı silinirken hata oluştu: " + string.Join(", ", result.Errors.Select(e => e.Description));
                            return RedirectToAction("Employees");
                        }
                    }

                    // Handle related appointments: set to Available and nullify EmployeeId
                    var relatedAppointments = _context.TrainerAvailabilities.Where(a => a.EmployeeId == employee.Id);
                    foreach (var app in relatedAppointments)
                    {
                        app.Status = "Available";
                        app.EmployeeId = null;
                    }

                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Üye silinirken bir hata oluştu: " + ex.Message;
            }
            return RedirectToAction("Employees");
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true // Auto confirm for admin created users
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Users");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
                return View(new UserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email });
            else
                return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            IdentityUser user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.UserName;

                if (!string.IsNullOrEmpty(model.Password))
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Users");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            else
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Users");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            else
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");

            return View("Users", _userManager.Users.ToList());
        }
        // --- Appointments Management ---
        public IActionResult Appointments()
        {
            var appointments = _context.TrainerAvailabilities
                .Include(a => a.Trainer)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .Where(a => a.Status != "Available")
                .ToList();
            return View(appointments);
        }

        [HttpPost]
        public IActionResult ApproveAppointment(int id)
        {
            var app = _context.TrainerAvailabilities.Find(id);
            if (app != null)
            {
                app.Status = "Approved";
                _context.SaveChanges();
                TempData["Message"] = "Randevu onaylandı.";
            }
            return RedirectToAction("Appointments");
        }

        [HttpPost]
        public IActionResult CancelAppointment(int id)
        {
            var app = _context.TrainerAvailabilities.Find(id);
            if (app != null)
            {
                app.Status = "Available";
                app.EmployeeId = null;
                app.ServiceId = null;
                _context.SaveChanges();
                TempData["Message"] = "Randevu reddedildi / iptal edildi.";
            }
            return RedirectToAction("Appointments");
        }

        // --- Available Trainers Filter ---
        public IActionResult AvailableTrainers(DateTime? date)
        {
            if (date == null)
            {
                // Initial view, no date selected
                return View(new List<Trainer>());
            }

            // Find trainers who have at least one 'Available' slot on the given date
            // StartTime is 'timestamp without time zone' in PG usually, but EF handles comparison
            // We compare Date part.

            var targetDate = date.Value.Date;
            var nextDate = targetDate.AddDays(1);

            var availableTrainerIds = _context.TrainerAvailabilities
                .Where(a => a.Status == "Available" &&
                            a.StartTime >= targetDate && a.StartTime < nextDate)
                .Select(a => a.TrainerID)
                .Distinct()
                .ToList();

            var trainers = _context.Trainers
                .Where(t => availableTrainerIds.Contains(t.TrainerID))
                .ToList();

            ViewBag.SelectedDate = date.Value.ToString("yyyy-MM-dd");
            return View(trainers);
        }
        // --- Calendar Management ---
        public IActionResult Calendar()
        {
            var trainers = _context.Trainers.ToList();
            return View(trainers);
        }

        public IActionResult GetCalendarEvents()
        {
            var events = _context.TrainerAvailabilities
                .Include(a => a.Trainer)
                .Select(a => new
                {
                    id = a.AvailabilityID,
                    title = (a.Trainer != null ? a.Trainer.FullName : "Eğitmen") + " - " + (a.Status == "Available" ? "Müsait" : (a.Status == "Booked" ? "Rezerve" : "Onaylı")),
                    start = a.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"), // ISO 8601 format
                    end = a.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = a.Status == "Available" ? "#28a745" : (a.Status == "Booked" ? "#ffc107" : "#17a2b8"), // Green, Yellow, Blue
                    textColor = a.Status == "Booked" ? "#000" : "#fff",
                    description = a.Status == "Available" ? "Müsait" : "Dolu"
                })
                .ToList();

            return Json(events);
        }
        // ADMIN: Yeni Müsaitlik Ekleme (Admin Panel)
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        public IActionResult AddAvailability([FromBody] TrainerAvailability model)
        {
            if (model.TrainerID == 0) return BadRequest("Eğitmen seçimi yapılmadı.");
            try
            {
                var exists = _context.TrainerAvailabilities.Any(a =>
                    a.TrainerID == model.TrainerID &&
                    ((model.StartTime >= a.StartTime && model.StartTime < a.EndTime) ||
                     (model.EndTime > a.StartTime && model.EndTime <= a.EndTime) ||
                     (model.StartTime <= a.StartTime && model.EndTime >= a.EndTime)));
                if (exists) return BadRequest("Bu saat aralığında zaten bir kayıt (Müsait veya Dolu) mevcut.");
                model.Status = "Available";
                model.EmployeeId = null;
                _context.TrainerAvailabilities.Add(model);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Veritabanı hatası: " + ex.Message });
            }
        }
    }
}
