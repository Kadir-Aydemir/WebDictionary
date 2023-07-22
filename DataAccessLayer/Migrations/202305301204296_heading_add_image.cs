namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class heading_add_image : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Headings", "HeadingImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Headings", "HeadingImage");
        }
    }
}
