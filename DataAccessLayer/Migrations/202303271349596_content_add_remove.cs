namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class content_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "ContentRemove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "ContentRemove");
        }
    }
}
