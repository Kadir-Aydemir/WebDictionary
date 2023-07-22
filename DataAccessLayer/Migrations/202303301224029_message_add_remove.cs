namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class message_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Remove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Remove");
        }
    }
}
