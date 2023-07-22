namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_blocked_class : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blockeds",
                c => new
                    {
                        BlockID = c.Int(nullable: false, identity: true),
                        WriterID = c.Int(),
                        ExpireDate = c.DateTime(nullable: false),
                        Reason = c.String(maxLength: 200),
                        Situation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BlockID)
                .ForeignKey("dbo.Writers", t => t.WriterID)
                .Index(t => t.WriterID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blockeds", "WriterID", "dbo.Writers");
            DropIndex("dbo.Blockeds", new[] { "WriterID" });
            DropTable("dbo.Blockeds");
        }
    }
}
