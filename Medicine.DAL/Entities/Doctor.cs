using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.DAL.Entities
{
    public class Doctor
    {
        [Key]
        [ForeignKey("ClientProfile")]
        public string Id { get; set; }
        public virtual ClientProfile ClientProfile { get; set; }
        [MaxLength(100)]
        public string Qualification { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public Doctor()
        {
            Patients = new List<Patient>();
        }
    }
}
