namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 移除order的starttime問號移除detaildate新增carerecord欄位remark : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CareRecords", "DetailedDateId", "dbo.DetailedDates");
            DropIndex("dbo.CareRecords", new[] { "DetailedDateId" });
            AddColumn("dbo.CareRecords", "OrdersID", c => c.Int(nullable: false));
            AddColumn("dbo.CareRecords", "Remark", c => c.String());
            AddColumn("dbo.CareRecords", "WriteTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "EndDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.CareRecords", "OrdersID");
            AddForeignKey("dbo.CareRecords", "OrdersID", "dbo.Orders", "Id", cascadeDelete: true);
            DropColumn("dbo.CareRecords", "DetailedDateId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareRecords", "DetailedDateId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CareRecords", "OrdersID", "dbo.Orders");
            DropIndex("dbo.CareRecords", new[] { "OrdersID" });
            AlterColumn("dbo.Orders", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Orders", "StartDate", c => c.DateTime());
            DropColumn("dbo.CareRecords", "WriteTime");
            DropColumn("dbo.CareRecords", "Remark");
            DropColumn("dbo.CareRecords", "OrdersID");
            CreateIndex("dbo.CareRecords", "DetailedDateId");
            AddForeignKey("dbo.CareRecords", "DetailedDateId", "dbo.DetailedDates", "Id", cascadeDelete: true);
        }
    }
}
