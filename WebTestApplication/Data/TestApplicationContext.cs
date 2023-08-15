using Microsoft.EntityFrameworkCore;
using WebTestApplication.Models;

namespace WebTestApplication.Data
{
    public class TestApplicationContext : DbContext
    {
        public TestApplicationContext(DbContextOptions<TestApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().OwnsMany(c => c.Emails, ownedNavigationBuilder => ownedNavigationBuilder.Property(e => e.Id).ValueGeneratedOnAdd());
            modelBuilder.Entity<Contact>(c => c.Property(c => c.Id).ValueGeneratedOnAdd());
        }

        public DbSet<Contact> Contacts { get; set; } = default!;
    }

// Alternatively, it can be implemented a more classic approach having a DbSet<Email> and the one-to-many relation defined by .HasMany()
// However, the solution with DbSet<Email> ought to have an Email model defined with a foreign key ContactId in order to be useful and the test problem did not have a foreign key field in the Email model
// So, I guess that I do not have to manipulate directly the table with Emails through Entity Framework and the Web API is limited to Contacts

/*
    public class TestApplicationContext : DbContext
    {
        public TestApplicationContext(DbContextOptions<TestApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasMany(c => c.Emails);

            modelBuilder.Entity<Contact>(c => c.Property(c => c.Id).ValueGeneratedOnAdd());
            modelBuilder.Entity<Email>(e => e.Property(e => e.Id).ValueGeneratedOnAdd());
        }

        public DbSet<Contact> Contacts { get; set; } = default!;
        public DbSet<Email> Emails { get; set; } = default!;
    }

namespace WebTestApplication.Models
{
    public class Email
    {
        public long Id { get; set; }

        public bool IsPrimary { get; set; }

        public string Address { get; set; } = default!;

        // Foreign key ContactId related to Contact entity would allow to add an email to an existing Contact entity using an Emails Web API
        // If ContactId is not declared, then the emails can be added, updated and deleted only through Contacts Web API, modifying Emails collection of Contact entity then updating (saving) the Contact entity
        public long ContactId { get; set; }
    }
}
*/
}
