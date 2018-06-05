using Medicine.DAL.Identity;
using System;

namespace Medicine.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IDoctorManager Doctors { get; }
        IPatientManager Patients { get; }
        IRecipeManeger Recipes { get; }
        IMedicamentManeger Medicaments { get; }
        void Save();
    }
}
