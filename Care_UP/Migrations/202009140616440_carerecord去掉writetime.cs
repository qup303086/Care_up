namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carerecord去掉writetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CareRecords", "InitDate", c => c.DateTime());
            DropColumn("dbo.CareRecords", "Whether");
            DropColumn("dbo.CareRecords", "WriteTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareRecords", "WriteTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CareRecords", "Whether", c => c.String());
            AlterColumn("dbo.CareRecords", "InitDate", c => c.DateTime());
        }
    }
}
