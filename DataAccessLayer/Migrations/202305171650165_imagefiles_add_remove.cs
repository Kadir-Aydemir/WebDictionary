namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagefiles_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageFiles", "ImageRemove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageFiles", "ImageRemove");
        }
    }
}
