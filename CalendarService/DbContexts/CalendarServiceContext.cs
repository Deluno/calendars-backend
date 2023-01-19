using CalendarService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.DbContexts;

public class CalendarServiceContext : IdentityDbContext<AppUser, AppRole, int>
{
    public CalendarServiceContext(DbContextOptions<CalendarServiceContext> options) : base(options)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; } = null!;
    public DbSet<Calendar> Calendars { get; set; } = null!;
    public DbSet<CalendarEvent> CalendarEvents { get; set; } = null!;
    public DbSet<CalendarTask> CalendarTasks { get; set; } = null!;
    public DbSet<CalendarItem> CalendarItems { get; set; } = null!;
    public DbSet<CalendarItemFile> CalendarItemFiles { get; set; } = null!;
    public DbSet<CalendarUserSubscription> Subscriptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CalendarUserInvitation>()
            .HasKey(so => new { so.UserId, so.CalendarId });
        builder.Entity<CalendarUserInvitation>()
            .HasOne(so => so.User)
            .WithMany(u => u.SharedCalendars)
            .HasForeignKey(so => so.UserId);
        builder.Entity<CalendarUserInvitation>()
            .HasOne(so => so.Calendar)
            .WithMany(c => c.InvitedUsers)
            .HasForeignKey(so => so.CalendarId);

        builder.Entity<CalendarUserSubscription>()
            .HasKey(cs => new { cs.UserId, cs.CalendarId });
        builder.Entity<CalendarUserSubscription>()
            .HasOne(cs => cs.User)
            .WithMany(u => u.SubscribedCalendars)
            .HasForeignKey(cs => cs.UserId);
        builder.Entity<CalendarUserSubscription>()
            .HasOne(cs => cs.Calendar)
            .WithMany(c => c.SubscribedUsers)
            .HasForeignKey(cs => cs.CalendarId);

        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.HasIndex(u => u.Email).IsUnique();
        });
        builder.Entity<AppRole>(entity =>
        {
            entity.Property(r => r.Id).ValueGeneratedOnAdd();
            entity.HasIndex(r => r.Name).IsUnique();
        });

        builder
            .Ignore<IdentityUserLogin<string>>()
            .Ignore<IdentityUserToken<string>>();

        builder.Entity<AppUser>()
            .Ignore(u => u.AccessFailedCount)
            .Ignore(u => u.ConcurrencyStamp)
            .Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.SecurityStamp)
            .Ignore(u => u.TwoFactorEnabled);

        builder.Entity<Calendar>()
            .HasData(
                new Calendar
                {
                    Id = 1,
                    Name = "Admin Calendar",
                    OwnerId = 1,
                    IsPublic = false
                }
            );

        builder.Entity<AppUser>().ToTable("AppUsers");
        builder.Entity<Calendar>().ToTable("Calendars");
        builder.Entity<CalendarEvent>().ToTable("CalendarEvents");
        builder.Entity<CalendarTask>().ToTable("CalendarTasks");
        builder.Entity<CalendarItem>().ToTable("CalendarItems");
        builder.Entity<CalendarItemFile>().ToTable("CalendarItemFiles");

        SeedUsers(builder);
        SeedRoles(builder);
        SeedUserRoles(builder);
    }

    private static void SeedUsers(ModelBuilder builder)
    {
        var hasher = new PasswordHasher<AppUser>();
        var user = new AppUser
        {
            Id = 1,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@localhost",
            NormalizedEmail = "ADMIN@LOCALHOST"
        };
        user.PasswordHash = hasher.HashPassword(user, "admin");

        builder.Entity<AppUser>().HasData(user);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<AppRole>().HasData(
            new AppRole { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
            new AppRole { Id = 2, Name = "User", NormalizedName = "USER" }
        );
    }

    private static void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int> { RoleId = 1, UserId = 1 }
        );
    }
}