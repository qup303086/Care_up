namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 因應列舉改動 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendants", "ServiceTime", c => c.Int(nullable: false));
            AlterColumn("dbo.Attendants", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Status", c => c.String());
            AlterColumn("dbo.Attendants", "Status", c => c.String());
            AlterColumn("dbo.Attendants", "ServiceTime", c => c.String());
        }
    }
}
