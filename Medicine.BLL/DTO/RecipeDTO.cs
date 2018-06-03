using Medicine.DAL.Entities;

namespace Medicine.BLL.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }  
        public string MedicamentName { get; set; }
        public virtual Medicament Medicament { get; set; }
        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public double Volume { get; set; }
        public int AmountPerDay { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
    }
}
