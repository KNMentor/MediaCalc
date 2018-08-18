namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeasePaidAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leases", "Paid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leases", "Paid");
        }
    }
}
