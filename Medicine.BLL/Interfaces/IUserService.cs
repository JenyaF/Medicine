using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Medicine.BLL.DTO;
using Medicine.BLL.Infrastructure;
using Medicine.DAL.Interfaces;
namespace Medicine.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        IUnitOfWork Database { get; set; }
      //  Task<OperationDetails> CreateAsync(UserDTO userDto);
      //  Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto);
        ClaimsIdentity Authenticate(UserDTO userDto);
       // Task SetInitialData(UserDTO adminDto, List<string> roles);
        string GetRole(string email);
       void SetInitialData( List<string> roles);
        IEnumerable<PatientDTO> GetAll(string doctorId);
        void Update(PatientDTO patientDTO);
        OperationDetails Create(PatientDTO patientDTO);
        void Delete(string patientId);
        OperationDetails Create(DoctorDTO doctorDTO);
        IEnumerable<DoctorDTO> GetAll();
        void Delete(string id, string doctorId);
        void Update(DoctorDTO doctorDTO);
        string GetId(string email);

        DoctorDTO GetDoctor(string id);
        PatientDTO GetPatient(string id);
    }
}