namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blocked_add_blockeddate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blockeds", "BlockedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blockeds", "BlockedDate");
        }
    }
}
