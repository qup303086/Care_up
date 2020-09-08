namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 照員地區多對多 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AttendantLocations", "AttendantId", "dbo.Attendants");
            DropForeignKey("dbo.AttendantLocations", "LocationId", "dbo.Locations");
            DropIndex("dbo.AttendantLocations", new[] { "AttendantId" });
            DropIndex("dbo.AttendantLocations", new[] { "LocationId" });
            CreateTable(
                "dbo.LocationsAttendants",
                c => new
                    {
                        Locations_Id = c.Int(nullable: false),
                        Attendants_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Locations_Id, t.Attendants_Id })
                .ForeignKey("dbo.Locations", t => t.Locations_Id, cascadeDelete: true)
                .ForeignKey("dbo.Attendants", t => t.Attendants_Id, cascadeDelete: true)
                .Index(t => t.Locations_Id)
                .Index(t => t.Attendants_Id);
            
            DropTable("dbo.AttendantLocations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AttendantLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttendantId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.LocationsAttendants", "Attendants_Id", "dbo.Attendants");
            DropForeignKey("dbo.LocationsAttendants", "Locations_Id", "dbo.Locations");
            DropIndex("dbo.LocationsAttendants", new[] { "Attendants_Id" });
            DropIndex("dbo.LocationsAttendants", new[] { "Locations_Id" });
            DropTable("dbo.LocationsAttendants");
            CreateIndex("dbo.AttendantLocations", "LocationId");
            CreateIndex("dbo.AttendantLocations", "AttendantId");
            AddForeignKey("dbo.AttendantLocations", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AttendantLocations", "AttendantId", "dbo.Attendants", "Id", cascadeDelete: true);
        }
    }
}
