namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editSalary : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendants", "Salary", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Attendants", "Salary", c => c.String(maxLength: 50));
        }
    }
}
