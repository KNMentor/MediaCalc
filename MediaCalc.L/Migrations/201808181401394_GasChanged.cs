namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GasChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConstFees", "GasValueForUnit", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterStartGas", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterMiddleGas", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterEndGas", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "DeltaEnergyForThisUser", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "DeltaGasForThisUser", c => c.Double(nullable: false));
            DropColumn("dbo.ConstFees", "Gas");
            DropColumn("dbo.Leases", "DeltaEnergyWaterForThisUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leases", "DeltaEnergyWaterForThisUser", c => c.Double(nullable: false));
            AddColumn("dbo.ConstFees", "Gas", c => c.Double(nullable: false));
            DropColumn("dbo.Leases", "DeltaGasForThisUser");
            DropColumn("dbo.Leases", "DeltaEnergyForThisUser");
            DropColumn("dbo.Leases", "CounterEndGas");
            DropColumn("dbo.Leases", "CounterMiddleGas");
            DropColumn("dbo.Leases", "CounterStartGas");
            DropColumn("dbo.ConstFees", "GasValueForUnit");
        }
    }
}
