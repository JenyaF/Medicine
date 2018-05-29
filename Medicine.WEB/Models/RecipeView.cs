using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medicine.WEB.Models
{
    public class RecipeView
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        public string PatientId { get; set; }
        public double Volume { get; set; }
        public int AmountPerDay { get; set; }
    }
}