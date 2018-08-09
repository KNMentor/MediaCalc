namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewClassesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        RentValue = c.Double(nullable: false),
                        Internet = c.Double(nullable: false),
                        Tv = c.Double(nullable: false),
                        Garbage = c.Double(nullable: false),
                        Gas = c.Double(nullable: false),
                        MaxUsers = c.Int(nullable: false),
                        HotWater = c.Double(nullable: false),
                        ColdWater = c.Double(nullable: false),
                        Energy = c.Double(nullable: false),
                        FlatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flats", t => t.FlatId, cascadeDelete: true)
                .Index(t => t.FlatId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leases", t => t.LeaseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.LeaseId);
            
            AddColumn("dbo.Users", "Phone", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Flats", "Street", c => c.String(nullable: false));
            AlterColumn("dbo.Flats", "HomeNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Surname", c => c.String(nullable: false));
            DropColumn("dbo.Flats", "Rent");
            DropColumn("dbo.Flats", "Internet");
            DropColumn("dbo.Flats", "Tv");
            DropColumn("dbo.Flats", "Garbage");
            DropColumn("dbo.Flats", "Gas");
            DropColumn("dbo.Flats", "MaxUsers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Flats", "MaxUsers", c => c.Int(nullable: false));
            AddColumn("dbo.Flats", "Gas", c => c.Double(nullable: false));
            AddColumn("dbo.Flats", "Garbage", c => c.Double(nullable: false));
            AddColumn("dbo.Flats", "Tv", c => c.Double(nullable: false));
            AddColumn("dbo.Flats", "Internet", c => c.Double(nullable: false));
            AddColumn("dbo.Flats", "Rent", c => c.Double(nullable: false));
            DropForeignKey("dbo.RentalPeriods", "UserId", "dbo.Users");
            DropForeignKey("dbo.RentalPeriods", "LeaseId", "dbo.Leases");
            DropForeignKey("dbo.Leases", "FlatId", "dbo.Flats");
            DropIndex("dbo.RentalPeriods", new[] { "LeaseId" });
            DropIndex("dbo.RentalPeriods", new[] { "UserId" });
            DropIndex("dbo.Leases", new[] { "FlatId" });
            AlterColumn("dbo.Users", "Surname", c => c.String());
            AlterColumn("dbo.Users", "Name", c => c.String());
            AlterColumn("dbo.Flats", "HomeNumber", c => c.String());
            AlterColumn("dbo.Flats", "Street", c => c.String());
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Phone");
            DropTable("dbo.RentalPeriods");
            DropTable("dbo.Leases");
        }
    }
}
