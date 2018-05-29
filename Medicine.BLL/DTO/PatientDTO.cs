using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.BLL.DTO
{
    public class PatientDTO : UserDTO
    {
        [MaxLength(200)]
        public string historyOfTreatment { get; set; }
        [Required]
        public string DoctorId { get; set; }
        public virtual ICollection<RecipeDTO> RecipeDTOs { get; set; }
        public PatientDTO()
        {
            RecipeDTOs = new List<RecipeDTO>();
        }
    }
}
