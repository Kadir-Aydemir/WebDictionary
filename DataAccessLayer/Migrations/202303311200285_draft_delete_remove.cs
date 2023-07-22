namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class draft_delete_remove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DraftMessages", "Remove");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DraftMessages", "Remove", c => c.Boolean(nullable: false));
        }
    }
}
