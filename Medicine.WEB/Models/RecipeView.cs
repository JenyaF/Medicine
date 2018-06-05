using System.ComponentModel.DataAnnotations;
using System;
namespace Medicine.WEB.Models
{
    public class RecipeView
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        [Display(Name = "Name of medicament")]
        [Required]
        public string MedicamentName { get; set; }
        [Required]     
        public string PatientId { get; set; }
        [Range( 0.1, 10)]
        public double Volume { get; set; }
        [Display(Name ="Amount per day")]
        [Range(1,100)]
        public int AmountPerDay { get; set; }
        [Display(Name ="Start day")]
        [Required] 
        [DataType(DataType.Date)]
        public string StartDate { get; set; }
        [Display(Name ="Finish day")]
        [Required]
        [DataType(DataType.Date)]        
        public string FinishDate { get; set; }
    }
}