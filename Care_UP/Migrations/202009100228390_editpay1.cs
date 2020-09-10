namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpay1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Pays", "OrderId");
            AddForeignKey("dbo.Pays", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pays", "OrderId", "dbo.Orders");
            DropIndex("dbo.Pays", new[] { "OrderId" });
        }
    }
}
