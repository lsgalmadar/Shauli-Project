namespace ShauliBlogProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNullPostComment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.Comment", "content_comment", c => c.String(nullable: false));
            AlterColumn("dbo.Post", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Post", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.Post", "url_author_post", c => c.String(nullable: false));
            AlterColumn("dbo.Post", "contentPost", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Post", "contentPost", c => c.String());
            AlterColumn("dbo.Post", "url_author_post", c => c.String());
            AlterColumn("dbo.Post", "Author", c => c.String());
            AlterColumn("dbo.Post", "Title", c => c.String());
            AlterColumn("dbo.Comment", "content_comment", c => c.String());
            AlterColumn("dbo.Comment", "Author", c => c.String());
        }
    }
}
