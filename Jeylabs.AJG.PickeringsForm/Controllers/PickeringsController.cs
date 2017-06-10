using Jeylabs.AJG.PickeringsForm.DatabaseContext;
using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using Jeylabs.AJG.PickeringsForm.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jeylabs.AJG.PickeringsForm.Helpers;
using Newtonsoft.Json.Converters;
using System.Threading;
using System.Web.UI;

namespace Jeylabs.AJG.PickeringsForm.Controllers
{
    /// <summary>
    /// Pickerings controller class
    /// </summary>
    public class PickeringsController : Controller
    {
        /// <summary>
        /// Returns main View of the form
        /// </summary>
        /// <returns> returns pickering form</returns>    
        [ExceptionHandler]
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Get All Insured Details 
        /// </summary>
        /// <returns> Returns all insured details as Json response</returns>      
        [ExceptionHandler]
        [HttpGet]
        public JsonResult AllInsuredDetails()
        {
            List<InsuredDropDown> insuredDetails;

            try
            {
                insuredDetails = new PickeringsRepository().GetAllInsured();
            }
            catch (Exception ex)
            {
                WriteCustomErrorLog.WriteLog("Database error while fetching all insured details");
                throw new Exception(ex.Message);
            }

            return Json(insuredDetails, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Method to Collect Json data and Files 
        /// </summary>
        /// <param name="formValues"> To collect Json data and Files</param>
        /// <returns> returns reference no and submission date as json response</returns>       
        [ExceptionHandler]
        [HttpPost]
        public JsonResult FormPost(FormCollection formValues)
        {
            string claimsAdviceData = Request.Form["claimsAdvice"];
            var dateformat = "dd/MM/yyyy";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateformat };
            ClaimAdvice claimAdvice = JsonConvert.DeserializeObject<ClaimAdvice>(claimsAdviceData, dateTimeConverter);
            claimAdvice = RemoveEmptyItems(claimAdvice);

            List<HttpPostedFileBase> fileCollection = new List<HttpPostedFileBase>();

            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        fileCollection.Add(fileContent);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteCustomErrorLog.WriteLog("Error occured while adding file collection");
                throw new Exception(ex.Message);
            }

            List<string> attachmentFilesPath = new FileHandler().fileSave(fileCollection, claimAdvice.ContactEmail + DateTime.Now.ToString("ddMMyyhhmmsstt"));
            fileCollection.Clear();

            IDictionary<string, string> Configurations = PickeringsRepository.GetConfigurationSetting();

            ClaimAdvice storedClaimAdvice = PickeringsRepository.StoreClaimAdvice(claimAdvice, attachmentFilesPath, Configurations["DatabaseRecoveryPath"]);

            Thread MailThread = new Thread(new ThreadStart(delegate ()
            {
                SendMail(attachmentFilesPath, storedClaimAdvice, Configurations);
            }));

            MailThread.Start();

            string[] referenceNumber = { claimAdvice.ReferenceNumber, string.Format("{0:dd/MM/yy hh:mm:ss tt}", storedClaimAdvice.CreationDate) };
            return Json(referenceNumber, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Method return a view for form submission confirmation
        /// </summary>
        /// <param name="referenceNumber"> Confirmation Number</param>
        /// <param name="submitedDate"> Submitted Date and Time</param>
        /// <returns></returns>       
        [ExceptionHandler]
        [HttpGet]
        public ActionResult Acknowledgement(string referenceNumber, string submitedDate)
        {
            ViewBag.ReferenceNumber = referenceNumber;
            ViewBag.SubmitedDate = submitedDate;

            if (referenceNumber == "undefined" || submitedDate == "undefined")
            {
                return View("Error");
            }
            return View();
        }

        /// <summary>
        /// Method to send email - ( Initiator and AJG)
        /// </summary>
        /// <param name="attachmentFilesPath"></param>
        /// <param name="claimAdvice"></param>
        [ExceptionHandler]
        public void SendMail(List<string> attachmentFilesPath, ClaimAdvice claimAdvice, IDictionary<string, string> Configurations)
        {
            string[] emailBodys = HtmlTableGenerator.CreateHtmlMail(claimAdvice);
            bool mailSentStatus = EmailSender.SendMail(Configurations, emailBodys[0], "Pickerings Claim Submission – Reference Number " + claimAdvice.ReferenceNumber, attachmentFilesPath);
            if (!mailSentStatus)
            {
                WriteCustomErrorLog.WriteLog("Email failed to AJG - Ref # : " + claimAdvice.ReferenceNumber.ToString());
                CsvGenerator.SaveAsCsV(claimAdvice, @Configurations["EmailToAJGrecoveryPath"], attachmentFilesPath);
            }

            Configurations["ToAddress"] = claimAdvice.ContactEmail;
            mailSentStatus = EmailSender.SendMail(Configurations, emailBodys[1], "Pickerings Claim Submission – Reference Number " + claimAdvice.ReferenceNumber);
            if (!mailSentStatus)
            {
                WriteCustomErrorLog.WriteLog("Email failed to initiator - Ref # : " + claimAdvice.ReferenceNumber.ToString());
                CsvGenerator.SaveAsCsV(claimAdvice, @Configurations["EmailToInitiatorRecoveryPath"], attachmentFilesPath);
            }

            if (attachmentFilesPath.Count > 0)
            {
                string serverPath = Path.GetDirectoryName(attachmentFilesPath[0]);
                new FileHandler().DeleteFiles(serverPath);
            }

        }

        /// <summary>
        /// Method to return error view
        /// </summary>
        /// <returns> error page </returns>
        [ExceptionHandler]
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Method to remove empty or null items from the list - To ensure database consistency
        /// </summary>
        /// <param name="claimAdvice"> Claim form data </param>
        /// <returns></returns>
        [ExceptionHandler]
        public ClaimAdvice RemoveEmptyItems(ClaimAdvice claimAdvice)
        {
            ClaimAdvice objClaimAdvice = new ClaimAdvice();
            objClaimAdvice = claimAdvice;

            try
            {

                objClaimAdvice.SupportingFileUploads.RemoveAll(item => item.UploadFilename == "" || item.UploadFilename == null);
                objClaimAdvice.ThirdPartyFileUploads.RemoveAll(item => item.UploadFilename == "" || item.UploadFilename == null);
                objClaimAdvice.SceneWitnesses.RemoveAll(item => item.Name == "" && item.Address == "" && item.Phone == "" && item.Registration == "");
                objClaimAdvice.ThirdPartyBlames.RemoveAll(item => item.Name == "" && item.Address == "" && item.Phone == "" && item.Registration == "");
                objClaimAdvice.Claims.RemoveAll(item => item.PropertyDescription == "" && item.ItemAge == "");

            }
            catch (Exception ex)
            {
                WriteCustomErrorLog.WriteLog("Error occured while removing empty list items from claim advice");
                throw new Exception(ex.Message);
            }

            return objClaimAdvice;
        }


    }
}
