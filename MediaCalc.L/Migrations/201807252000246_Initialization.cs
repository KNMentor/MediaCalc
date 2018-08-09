namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Street = c.String(),
                        HomeNumber = c.String(),
                        RoomNumber = c.String(),
                        Rent = c.Double(nullable: false),
                        Internet = c.Double(nullable: false),
                        Tv = c.Double(nullable: false),
                        Garbage = c.Double(nullable: false),
                        Gas = c.Double(nullable: false),
                        MaxUsers = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Pesel = c.String(),
                        Account = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Flats");
        }
    }
}
