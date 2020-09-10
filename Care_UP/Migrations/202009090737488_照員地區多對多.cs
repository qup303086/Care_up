namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 照員地區多對多 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Elders", "Remarks", c => c.String());
            AlterColumn("dbo.CareRecords", "Mood", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Place", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Body", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Equipment", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "ServiceItems", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Urgent", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Relationship", c => c.String(nullable: false));
            AlterColumn("dbo.Elders", "Phone", c => c.String(nullable: false));
            DropColumn("dbo.DetailedDates", "CareRecord");
            DropColumn("dbo.Orders", "Remarks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Remarks", c => c.String());
            AddColumn("dbo.DetailedDates", "CareRecord", c => c.String());
            AlterColumn("dbo.Elders", "Phone", c => c.String());
            AlterColumn("dbo.Elders", "Relationship", c => c.String());
            AlterColumn("dbo.Elders", "Urgent", c => c.String());
            AlterColumn("dbo.Elders", "ServiceItems", c => c.String());
            AlterColumn("dbo.Elders", "Equipment", c => c.String());
            AlterColumn("dbo.Elders", "Body", c => c.String());
            AlterColumn("dbo.Elders", "Address", c => c.String());
            AlterColumn("dbo.Elders", "Place", c => c.String());
            AlterColumn("dbo.Elders", "Name", c => c.String());
            AlterColumn("dbo.CareRecords", "Mood", c => c.String());
            DropColumn("dbo.Elders", "Remarks");
        }
    }
}
