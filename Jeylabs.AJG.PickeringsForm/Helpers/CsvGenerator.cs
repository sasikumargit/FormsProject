using CsvHelper;
using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using Jeylabs.AJG.PickeringsForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Helpers
{
    /// <summary>
    /// Class to generate csv file 
    /// </summary>
    [ExceptionHandler]
    public class CsvGenerator
    {
    /// <summary>
    /// Class to Save CSV file to defined path
    /// </summary>
    /// <param name="claimAdvice"> Claim Advice form details</param>
    /// <param name="backupLocation"> Backup Location</param>
    /// <param name="filePaths"> File path</param>
    /// <param name="isDbError"> flag to check database error</param>
        [ExceptionHandler]
        public static void SaveAsCsV(ClaimAdvice claimAdvice,string backupLocation,List<string>filePaths, bool isDbError=false)
        {
            string path = backupLocation + "//" +" "+ ((isDbError) ? claimAdvice.ContactEmail + "  " + claimAdvice.CreationDate.ToString("dd_MM_yyyy_hh_mm_ss") : claimAdvice.ReferenceNumber);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (string filePath in filePaths)
            {
                string FileName = Path.GetFileName(filePath);
                System.IO.File.Copy(filePath, path + "//" + FileName, true);
            }
            TextWriter textWriter = new StreamWriter(path+ "//" + "Data.csv");
            var csv = new CsvWriter(textWriter);
            csv.WriteField("ReferenceNumber");
            csv.WriteField(claimAdvice.ReferenceNumber);
            csv.NextRecord();
            csv.WriteField("Insured Business Name");
            csv.WriteField(claimAdvice.InsuredBusinessName);
            csv.NextRecord();
            csv.WriteField("Insured ABN");
            csv.WriteField(claimAdvice.InsuredABN);
            csv.NextRecord();
            csv.WriteField("Insured Business Address");
            csv.WriteField(claimAdvice.InsuredBusinessAddress);
            csv.NextRecord();
            csv.WriteField("Contact Name");
            csv.WriteField(claimAdvice.ContactName);
            csv.NextRecord();
            csv.WriteField("Contact Email");
            csv.WriteField(claimAdvice.ContactEmail);
            csv.NextRecord();
            csv.WriteField("Date of Event");
            csv.WriteField(claimAdvice.DateofEvent);
            csv.NextRecord();
            csv.WriteField("Time of Day");
            csv.WriteField(claimAdvice.TimeofDay);
            csv.NextRecord();
            csv.WriteField("Where did the event occur?");
            csv.WriteField(claimAdvice.EventAddress);
            csv.NextRecord();
            csv.WriteField("How did the loss or damage occur? Please describe the damage.");
            csv.WriteField(claimAdvice.HowLossDescription);
            csv.NextRecord();
            csv.NextRecord();
            csv.WriteField("Is any Third Party to blame for the loss or damage?");
            csv.WriteField(claimAdvice.ThirdPartyBlame);
            csv.NextRecord();
            foreach (var item in claimAdvice.ThirdPartyBlames)
            {
                csv.NextRecord();
                csv.WriteField("Third Party Details ("+(claimAdvice.ThirdPartyBlames.IndexOf(item)+1).ToString()+")");
                csv.NextRecord();
                csv.WriteField("Name");
                csv.WriteField(item.Name);
                csv.NextRecord();
                csv.WriteField("Registration");
                csv.WriteField(item.Registration);
                csv.NextRecord();
                csv.WriteField("Address");
                csv.WriteField(item.Address);
                csv.NextRecord();
                csv.WriteField("Phone");
                csv.WriteField(item.Phone);
                csv.NextRecord();
            }
            csv.NextRecord();
            csv.WriteField("Have you received, or do you anticipate receiving, notice of any claim from or on behalf of third parties?");
            csv.WriteField(claimAdvice.ReceiveNoticeThirdParty);
            csv.NextRecord();
            if (claimAdvice.ReceiveNoticeThirdParty == "Yes")
            {
                csv.WriteField("Please provide details");
                csv.WriteField(claimAdvice.ReceiveNoticeThirdPartyDetails);
                csv.NextRecord();
            }
            foreach (var item in claimAdvice.SceneWitnesses)
            {
                csv.NextRecord();
                csv.WriteField("Witnesses at Scene(" + (claimAdvice.SceneWitnesses.IndexOf(item) + 1).ToString() + ")");
                csv.NextRecord();
                csv.WriteField("Name");
                csv.WriteField(item.Name);
                csv.NextRecord();
                csv.WriteField("Registration");
                csv.WriteField(item.Registration);
                csv.NextRecord();
                csv.WriteField("Address");
                csv.WriteField(item.Address);
                csv.NextRecord();
                csv.WriteField("Phone");
                csv.WriteField(item.Phone);
                csv.NextRecord();
            }
            csv.NextRecord();
            csv.WriteField("Is the claim for a burglary or theft?");
            csv.WriteField(claimAdvice.BurglaryorTheft);
            if (claimAdvice.BurglaryorTheft == "Yes")
            {
                csv.NextRecord();
                csv.WriteField("Police Station");
                csv.WriteField(claimAdvice.PoliceStation);
                csv.NextRecord();
                csv.WriteField("Police Officer Name");
                csv.WriteField(claimAdvice.PoliceOfficerName);
                csv.NextRecord();
                csv.WriteField("Police Report Number");
                csv.WriteField(claimAdvice.PoliceReportNumber);
            }
            csv.NextRecord();
            foreach (var item in claimAdvice.Claims)
            {
                csv.NextRecord();
                csv.WriteField("Claim Details (" + (claimAdvice.Claims.IndexOf(item) + 1).ToString() + ")");
                csv.NextRecord();
                csv.WriteField("Description of lost and/or damaged Property");
                csv.WriteField(item.PropertyDescription);
                csv.NextRecord();
                csv.WriteField("Item Age");
                csv.WriteField(item.ItemAge);
                csv.NextRecord();
                csv.WriteField("Original Cost");
                csv.WriteField(item.OriginalCost);
                csv.NextRecord();
                csv.WriteField("Replacement/Repair Cost");
                csv.WriteField(item.ReplacementValue);
                csv.NextRecord();
                csv.WriteField("Amount Claimed");
                csv.WriteField(item.AmountClaimed);
                csv.NextRecord();
            }
            csv.NextRecord();
            csv.WriteField("Total amount claimed");
            csv.WriteField(claimAdvice.TotalAmountClaimed);
            csv.NextRecord();
            csv.WriteField("Third Party Liability Claims");
            csv.WriteField(claimAdvice.ThirdPartyLiabilityClaim);
            csv.NextRecord();
            if (claimAdvice.ThirdPartyLiabilityClaim == "Yes")
            {
                csv.WriteField("Name of Third Party");
                csv.WriteField(claimAdvice.ThirdPartyName);
                csv.NextRecord();
                csv.WriteField("Permanent Address");
                csv.WriteField(claimAdvice.ThirdPartyPermanentAddress);
                csv.NextRecord();
                csv.WriteField("Nature and extent of injuries/damage");
                csv.WriteField(claimAdvice.InjuriesDamageDescription);
                csv.NextRecord();
                csv.WriteField("Have you received any correspondence from Third Parties?");
                csv.WriteField(claimAdvice.ThirdPartyCorrespondence);

                csv.NextRecord();
                foreach (var item in claimAdvice.ThirdPartyFileUploads)
                {
                    csv.NextRecord();
                    csv.WriteField("Third Party Correspondence File Upload Link Table (" + (claimAdvice.ThirdPartyFileUploads.IndexOf(item) + 1).ToString() + ")");
                    csv.NextRecord();
                    csv.WriteField("Upload Description");
                    csv.WriteField(item.UploadDescription);
                    csv.NextRecord();
                    csv.WriteField("Upload Description");
                    csv.WriteField(item.UploadFilename);
                    csv.NextRecord();
                }
                csv.NextRecord();
                csv.WriteField("Have you made any admission of liability?");
                csv.WriteField(claimAdvice.AdmissionofLiability);
                csv.NextRecord();
                csv.WriteField("Give details");
                csv.WriteField(claimAdvice.AdmissionofLiabilityDetails);
                csv.NextRecord();
            }
            foreach (var item in claimAdvice.SupportingFileUploads)
            {
                csv.NextRecord();
                csv.WriteField("Third Party Correspondence File Upload Link Table (" + (claimAdvice.SupportingFileUploads.IndexOf(item) + 1).ToString() + ")");
                csv.NextRecord();
                csv.WriteField("Upload Description");
                csv.WriteField(item.UploadDescription);
                csv.NextRecord();
                csv.WriteField("Upload Description");
                csv.WriteField(item.UploadFilename);
                csv.NextRecord();
            }
            csv.NextRecord();
            csv.WriteField("Other Comments");
            csv.WriteField(claimAdvice.OtherComments);
            csv.NextRecord();
            textWriter.Close();
        }
    }
}