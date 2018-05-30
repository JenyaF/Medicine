using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.DTO;
using Medicine.BLL.Infrastructure;

namespace Medicine.BLL.Interfaces
{
    public interface IDoctorService 
    {
        IUserService userService { get; set; }
        IEnumerable<DoctorDTO> GetAll();
        void Create(DoctorDTO item);
        void Update(DoctorDTO item);
        void Delete(string id, string doctorId);
        //Task<OperationDetails> CreateAsync(DoctorDTO item);
       
    }
}
