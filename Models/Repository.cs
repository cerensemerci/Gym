using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Basics.Models
{
    public class RepositoryContext : IdentityDbContext<IdentityUser>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerAvailability> TrainerAvailabilities { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Gyms
            builder.Entity<Gym>().HasData(
                new Gym { Id = 1, Name = "Sakarya Merkez Spor Salonu", Address = "Serdivan, Sakarya", WorkingHours = "07:00 - 23:00" },
                new Gym { Id = 2, Name = "Erenler VIP Gym", Address = "Erenler, Sakarya", WorkingHours = "08:00 - 22:00" }
            );

            // Seed Services
            builder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Fitness", Duration = 60, Price = 100 },
                new Service { Id = 2, Name = "Yoga", Duration = 45, Price = 150 },
                new Service { Id = 3, Name = "Pilates", Duration = 50, Price = 200 },
                new Service { Id = 4, Name = "Crossfit", Duration = 60, Price = 180 }
            );

            // Seed Trainers
            builder.Entity<Trainer>().HasData(
                new Trainer { TrainerID = 1, FirstName = "Ahmet", LastName = "Yılmaz", Specialty = "Fitness & Bodybuilding" },
                new Trainer { TrainerID = 2, FirstName = "Ayşe", LastName = "Demir", Specialty = "Yoga & Pilates" },
                new Trainer { TrainerID = 3, FirstName = "Mehmet", LastName = "Kaya", Specialty = "Crossfit & Hiit" }
            );

            // Seed many-to-many (Trainer <-> Service)
            // Join table name defaults to ServiceTrainer with FKs ServiceId and TrainersTrainerID
            builder.Entity("ServiceTrainer").HasData(
                new { ServicesId = 1, TrainersTrainerID = 1 }, // Ahmet -> Fitness
                new { ServicesId = 2, TrainersTrainerID = 2 }, // Ayşe -> Yoga
                new { ServicesId = 3, TrainersTrainerID = 2 }, // Ayşe -> Pilates
                new { ServicesId = 4, TrainersTrainerID = 3 }  // Mehmet -> Crossfit
            );

            // Seed Roles
            var adminRoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210";
            var userRoleId = "8e445865-a24d-4543-a6c6-9443d048cdb9";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            // Seed Admin User
            var adminUserId = "8e445865-a24d-4543-a6c6-9443d048b0";
            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "b201210581@sakarya.edu.tr",
                NormalizedUserName = "B201210581@SAKARYA.EDU.TR",
                Email = "b201210581@sakarya.edu.tr",
                NormalizedEmail = "B201210581@SAKARYA.EDU.TR",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "sau");

            builder.Entity<IdentityUser>().HasData(adminUser);

            // Assign Admin Role to User
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId }
            );
        }
    }
}