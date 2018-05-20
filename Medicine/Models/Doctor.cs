using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.Models
{
    [Table("Doctors")]
    public class Doctor:Person
    {
        public string Recomendation { get; set; }
        [MaxLength(100)]
        public string Qualification { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public Doctor()
        {
            Patients = new List<Patient>();
        }
    }
}
