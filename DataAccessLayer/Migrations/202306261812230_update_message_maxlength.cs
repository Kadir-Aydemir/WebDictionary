namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_message_maxlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DraftMessages", "DraftMessageContent", c => c.String());
            AlterColumn("dbo.Messages", "MessageContent", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "MessageContent", c => c.String(maxLength: 1000));
            AlterColumn("dbo.DraftMessages", "DraftMessageContent", c => c.String(maxLength: 1000));
        }
    }
}
