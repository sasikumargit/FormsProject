namespace Jeylabs.AJG.PickeringsForm.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Jeylabs.AJG.PickeringsForm.DatabaseContext.PickeringsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(Jeylabs.AJG.PickeringsForm.DatabaseContext.PickeringsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.ConfigurationSettings.AddOrUpdate(
            //  p => p.Name,
            //  new Models.ConfigurationSetting { Name = "EmailToAJGrecoveryPath", Value = @"d:\Pickerings Claim Advice Forms\Send AJG email error recovery\" },
            //  new Models.ConfigurationSetting { Name = "EmailToInitiatorRecoveryPath", Value = @"d:\Pickerings Claim Advice Forms\Send Initiator email error recovery\" },
            //  new Models.ConfigurationSetting { Name = "DatabaseRecoveryPath", Value = @"d:\Pickerings Claim Advice Forms\Database error recovery\" }
            //);
            //
        }
    }
}
