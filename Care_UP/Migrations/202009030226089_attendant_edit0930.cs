namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attendant_edit0930 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttendantLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttendantId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attendants", t => t.AttendantId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.AttendantId)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AttendantLocations", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.AttendantLocations", "AttendantId", "dbo.Attendants");
            DropIndex("dbo.AttendantLocations", new[] { "LocationId" });
            DropIndex("dbo.AttendantLocations", new[] { "AttendantId" });
            DropTable("dbo.AttendantLocations");
        }
    }
}
