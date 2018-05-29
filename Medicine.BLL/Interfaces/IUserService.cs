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
        Task<OperationDetails> CreateAsync(UserDTO userDto);
       /* Task<OperationDetails> CreateDoctorAsync(DoctorDTO doctorDTO);
      //  Task<OperationDetails> CreatePatientAsync(PatientDTO patientDTO);
        IEnumerable<DoctorDTO> GetDoctors();
        IEnumerable<PatientDTO> GetPatients();
       // Task<OperationDetails> UpdateDoctorAsync(DoctorDTO doctorDTO);
      //  Task<OperationDetails> UpdatePatientAsync(PatientDTO patientDTO);
        Task<OperationDetails> DeleteDoctorAsync(string doctorId);
        Task<OperationDetails> DeletePatientAsync(string patientId);

        // Task<OperationDetails> Delete(string Id);
        //    Task<OperationDetails> Update(UserDTO userDto);*/
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        string GetRole(string email);

        IEnumerable<PatientDTO> GetAll(string doctorId);
        void Update(PatientDTO patientDTO);
        void Create(PatientDTO patientDTO);
        void Delete(string patientId);
        void Create(DoctorDTO doctorDTO);
        IEnumerable<DoctorDTO> GetAll();
        void Delete(string id, string doctorId);
        void Update(DoctorDTO doctorDTO);
        string GetDoctorId(string email);
        DoctorDTO GetDoctor(string id);
        PatientDTO GetPatient(string id);
    }
}