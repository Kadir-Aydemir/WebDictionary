namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_like_class : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikeID = c.Int(nullable: false, identity: true),
                        ContentID = c.Int(nullable: false),
                        WriterID = c.Int(),
                        LikeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LikeID)
                .ForeignKey("dbo.Contents", t => t.ContentID, cascadeDelete: true)
                .ForeignKey("dbo.Writers", t => t.WriterID)
                .Index(t => t.ContentID)
                .Index(t => t.WriterID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "WriterID", "dbo.Writers");
            DropForeignKey("dbo.Likes", "ContentID", "dbo.Contents");
            DropIndex("dbo.Likes", new[] { "WriterID" });
            DropIndex("dbo.Likes", new[] { "ContentID" });
            DropTable("dbo.Likes");
        }
    }
}
