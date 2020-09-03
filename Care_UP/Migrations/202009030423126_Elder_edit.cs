namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Elder_edit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Elders", "ServiceItems", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Elders", "ServiceItems", c => c.Int(nullable: false));
        }
    }
}
