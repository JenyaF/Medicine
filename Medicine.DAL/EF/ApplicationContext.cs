using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Medicine.DAL.Entities;

namespace Medicine.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>//<ApplicationUser,ApplicationRole,int,IdentityUserLogin<int>,IdentityUserRole<int>,IdentityUserClaim<int>>
    {
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new StoreDbInitializer());
        }
        public ApplicationContext(string conectionString) : base(conectionString) { }

         public DbSet<ClientProfile> ClientProfiles { get; set; }
         public DbSet<Patient> Patients { get; set; }
         public DbSet<Doctor> Doctors { get; set; }
         public DbSet<Medicament> Medicaments { get; set; }
         public DbSet<Recipe> Recipes { get; set; }
       /* protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<ClientProfile>().ToTable("ClientProfiles");
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        }*/
    }
}
