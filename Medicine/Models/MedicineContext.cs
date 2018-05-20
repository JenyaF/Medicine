using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Medicine.Models
{
    class MedicineContext:DbContext
    {
        static MedicineContext()
        {
            Database.SetInitializer<MedicineContext>(new MyContextInitilizer());
        }
        public MedicineContext() : base("MedicineContext") { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {    
            modelBuilder.Properties<string>().
                Configure(s => s.HasMaxLength(30)); 
            
            modelBuilder.Properties<string>().
                Where(s => s.Name == "Name").
                Configure(s => s.IsRequired());

        }
    }

}
