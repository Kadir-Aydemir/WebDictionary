﻿namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class writer_add_title : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Writers", "WriterTitle", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Writers", "WriterTitle");
        }
    }
}
