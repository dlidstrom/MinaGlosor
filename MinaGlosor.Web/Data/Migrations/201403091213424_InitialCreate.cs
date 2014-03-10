using System.Data.Entity.Migrations;

namespace MinaGlosor.Web.Data.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateAccountRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 320),
                        ActivationCode = c.Guid(nullable: false),
                        Used = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 320),
                        HashedPassword = c.String(nullable: false, maxLength: 120),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.WordLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 1024),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);

            CreateTable(
                "dbo.Words",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WordListId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Text = c.String(nullable: false, maxLength: 1024),
                        Definition = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WordLists", t => t.WordListId, cascadeDelete: true)
                .Index(t => t.WordListId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Words", "WordListId", "dbo.WordLists");
            DropForeignKey("dbo.WordLists", "OwnerId", "dbo.Users");
            DropIndex("dbo.Words", new[] { "WordListId" });
            DropIndex("dbo.WordLists", new[] { "OwnerId" });
            DropTable("dbo.Words");
            DropTable("dbo.WordLists");
            DropTable("dbo.Users");
            DropTable("dbo.CreateAccountRequests");
        }
    }
}