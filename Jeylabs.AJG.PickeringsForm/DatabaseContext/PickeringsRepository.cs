using Jeylabs.AJG.PickeringsForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Jeylabs.AJG.PickeringsForm.Helpers;
using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using System.IO;

namespace Jeylabs.AJG.PickeringsForm.DatabaseContext
{
    /// <summary>
    /// Pickerings Repository
    /// </summary>
    [ExceptionHandler]
    public class PickeringsRepository
    {
        /// <summary>
        /// Method to store claim advice form details
        /// </summary>
        /// <param name="claimAdvice">Claim form details</param>
        /// <param name="filePaths">Temporary file path list</param>
        /// <param name="databaseRecoveryPath"> Database recovery path</param>
        /// <returns> Returns stored information with reference No</returns>
        [ExceptionHandler]
        public static ClaimAdvice StoreClaimAdvice(ClaimAdvice claimAdvice, List<string> filePaths, string databaseRecoveryPath)
        {
            try
            {
                int isSaved;
                using (var context = new PickeringsContext())
                {
                    context.ClaimAdvices.Add(claimAdvice);
                    isSaved = context.SaveChanges();
                    claimAdvice.ReferenceNumber = "PCAF" + claimAdvice.id;
                    context.SaveChanges();
                }

                if (isSaved == 0)
                {
                    WriteCustomErrorLog.WriteLog("An error occured while saving claim advice - Email-"+ claimAdvice.ContactEmail.ToString());
                    CsvGenerator.SaveAsCsV(claimAdvice, @databaseRecoveryPath, filePaths, true);
                }

            }
            catch (Exception ex)
            {
                WriteCustomErrorLog.WriteLog("An error occured while saving claim advice - Email-" + claimAdvice.ContactEmail.ToString());
                CsvGenerator.SaveAsCsV(claimAdvice, @databaseRecoveryPath, filePaths, true);

                if (filePaths.Count > 0)
                {
                    string serverPath = Path.GetDirectoryName(filePaths[0]);
                    new FileHandler().DeleteFiles(serverPath);
                }

                throw new Exception(ex.Message);
            }

            return claimAdvice;

        }
        
        /// <summary>
        /// Get all insured details to bind the dropdownlist
        /// </summary>
        /// <returns></returns>
        [ExceptionHandler]
        public List<InsuredDropDown> GetAllInsured()
        {
            List<InsuredDropDown> InsuredDropDowns = new List<InsuredDropDown>();
            try
            {
                using (PickeringsContext context = new PickeringsContext())
                {
                    InsuredDropDowns = context.InsuredDropDowns.ToList<InsuredDropDown>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return InsuredDropDowns;
        }

        /// <summary>
        /// Get Configuration Settings from the database
        /// </summary>
        /// <returns></returns>

        [ExceptionHandler]
        public static IDictionary<string, string> GetConfigurationSetting()
        {
            IDictionary<string, string> Configurations = new Dictionary<string, string>();
            try
            {
                using (var context = new PickeringsContext())
                {
                    var ConfigurationSetting = context.ConfigurationSettings.ToList<ConfigurationSetting>();
                    Configurations.Add("PickeringsEmailID", ConfigurationSetting.Where(p => p.Name == "PickeringsEmailID").FirstOrDefault().Value);
                    Configurations.Add("PickeringsEmailPassword", ConfigurationSetting.Where(p => p.Name == "PickeringsEmailPassword").FirstOrDefault().Value);
                    Configurations.Add("ToAddress", ConfigurationSetting.Where(p => p.Name == "ToAJGEmailAddress").FirstOrDefault().Value);
                    Configurations.Add("SmtpClient", ConfigurationSetting.Where(p => p.Name == "SmtpClient").FirstOrDefault().Value);
                    Configurations.Add("PortId", ConfigurationSetting.Where(p => p.Name == "PortId").FirstOrDefault().Value);
                    Configurations.Add("EmailToAJGrecoveryPath", ConfigurationSetting.Where(p => p.Name == "EmailToAJGrecoveryPath").FirstOrDefault().Value);
                    Configurations.Add("EmailToInitiatorRecoveryPath", ConfigurationSetting.Where(p => p.Name == "EmailToInitiatorRecoveryPath").FirstOrDefault().Value);
                    Configurations.Add("DatabaseRecoveryPath", ConfigurationSetting.Where(p => p.Name == "DatabaseRecoveryPath").FirstOrDefault().Value);
                }
            }

            catch (Exception ex)
            {
                WriteCustomErrorLog.WriteLog("Database error while fetching configuration settings");
                throw new Exception(ex.Message);
            }

            return Configurations;
        }
    }
}