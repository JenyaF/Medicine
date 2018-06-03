using System.ComponentModel.DataAnnotations;

namespace Medicine.WEB.Models
{
    public class MedicamentView
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}