namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addphoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendants", "Photo", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendants", "Photo");
        }
    }
}
