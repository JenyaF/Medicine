using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [MaxLength(200)]
        public string historyOfTreatment { get; set; }
        [Required]
        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public Patient()
        {
            Recipes = new List<Recipe>();
        }
    }
}
