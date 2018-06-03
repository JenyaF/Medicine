using System.Collections.Generic;

namespace Medicine.BLL.DTO
{
    public class PatientDTO : UserDTO
    {
        public string historyOfTreatment { get; set; }
        public string DoctorId { get; set; }
        public virtual ICollection<RecipeDTO> RecipeDTOs { get; set; }
        public PatientDTO()
        {
            RecipeDTOs = new List<RecipeDTO>();
        }
    }
}
