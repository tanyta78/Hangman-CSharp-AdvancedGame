namespace Hangman.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangewordsLengthconstrain : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Words", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Words", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
