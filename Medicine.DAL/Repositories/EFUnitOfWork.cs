using Medicine.DAL.EF;
using Medicine.DAL.Entities;
using Medicine.DAL.Interfaces;
using Medicine.DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Medicine.DAL.Identity;
using System.Threading.Tasks;

namespace Medicine.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;
        private DoctorRepository doctorRepository;
        private PatientRepository patientRepository;
        private MedicamentRepository medicamentRepository;
        private RecipeRepository recipeRepository;
        public EFUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser/*,ApplicationRole,int,IdentityUserLogin<int>,IdentityUserRole<int>,IdentityUserClaim<int>*/>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole/*,int,IdentityUserRole<int>*/>(db));
            clientManager = new ClientManager(db);
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }
        public IDoctorManager Doctors { get => doctorRepository ?? new DoctorRepository(db); }
        public IPatientManager Patients { get => patientRepository ?? new PatientRepository(db); }
        public IMedicamentManeger Medicaments { get => medicamentRepository ?? new MedicamentRepository(db); }
        public IRecipeManeger Recipes { get => recipeRepository ?? new RecipeRepository(db); }
        /*public void Save()
        {
            db.SaveChanges();
        }
        */
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
