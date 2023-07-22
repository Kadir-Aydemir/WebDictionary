namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class heading_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Headings", "HeadingRemove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Headings", "HeadingRemove");
        }
    }
}
