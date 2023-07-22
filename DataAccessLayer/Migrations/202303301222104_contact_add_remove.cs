namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contact_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Remove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Remove");
        }
    }
}
