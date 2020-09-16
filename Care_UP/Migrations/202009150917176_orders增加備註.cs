namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders增加備註 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Remark");
        }
    }
}
