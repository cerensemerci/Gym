using Basics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly RepositoryContext _context;

        public DiagnosticController(RepositoryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return Ok("Diagnostic Controller is working");
        }

        public async Task<IActionResult> CheckTables()
        {
            try
            {
                var tables = new List<string>();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";
                    await _context.Database.OpenConnectionAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
                return Json(tables);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
