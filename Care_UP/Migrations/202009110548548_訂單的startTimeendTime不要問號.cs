namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 訂單的startTimeendTime不要問號 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "StartDate", c => c.DateTime());
        }
    }
}
