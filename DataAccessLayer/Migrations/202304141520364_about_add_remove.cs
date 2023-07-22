namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class about_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Abouts", "AboutRemove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Abouts", "AboutRemove");
        }
    }
}
