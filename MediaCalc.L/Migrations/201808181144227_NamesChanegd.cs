namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamesChanegd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leases", "DeltaHotWaterForThisUser", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "DeltaColdWaterForThisUser", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "DeltaEnergyWaterForThisUser", c => c.Double(nullable: false));
            DropColumn("dbo.Leases", "DeltaHotWater");
            DropColumn("dbo.Leases", "DeltaColdWater");
            DropColumn("dbo.Leases", "DeltaEnergyWater");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leases", "DeltaEnergyWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "DeltaColdWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "DeltaHotWater", c => c.Double(nullable: false));
            DropColumn("dbo.Leases", "DeltaEnergyWaterForThisUser");
            DropColumn("dbo.Leases", "DeltaColdWaterForThisUser");
            DropColumn("dbo.Leases", "DeltaHotWaterForThisUser");
        }
    }
}
