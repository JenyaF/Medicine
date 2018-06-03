using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.DAL.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        public virtual Medicament Medicament { get; set; }
        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }      
        public double Volume { get; set; }
        public int AmountPerDay { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
    }
}
