﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicine.BLL.DTO
{
    public class MedicamentDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}