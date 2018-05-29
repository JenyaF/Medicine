using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.DTO;

namespace Medicine.BLL.Interfaces
{
    public interface IPatientService
    {
        IUserService userService { get; set; }
        string DoctorId { get; set; }
        IEnumerable<PatientDTO> GetAll();
        void Create(PatientDTO item);
        void Update(PatientDTO item);
        void Delete(string id);
    }
}
