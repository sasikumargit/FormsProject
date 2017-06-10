namespace Jeylabs.AJG.PickeringsForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class length_registration_field : DbMigration
    {
        public override void Up()
        {
            AlterColumn("pic.SceneWitness", "Registration", c => c.String(maxLength: 7));
            AlterColumn("pic.ThirdPartyBlame", "Registration", c => c.String(maxLength: 7));
        }
        
        public override void Down()
        {
            AlterColumn("pic.ThirdPartyBlame", "Registration", c => c.String(maxLength: 6));
            AlterColumn("pic.SceneWitness", "Registration", c => c.String(maxLength: 6));
        }
    }
}
