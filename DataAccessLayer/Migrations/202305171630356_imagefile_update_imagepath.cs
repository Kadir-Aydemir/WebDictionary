namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagefile_update_imagepath : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.ImageFiles", "ImagePath", c => c.Binary());
            Sql("ALTER TABLE ImageFiles ADD NewImagePath VARBINARY(MAX)");

            Sql("UPDATE ImageFiles SET NewImagePath = CAST(ImagePath AS VARBINARY(MAX))");

            Sql("ALTER TABLE ImageFiles DROP COLUMN ImagePath");

            Sql("EXEC sp_rename 'ImageFiles.NewImagePath', 'ImagePath', 'COLUMN'");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ImageFiles", "ImagePath", c => c.String(maxLength: 250));
        }
    }
}
