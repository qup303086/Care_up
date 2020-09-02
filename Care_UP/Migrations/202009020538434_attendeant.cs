namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attendeant : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendants", "Password", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Attendants", "Password", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
