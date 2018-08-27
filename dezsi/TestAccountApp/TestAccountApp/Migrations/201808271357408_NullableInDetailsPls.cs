namespace TestAccountApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableInDetailsPls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUserDetails", "DOB", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUserDetails", "DOB", c => c.DateTime(nullable: false));
        }
    }
}
