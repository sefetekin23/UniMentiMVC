using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniMenti.Models;
using UniMenti.ViewModels;


namespace UniMenti.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {   
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> Users { get; set; }


    }
}
