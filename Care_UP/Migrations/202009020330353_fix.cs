namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attendants", "LocationId", "dbo.Locations");
            DropIndex("dbo.Attendants", new[] { "LocationId" });
            DropColumn("dbo.Attendants", "LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attendants", "LocationId", c => c.Int());
            CreateIndex("dbo.Attendants", "LocationId");
            AddForeignKey("dbo.Attendants", "LocationId", "dbo.Locations", "Id");
        }
    }
}
