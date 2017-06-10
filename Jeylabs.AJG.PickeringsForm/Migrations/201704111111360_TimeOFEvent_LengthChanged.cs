namespace Jeylabs.AJG.PickeringsForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeOFEvent_LengthChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("pic.ClaimAdvice", "TimeofDay", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("pic.ClaimAdvice", "TimeofDay", c => c.String(maxLength: 5));
        }
    }
}
