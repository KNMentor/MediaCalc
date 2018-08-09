namespace MediaCalc.L.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlatUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flats", "City", c => c.String());
            AddColumn("dbo.Flats", "PostCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Flats", "PostCode");
            DropColumn("dbo.Flats", "City");
        }
    }
}
