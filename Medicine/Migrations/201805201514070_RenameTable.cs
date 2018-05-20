namespace Medicine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Medicines", newName: "Medicaments");
            RenameTable(name: "dbo.MedicinePatients", newName: "MedicamentPatients");
            RenameColumn(table: "dbo.MedicamentPatients", name: "Medicine_Id", newName: "Medicament_Id");
            RenameIndex(table: "dbo.MedicamentPatients", name: "IX_Medicine_Id", newName: "IX_Medicament_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.MedicamentPatients", name: "IX_Medicament_Id", newName: "IX_Medicine_Id");
            RenameColumn(table: "dbo.MedicamentPatients", name: "Medicament_Id", newName: "Medicine_Id");
            RenameTable(name: "dbo.MedicamentPatients", newName: "MedicinePatients");
            RenameTable(name: "dbo.Medicaments", newName: "Medicines");
        }
    }
}
