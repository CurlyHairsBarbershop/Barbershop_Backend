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
    
    // public required DbSet<BarberRole> BarberRoles { get; set; }
    //
    // public required DbSet<CustomerRole> CustomerRoles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().Property(u => u.Id).UseIdentityAlwaysColumn();
        builder.Entity<ApplicationUser>().UseTptMappingStrategy();
        
        builder.Entity<Favor>().HasKey(e => e.Id);
        builder.Entity<Favor>().Property(e => e.Id).UseIdentityAlwaysColumn();
        
        builder.Entity<Appointment>().HasKey(e => e.Id);
        builder.Entity<Appointment>().Property(e => e.Id).UseIdentityAlwaysColumn();
        builder.Entity<Appointment>().HasMany(a => a.Favors).WithMany();
        
        builder.Entity<Barber>().HasMany(b => b.Appointments);
        
        builder.Entity<Client>().HasMany(c => c.Appointments);

        builder.Entity<IdentityRole<int>>().HasData(
            new { Id = -1, Name = nameof(Barber), NormalizedName = nameof(Barber).ToUpper() },
            new { Id = -2, Name = nameof(Client), NormalizedName = nameof(Client).ToUpper() });
        
        base.OnModelCreating(builder);
    }
}