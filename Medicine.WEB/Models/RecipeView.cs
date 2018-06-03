using System.ComponentModel.DataAnnotations;

namespace Medicine.WEB.Models
{
    public class RecipeView
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        [Required]
        public string MedicamentName { get; set; }
        [Required]     
        public string PatientId { get; set; }
        [Range(0.1, 10)]
        public double Volume { get; set; }
        [Range(1, 1000)]
        public int AmountPerDay { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]        
        public string FinishDate { get; set; }
    }
}