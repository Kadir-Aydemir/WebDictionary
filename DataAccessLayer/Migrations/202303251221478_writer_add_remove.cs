namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class writer_add_remove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Writers", "WriterRemove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Writers", "WriterRemove");
        }
    }
}
