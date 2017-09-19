namespace ShauliBlogProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSeggPostTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SeggPost",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ByAuthor = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SeggPost");
        }
    }
}
