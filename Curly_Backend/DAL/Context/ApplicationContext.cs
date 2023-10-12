using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DAL.Context;

public class ApplicationContext : IdentityDbContext
{
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }
    
    public required DbSet<Favor> Favors { get; set; }

    public required DbSet<Appointment> Appointments { get; set; }
    
    public required DbSet<Barber> Barbers { get; set; }
    
    public required DbSet<Client> Clients { get; set; }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUser>().Property(u => u.Id).UseIdentityAlwaysColumn();
        builder.Entity<IdentityUser>().UseTptMappingStrategy();
        
        builder.Entity<Favor>().HasKey(e => e.Id);
        builder.Entity<Favor>().Property(e => e.Id).UseIdentityAlwaysColumn();
        
        builder.Entity<Appointment>().HasKey(e => e.Id);
        builder.Entity<Appointment>().Property(e => e.Id).UseIdentityAlwaysColumn();
        builder.Entity<Appointment>().HasMany(a => a.Favors).WithMany();

        //builder.Entity<Barber>().HasKey(b => b.Id);
        //builder.Entity<Barber>().Property(b => b.Id).UseIdentityAlwaysColumn();
        builder.Entity<Barber>().HasMany(b => b.Appointments);

        //builder.Entity<Client>().HasKey(b => b.Id);
        //builder.Entity<Client>().Property(b => b.Id).UseIdentityAlwaysColumn();
        builder.Entity<Client>().HasMany(c => c.Appointments);
        
        base.OnModelCreating(builder);
    }
}