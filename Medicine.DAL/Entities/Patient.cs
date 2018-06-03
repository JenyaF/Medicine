using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.DAL.Entities
{
    public class Patient
    {
        [Key]
        [ForeignKey("ClientProfile")]
        public string Id { get; set; }
        public virtual ClientProfile ClientProfile { get;set; }
        public string historyOfTreatment { get; set; }
        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public Patient()
        {
            Recipes = new List<Recipe>();
        }
    }
}
