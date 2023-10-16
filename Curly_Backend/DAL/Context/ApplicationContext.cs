using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class ApplicationContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }
    
    public required DbSet<Favor> Favors { get; set; }

    public required DbSet<Appointment> Appointments { get; set; }
    
    public required DbSet<Barber> Barbers { get; set; }
    
    public required DbSet<Client> Clients { get; set; }
    
    public required DbSet<Admin> Admins { get; set; }
    
    public required DbSet<Review> Reviews { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().Property(u => u.Id).UseIdentityAlwaysColumn();
        builder.Entity<ApplicationUser>().UseTptMappingStrategy();
        
        builder.Entity<Barber>().HasMany(b => b.Appointments).WithOne(a => a.Barber);
        builder.Entity<Barber>().HasMany(b => b.Reviews).WithOne(r => r.Barber);
        builder.Entity<Barber>().Ignore(b => b.Rating);
        builder.Entity<Barber>().Ignore(b => b.Earnings);
        
        builder.Entity<Client>().HasMany(c => c.Appointments).WithOne(a => a.Client);
        builder.Entity<Client>().HasMany(c => c.Reviews).WithOne(r => r.Publisher);
        
        builder.Entity<Favor>().HasKey(f => f.Id);
        builder.Entity<Favor>().Property(e => e.Id).UseIdentityAlwaysColumn();

        builder.Entity<Review>().HasKey(r => r.Id);
        builder.Entity<Review>().Property(r => r.Id).UseIdentityAlwaysColumn();
        
        builder.Entity<Appointment>().HasKey(e => e.Id);
        builder.Entity<Appointment>().Property(e => e.Id).UseIdentityAlwaysColumn();
        builder.Entity<Appointment>().HasMany(a => a.Favors).WithMany();
        builder.Entity<Appointment>().Property(e => e.PlacedAt).HasDefaultValue(DateTime.UtcNow);
        builder.Entity<Appointment>().Property(e => e.At);
        
        builder.Entity<IdentityRole<int>>().HasData(
            new { Id = -1, Name = nameof(Barber), NormalizedName = nameof(Barber).ToUpper() },
            new { Id = -2, Name = nameof(Client), NormalizedName = nameof(Client).ToUpper() },
            new { Id = -3, Name = nameof(Admin), NormalizedName = nameof(Admin).ToUpper() });
        
        base.OnModelCreating(builder);
    }
}