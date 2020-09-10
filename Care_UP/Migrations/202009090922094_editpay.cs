namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pays", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pays", "Status");
        }
    }
}
