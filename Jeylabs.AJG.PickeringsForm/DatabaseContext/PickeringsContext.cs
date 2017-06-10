using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using Jeylabs.AJG.PickeringsForm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.DatabaseContext
{

    /// <summary>
    /// Pickerings database context
    /// </summary>
    [ExceptionHandler]
    public class PickeringsContext : DbContext
    {

        /// <summary>
        /// Pickerings Context Constructor
        /// </summary>
        public PickeringsContext() : base("AJG.ClaimApprovalContext")
        {

        }

        /// <summary>
        /// Claims Advice form
        /// </summary>
        public DbSet<ClaimAdvice> ClaimAdvices { get; set; }
        /// <summary>
        /// Claim Details
        /// </summary>
        public DbSet<Claim> Claims { get; set; }
        /// <summary>
        /// Insured dropdown list
        /// </summary>
        public DbSet<InsuredDropDown> InsuredDropDowns { get; set; }
        /// <summary>
        /// Supporting file uploads
        /// </summary>
        public DbSet<SupportingFileUpload> SupportingFileUploads { get; set; }
        /// <summary>
        /// Scene witnesses
        /// </summary>
        public DbSet<SceneWitness> SceneWitnesses { get; set; }
        /// <summary>
        /// Third party blames
        /// </summary>
        public DbSet<ThirdPartyBlame> ThirdPartyBlames { get; set; }
        /// <summary>
        /// Third Party Uploads
        /// </summary>
        public DbSet<ThirdPartyFileUpload> ThirdPartyFileUploads { get; set; }
        /// <summary>
        /// Configuration Settings
        /// </summary>
        public DbSet<ConfigurationSetting> ConfigurationSettings { get; set; }
    }
}