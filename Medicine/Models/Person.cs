using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data.Entity;

namespace Medicine.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }  
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }  
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
