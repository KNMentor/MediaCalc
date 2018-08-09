namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
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
                .Index(t => t.FlatId);
            
            CreateTable(
                "dbo.Flats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Street = c.String(),
                        HomeNumber = c.String(),
                        RoomNumber = c.String(),
                        MaxUsers = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Leases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        CounterStartHotWater = c.Double(nullable: false),
                        CounterStartColdWater = c.Double(nullable: false),
                        CounterStartEnergy = c.Double(nullable: false),
                        CounterMiddleHotWater = c.Double(nullable: false),
                        CounterMiddleColdWater = c.Double(nullable: false),
                        CounterMiddleEnergy = c.Double(nullable: false),
                        CounterEndHotWater = c.Double(nullable: false),
                        CounterEndColdWater = c.Double(nullable: false),
                        CounterEndEnergy = c.Double(nullable: false),
                        DeltaHotWater = c.Double(nullable: false),
                        DeltaColdWater = c.Double(nullable: false),
                        DeltaEnergyWater = c.Double(nullable: false),
                        VariablesSum = c.Double(nullable: false),
                        ConstSum = c.Double(nullable: false),
                        FlatsId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatsId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.FlatsId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Pesel = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConstFees", "FlatId", "dbo.Flats");
            DropForeignKey("dbo.Leases", "UserId", "dbo.Users");
            DropForeignKey("dbo.Leases", "FlatsId", "dbo.Flats");
            DropIndex("dbo.Leases", new[] { "UserId" });
            DropIndex("dbo.Leases", new[] { "FlatsId" });
            DropIndex("dbo.ConstFees", new[] { "FlatId" });
            DropTable("dbo.Users");
            DropTable("dbo.Leases");
            DropTable("dbo.Flats");
            DropTable("dbo.ConstFees");
        }
    }
}
