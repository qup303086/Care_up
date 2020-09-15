namespace Care_UP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class 增加QA用的資料表 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Attendant = c.String(),
                        Answer = c.String(),
                        ReplyTime = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttendantId = c.Int(nullable: false),
                        MemberAccount = c.String(nullable: false),
                        Quiz = c.String(nullable: false),
                        InitDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionAnswers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.QuestionAnswers", new[] { "QuestionId" });
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionAnswers");
        }
    }
}
