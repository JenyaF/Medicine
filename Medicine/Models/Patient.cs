using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.Models
{
    [Table("Patients")]
    public class Patient:Person
    {
        [MaxLength(200)]
        public string historyOfTreatment { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Medicament> Medicaments { get; set; }
        public Patient()
        {
            Medicaments = new List<Medicament>();
        }
        public Patient Copy()
        {
            return (Patient)this.MemberwiseClone();
        }
    }
}
