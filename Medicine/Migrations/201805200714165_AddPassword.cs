namespace Medicine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPassword : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Recomendation = c.String(maxLength: 30),
                        Qualification = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 30),
                        Surname = c.String(nullable: false, maxLength: 30),
                        DateOfBirth = c.DateTime(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        historyOfTreatment = c.String(maxLength: 200),
                        DoctorId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 30),
                        Surname = c.String(nullable: false, maxLength: 30),
                        DateOfBirth = c.DateTime(nullable: false),
                        Phone = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Volume = c.Double(nullable: false),
                        AmountPerDay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MedicinePatients",
                c => new
                    {
                        Medicine_Id = c.Int(nullable: false),
                        Patient_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Medicine_Id, t.Patient_Id })
                .ForeignKey("dbo.Medicines", t => t.Medicine_Id, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.Patient_Id, cascadeDelete: true)
                .Index(t => t.Medicine_Id)
                .Index(t => t.Patient_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicinePatients", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.MedicinePatients", "Medicine_Id", "dbo.Medicines");
            DropForeignKey("dbo.Patients", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.MedicinePatients", new[] { "Patient_Id" });
            DropIndex("dbo.MedicinePatients", new[] { "Medicine_Id" });
            DropIndex("dbo.Patients", new[] { "DoctorId" });
            DropTable("dbo.MedicinePatients");
            DropTable("dbo.Medicines");
            DropTable("dbo.Patients");
            DropTable("dbo.Doctors");
        }
    }
}
