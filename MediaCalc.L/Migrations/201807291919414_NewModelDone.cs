namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModelDone : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalPeriods", "LeaseId", "dbo.Leases");
            DropForeignKey("dbo.RentalPeriods", "UserId", "dbo.Users");
            DropIndex("dbo.RentalPeriods", new[] { "UserId" });
            DropIndex("dbo.RentalPeriods", new[] { "LeaseId" });
            CreateTable(
                "dbo.ConstFees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        RentValue = c.Double(nullable: false),
                        Internet = c.Double(nullable: false),
                        Tv = c.Double(nullable: false),
                        Gas = c.Double(nullable: false),
                        Garbage = c.Double(nullable: false),
                        HotWaterValueForUnit = c.Double(nullable: false),
                        ColdWaterValueForUnit = c.Double(nullable: false),
                        EnergyValueForUnit = c.Double(nullable: false),
                        FlatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .ForeignKey("dbo.Leases", t => t.FlatId, cascadeDelete: true)
                .Index(t => t.FlatId);
            
            AddColumn("dbo.Flats", "MaxUsers", c => c.Int(nullable: false));
            AddColumn("dbo.Leases", "From", c => c.DateTime(nullable: false));
            AddColumn("dbo.Leases", "To", c => c.DateTime(nullable: false));
            AddColumn("dbo.Leases", "CounterStartHotWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterStartColdWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterStartEnergy", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterMiddleHotWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterMiddleColdWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterMiddleEnergy", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterEndHotWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterEndColdWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "CounterEndEnergy", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "VariablesSum", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "ConstSum", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Leases", "UserId");
            AddForeignKey("dbo.Leases", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Leases", "Month");
            DropColumn("dbo.Leases", "Year");
            DropColumn("dbo.Leases", "RentValue");
            DropColumn("dbo.Leases", "Internet");
            DropColumn("dbo.Leases", "Tv");
            DropColumn("dbo.Leases", "Garbage");
            DropColumn("dbo.Leases", "Gas");
            DropColumn("dbo.Leases", "MaxUsers");
            DropColumn("dbo.Leases", "HotWater");
            DropColumn("dbo.Leases", "ColdWater");
            DropColumn("dbo.Leases", "Energy");
            DropColumn("dbo.Users", "Account");
            DropTable("dbo.RentalPeriods");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RentalPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        LeaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Account", c => c.String());
            AddColumn("dbo.Leases", "Energy", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "ColdWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "HotWater", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "MaxUsers", c => c.Int(nullable: false));
            AddColumn("dbo.Leases", "Gas", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "Garbage", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "Tv", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "Internet", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "RentValue", c => c.Double(nullable: false));
            AddColumn("dbo.Leases", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Leases", "Month", c => c.Int(nullable: false));
            DropForeignKey("dbo.ConstFees", "FlatId", "dbo.Leases");
            DropForeignKey("dbo.Leases", "UserId", "dbo.Users");
            DropForeignKey("dbo.ConstFees", "FlatId", "dbo.Flats");
            DropIndex("dbo.Leases", new[] { "UserId" });
            DropIndex("dbo.ConstFees", new[] { "FlatId" });
            DropColumn("dbo.Leases", "UserId");
            DropColumn("dbo.Leases", "ConstSum");
            DropColumn("dbo.Leases", "VariablesSum");
            DropColumn("dbo.Leases", "CounterEndEnergy");
            DropColumn("dbo.Leases", "CounterEndColdWater");
            DropColumn("dbo.Leases", "CounterEndHotWater");
            DropColumn("dbo.Leases", "CounterMiddleEnergy");
            DropColumn("dbo.Leases", "CounterMiddleColdWater");
            DropColumn("dbo.Leases", "CounterMiddleHotWater");
            DropColumn("dbo.Leases", "CounterStartEnergy");
            DropColumn("dbo.Leases", "CounterStartColdWater");
            DropColumn("dbo.Leases", "CounterStartHotWater");
            DropColumn("dbo.Leases", "To");
            DropColumn("dbo.Leases", "From");
            DropColumn("dbo.Flats", "MaxUsers");
            DropTable("dbo.ConstFees");
            CreateIndex("dbo.RentalPeriods", "LeaseId");
            CreateIndex("dbo.RentalPeriods", "UserId");
            AddForeignKey("dbo.RentalPeriods", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RentalPeriods", "LeaseId", "dbo.Leases", "Id", cascadeDelete: true);
        }
    }
}
