﻿using System;
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
        [Required]
        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        [Range(0.1, 1000)]
        public double Volume { get; set; }
        [Range(1, 10)]
        public int AmountPerDay { get; set; }
    }
}