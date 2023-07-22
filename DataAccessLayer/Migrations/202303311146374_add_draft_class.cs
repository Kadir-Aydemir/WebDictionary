namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_draft_class : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DraftMessages",
                c => new
                    {
                        DraftMessageId = c.Int(nullable: false, identity: true),
                        DraftSenderMail = c.String(maxLength: 50),
                        DraftReceiverMail = c.String(maxLength: 50),
                        DraftSubject = c.String(maxLength: 100),
                        DraftMessageContent = c.String(maxLength: 1000),
                        DraftMessageDate = c.DateTime(nullable: false),
                        Remove = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DraftMessageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DraftMessages");
        }
    }
}
