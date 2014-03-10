using System.Data.Entity.Migrations;

namespace MinaGlosor.Web.Data.Migrations
{
    public partial class User_CreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CreatedDate");
        }
    }
}
