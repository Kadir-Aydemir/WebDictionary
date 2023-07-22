namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class about_delete_images : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Abouts", "Image1");
            DropColumn("dbo.Abouts", "Image2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Abouts", "Image2", c => c.String(maxLength: 100));
            AddColumn("dbo.Abouts", "Image1", c => c.String(maxLength: 100));
        }
    }
}
