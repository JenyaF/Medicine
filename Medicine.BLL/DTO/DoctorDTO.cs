
using System.Collections.Generic;


namespace Medicine.BLL.DTO
{
    public class DoctorDTO : UserDTO
    {
        public string Qualification { get; set; }
        public virtual ICollection<PatientDTO> Patients { get; set; }
        public DoctorDTO()
        {
            Patients = new List<PatientDTO>();
        }
    }
}
