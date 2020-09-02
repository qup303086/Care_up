namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attendeant_edit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendants", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Attendants", "Salary", c => c.String(maxLength: 50));
            AlterColumn("dbo.Attendants", "Account", c => c.String(maxLength: 50));
            AlterColumn("dbo.Attendants", "Service", c => c.String());
            AlterColumn("dbo.Attendants", "File", c => c.String(maxLength: 50));
            AlterColumn("dbo.Attendants", "Experience", c => c.String());
            AlterColumn("dbo.Attendants", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Attendants", "Status", c => c.String(nullable: false));
            AlterColumn("dbo.Attendants", "Experience", c => c.String(nullable: false));
            AlterColumn("dbo.Attendants", "File", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Attendants", "Service", c => c.String(nullable: false));
            AlterColumn("dbo.Attendants", "Account", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Attendants", "Salary", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Attendants", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
