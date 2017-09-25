namespace ShauliBlogProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostType",
                c => new
                    {
                        PostTypeID = c.Int(nullable: false),
                        Sport = c.Int(nullable: false),
                        Music = c.Int(nullable: false),
                        Food = c.Int(nullable: false),
                        Vacation = c.Int(nullable: false),
                        Study = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostTypeID)
                .ForeignKey("dbo.Post", t => t.PostTypeID)
                .Index(t => t.PostTypeID);
            
            CreateTable(
                "dbo.UserType",
                c => new
                    {
                        UserTypeID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        PostID = c.Int(nullable: false),
                        Visit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostType", "PostTypeID", "dbo.Post");
            DropIndex("dbo.PostType", new[] { "PostTypeID" });
            DropTable("dbo.UserType");
            DropTable("dbo.PostType");
        }
    }
}
