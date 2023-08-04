using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerProject.Application.Model;

namespace TaskTrackerProject.Application.Infrastructure
{
    public class TaskTrackerContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<List> Lists => Set<List>();
        public DbSet<ListTask> Tasks => Set<ListTask>();

        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Additional config

            // Generic config for all entities
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }

        }
        /// <summary>
        /// Initialize the database with some values 
        /// enviroment is production.
        /// </summary>
        private async Task Initialize()
        {
            var users = new[]
            {
                new User(
                    username: "Ruffy",
                    password: "King",
                    email: "ruffy@spengergasse.at",
                    role: Userrole.Admin),
                new User(
                    username: "Zoro",
                    password: "Sword",
                    email: "zoro@spengergasse.at",
                    role: Userrole.User),
            };
            await Users.AddRangeAsync(users);
            await SaveChangesAsync();

            var userlist = new List( name: "GetStarted", user: users[0]);
            await Lists.AddAsync(userlist);
            await SaveChangesAsync();

            var task = new ListTask( 
                name: "Run 10km", 
                status: "Not Finished", 
                priority: "Low", 
                list: userlist, 
                date: new DateTime()
            );
            await Tasks.AddAsync(task);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Generates random values for testing the application. 
        /// enviroment is development.
        /// </summary>    
        private async Task Seed()
        {
            Randomizer.Seed = new Random(1039);
            var faker = new Faker("en");

            var users = new Faker<User>("en").CustomInstantiator(f =>
            {
                var username = f.Name.FirstName();
                return new User(
                    username: username.ToLower(),
                    password: "111",
                    email: $"{username.ToLower()}@spengergasse.at",
                    role: f.PickRandom<Userrole>())
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .GroupBy(a => a.Email).Select(g => g.First())
            .ToList();
            await Users.AddRangeAsync(users);
            await SaveChangesAsync();

            var lists = new Faker<List>("en").CustomInstantiator(f =>
            {
                return new List(
                    name: "GetStarted",
                    user: f.PickRandom<User>())
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .GroupBy(a => a.User).Select(g => g.First())
            .ToList();
            await Lists.AddRangeAsync(lists);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Creates the database. 
        /// Called once at application startup in Program.cs.
        /// </summary>    
        public async Task CreateDatabase(bool isDevelopment)
        {
            try
            {
                if (isDevelopment) { Database.EnsureDeleted(); }
                // EnsureCreated only creates the model if the database does not exist or it has no
                // tables. Returns true if the schema was created.  Returns false if there are
                // existing tables in the database. This avoids initializing multiple times.
                if (Database.EnsureCreated()) { await Initialize(); }
                if (isDevelopment) await Seed();
            }
            catch (Exception) {
                Console.WriteLine("Docker Container not on !!!");
            }
        }
    }
}
