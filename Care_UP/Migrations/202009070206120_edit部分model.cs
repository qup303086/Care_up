namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit部分model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendants", "Photo", c => c.String(maxLength: 50));
            AlterColumn("dbo.CareRecords", "InitDate", c => c.DateTime());
            AlterColumn("dbo.CareRecords", "EditDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "Total", c => c.Int());
            AlterColumn("dbo.Orders", "Star", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Star", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Total", c => c.Int(nullable: false));
            AlterColumn("dbo.CareRecords", "EditDate", c => c.String());
            AlterColumn("dbo.CareRecords", "InitDate", c => c.String());
            DropColumn("dbo.Attendants", "Photo");
        }
    }
}
