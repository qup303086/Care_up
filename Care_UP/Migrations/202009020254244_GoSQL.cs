namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoSQL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 10),
                        PasswordSalt = c.String(),
                        LocationId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Salary = c.String(nullable: false, maxLength: 50),
                        Account = c.String(nullable: false, maxLength: 50),
                        Service = c.String(nullable: false),
                        File = c.String(nullable: false, maxLength: 50),
                        ServiceTime = c.String(),
                        Experience = c.String(nullable: false),
                        StartDateTime = c.DateTime(),
                        EndDateTime = c.DateTime(),
                        InitDate = c.DateTime(),
                        EditDate = c.DateTime(),
                        Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Area = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElderId = c.Int(nullable: false),
                        AttendantId = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        StopDate = c.DateTime(),
                        Total = c.Int(nullable: false),
                        Remarks = c.String(),
                        Comment = c.String(),
                        Star = c.Int(nullable: false),
                        Cancel = c.String(),
                        InitDate = c.DateTime(),
                        EditDate = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attendants", t => t.AttendantId, cascadeDelete: true)
                .ForeignKey("dbo.Elders", t => t.ElderId, cascadeDelete: true)
                .Index(t => t.ElderId)
                .Index(t => t.AttendantId);
            
            CreateTable(
                "dbo.DetailedDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        CareRecord = c.String(),
                        NowDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.CareRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailedDateId = c.Int(nullable: false),
                        Mood = c.String(),
                        InitDate = c.String(),
                        EditDate = c.String(),
                        Whether = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DetailedDates", t => t.DetailedDateId, cascadeDelete: true)
                .Index(t => t.DetailedDateId);
            
            CreateTable(
                "dbo.Elders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        Name = c.String(),
                        Gender = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Place = c.String(),
                        Address = c.String(),
                        Body = c.String(),
                        Equipment = c.String(),
                        ServiceItems = c.Int(nullable: false),
                        Urgent = c.String(),
                        Relationship = c.String(),
                        Phone = c.Int(nullable: false),
                        InitDate = c.DateTime(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 10),
                        PasswordSalt = c.String(),
                        InitDate = c.DateTime(),
                        EditDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ElderId", "dbo.Elders");
            DropForeignKey("dbo.Elders", "MemberId", "dbo.Members");
            DropForeignKey("dbo.DetailedDates", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.CareRecords", "DetailedDateId", "dbo.DetailedDates");
            DropForeignKey("dbo.Orders", "AttendantId", "dbo.Attendants");
            DropForeignKey("dbo.Locations", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Attendants", "LocationId", "dbo.Locations");
            DropIndex("dbo.Elders", new[] { "MemberId" });
            DropIndex("dbo.CareRecords", new[] { "DetailedDateId" });
            DropIndex("dbo.DetailedDates", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "AttendantId" });
            DropIndex("dbo.Orders", new[] { "ElderId" });
            DropIndex("dbo.Locations", new[] { "CityId" });
            DropIndex("dbo.Attendants", new[] { "LocationId" });
            DropTable("dbo.Members");
            DropTable("dbo.Elders");
            DropTable("dbo.CareRecords");
            DropTable("dbo.DetailedDates");
            DropTable("dbo.Orders");
            DropTable("dbo.Cities");
            DropTable("dbo.Locations");
            DropTable("dbo.Attendants");
        }
    }
}
