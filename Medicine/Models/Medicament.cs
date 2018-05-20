using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(0,1000)]
        public double Volume { get; set; }
        [Range(0,10)]
        public int AmountPerDay { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public Medicament()
        {
            Patients = new List<Patient>();
        }
    }
}
