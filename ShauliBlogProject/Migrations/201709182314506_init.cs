namespace ShauliBlogProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        Title = c.String(),
                        Author = c.String(),
                        url_author_comment = c.String(),
                        content_comment = c.String(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Post", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        url_author_post = c.String(),
                        postDate = c.DateTime(nullable: false),
                        contentPost = c.String(),
                        image = c.String(),
                        video = c.String(),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.Fan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        gender = c.String(nullable: false),
                        birthDate = c.DateTime(nullable: false),
                        seniority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FansClub",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        Gender = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Seniority = c.Int(nullable: false),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Maps",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Address = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "PostID", "dbo.Post");
            DropIndex("dbo.Comment", new[] { "PostID" });
            DropTable("dbo.Maps");
            DropTable("dbo.FansClub");
            DropTable("dbo.Fan");
            DropTable("dbo.Post");
            DropTable("dbo.Comment");
        }
    }
}
