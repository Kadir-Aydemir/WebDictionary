namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class writer_update_imageformat : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE Writers ADD NewWriterImage VARBINARY(MAX)");

            Sql("UPDATE Writers SET NewWriterImage = CAST(WriterImage AS VARBINARY(MAX))");

            Sql("ALTER TABLE Writers DROP COLUMN WriterImage");

            Sql("EXEC sp_rename 'Writers.NewWriterImage', 'WriterImage', 'COLUMN'");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Writers", "WriterImage", c => c.String(maxLength: 100));
        }
    }
}
