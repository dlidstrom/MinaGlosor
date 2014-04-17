using System.Data.Entity.Migrations;

namespace MinaGlosor.Web.Data.Migrations
{
    public partial class PostPracticeSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PracticeSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(),
                        WordListId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OwnerId)
                .ForeignKey("dbo.WordLists", t => t.WordListId)
                .Index(t => t.WordListId)
                .Index(t => t.OwnerId);

            CreateTable(
                "dbo.PracticeWords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordScoreId = c.Int(nullable: false),
                        PracticeSessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PracticeSessions", t => t.PracticeSessionId)
                .ForeignKey("dbo.WordScores", t => t.WordScoreId)
                .Index(t => t.WordScoreId)
                .Index(t => t.PracticeSessionId);

            CreateTable(
                "dbo.WordScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        WordId = c.Int(nullable: false),
                        EasynessFactor = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Words", t => t.WordId)
                .Index(t => t.UserId)
                .Index(t => t.WordId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PracticeSessions", "WordListId", "dbo.WordLists");
            DropForeignKey("dbo.PracticeWords", "WordScoreId", "dbo.WordScores");
            DropForeignKey("dbo.WordScores", "WordId", "dbo.Words");
            DropForeignKey("dbo.WordScores", "UserId", "dbo.Users");
            DropForeignKey("dbo.PracticeWords", "PracticeSessionId", "dbo.PracticeSessions");
            DropForeignKey("dbo.PracticeSessions", "OwnerId", "dbo.Users");
            DropIndex("dbo.WordScores", new[] { "WordId" });
            DropIndex("dbo.WordScores", new[] { "UserId" });
            DropIndex("dbo.PracticeWords", new[] { "PracticeSessionId" });
            DropIndex("dbo.PracticeWords", new[] { "WordScoreId" });
            DropIndex("dbo.PracticeSessions", new[] { "OwnerId" });
            DropIndex("dbo.PracticeSessions", new[] { "WordListId" });
            DropTable("dbo.WordScores");
            DropTable("dbo.PracticeWords");
            DropTable("dbo.PracticeSessions");
        }
    }
}