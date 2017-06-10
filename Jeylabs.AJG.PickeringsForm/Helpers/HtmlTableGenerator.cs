using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using Jeylabs.AJG.PickeringsForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jeylabs.AJG.PickeringsForm.Helpers
{
    /// <summary>
    /// Class to generate html table
    /// </summary>
    [ExceptionHandler]
    public static class HtmlTableGenerator
    {
        /// <summary>
        /// Method to generate HTML mail template
        /// </summary>
        /// <param name="claimAdvice">Claim Advice form details</param>
        /// <returns></returns>
        [ExceptionHandler]
        public static string[] CreateHtmlMail(ClaimAdvice claimAdvice)
        {
            string htmlBodyTOAJG, htmlBodyTOCustomer = string.Empty;
            try
            {
                string TableBodyForAJG = AddTable(claimAdvice, "AJG");
                htmlBodyTOAJG = new FileHandler().ReadFile(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\PickeringEmailTemplateToAJG.html");
                htmlBodyTOAJG = htmlBodyTOAJG.Replace("####DateTime####", claimAdvice.CreationDate.ToString());
                htmlBodyTOAJG = htmlBodyTOAJG.Replace("####TableBody####", TableBodyForAJG);

                htmlBodyTOCustomer = new FileHandler().ReadFile(AppDomain.CurrentDomain.BaseDirectory + "App_Data\\PiceringEmailTemplateToCustomer.html");
                string TableBodyForCus = AddTable(claimAdvice, "Cus");
                htmlBodyTOCustomer = htmlBodyTOCustomer.Replace("####DateTime####", claimAdvice.CreationDate.ToString());
                htmlBodyTOCustomer = htmlBodyTOCustomer.Replace("####ContactName####", claimAdvice.ContactName);
                htmlBodyTOCustomer = htmlBodyTOCustomer.Replace("####TableBody####", TableBodyForCus);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new string[] { htmlBodyTOAJG, htmlBodyTOCustomer };
        }

        /// <summary>
        /// Method to add table
        /// </summary>
        /// <param name="claimAdvice"></param>
        /// <param name="toWhom"></param>
        /// <returns></returns>
        [ExceptionHandler]
        public static string AddTable(ClaimAdvice claimAdvice, string toWhom)
        {
            using (StringWriter sw = new StringWriter())
            {
                Table t = new Table();
                t.CellPadding = 8;
                t.CellSpacing = 0;
                t.BorderWidth = 2;
                t.Font.Name = "Arial";
                if (claimAdvice.ReferenceNumber != null)
                {
                    t.Rows.Add(GetRow("Reference Number", claimAdvice.ReferenceNumber));
                }
                if (claimAdvice.InsuredBusinessName != null)
                {
                    t.Rows.Add(GetRow("Insured Business Name", claimAdvice.InsuredBusinessName));
                }
                if (claimAdvice.InsuredABN != null)
                {
                    t.Rows.Add(GetRow("Insured ABN", claimAdvice.InsuredABN));
                }
                if (claimAdvice.InsuredBusinessAddress != null)
                {
                    t.Rows.Add(GetRow("Insured Business Address", claimAdvice.InsuredBusinessAddress));
                }
                if (claimAdvice.PolicyNumber != null && toWhom == "AJG")
                {
                    t.Rows.Add(GetRow("Policy Number", claimAdvice.PolicyNumber));
                }
                if (claimAdvice.ContactName != null)
                {
                    t.Rows.Add(GetRow("Contact Name", claimAdvice.ContactName));
                }
                if (claimAdvice.ContactEmail != null)
                {
                    t.Rows.Add(GetRow("Contact Email", claimAdvice.ContactEmail));
                }
                if (claimAdvice.DateofEvent.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    t.Rows.Add(GetRow("Date of Event", claimAdvice.DateofEvent.ToString("dd/MM/yyyy")));
                }
                if (claimAdvice.TimeofDay != null)
                {
                    t.Rows.Add(GetRow("Time of Day", claimAdvice.TimeofDay));
                }
                if (claimAdvice.EventAddress != null)
                {
                    t.Rows.Add(GetRow("Where did the event occur?", claimAdvice.EventAddress));
                }
                if (claimAdvice.HowLossDescription != null)
                {
                    t.Rows.Add(GetRow("How did the loss or damage occur?", claimAdvice.HowLossDescription));
                }
                t.RenderControl(new HtmlTextWriter(sw));


                string html = sw.ToString();
                html += "<br/>";
                Table t2 = new Table();
                t2.CellPadding = 8;
                t2.CellSpacing = 0;
                t2.BorderWidth = 2;
                t2.Font.Name = "Arial";
                t2.Rows.Add(GetRow("Is any Third Party to blame for the loss or damage? ", claimAdvice.ThirdPartyBlame));
                StringWriter sw2 = new StringWriter();
                t2.RenderControl(new HtmlTextWriter(sw2));
                html += sw2.ToString();
                html += "<br/>";
                if (claimAdvice.ThirdPartyBlame == "Yes")
                {
                    html += CreateTablesList(claimAdvice.ThirdPartyBlames);
                    html += "<br/>";
                }
                Table t3 = new Table();
                t3.CellPadding = 8;
                t3.CellSpacing = 0;
                t3.BorderWidth = 2;
                t3.Font.Name = "Arial";
                t3.Rows.Add(GetRow("Have you received, or do you anticipate receiving, notice of any claim from or on behalf of third parties? ", claimAdvice.ReceiveNoticeThirdParty));
                if (claimAdvice.ReceiveNoticeThirdParty == "Yes")
                    t3.Rows.Add(GetRow("Please provide details ", claimAdvice.ReceiveNoticeThirdPartyDetails));
                StringWriter sw3 = new StringWriter();
                t3.RenderControl(new HtmlTextWriter(sw3));
                html += sw3.ToString();
                html += "<br/>";
                Table t4 = new Table();
                t4.CellPadding = 8;
                t4.CellSpacing = 0;
                t4.BorderWidth = 2;
                t4.Font.Name = "Arial";
                t4.Rows.Add(GetRow("Any witnesses at scene?", claimAdvice.WitnessesatScene));
                StringWriter sw4 = new StringWriter();
                t4.RenderControl(new HtmlTextWriter(sw4));
                html += sw4.ToString();
                html += "<br/>";
                if (claimAdvice.WitnessesatScene == "Yes")
                {
                    html += CreateTablesList(claimAdvice.SceneWitnesses);
                    html += "<br/>";
                }
                html += "<br/>";
                Table t5 = new Table();
                t5.CellPadding = 8;
                t5.CellSpacing = 0;
                t5.BorderWidth = 2;
                t5.Font.Name = "Arial";
                t5.Rows.Add(GetRow("Is the claim for a burglary or theft?", claimAdvice.BurglaryorTheft));
                if (claimAdvice.BurglaryorTheft == "Yes")
                {
                    t5.Rows.Add(GetRow("Police Station", claimAdvice.PoliceStation));
                    t5.Rows.Add(GetRow("Police Officer Name", claimAdvice.PoliceOfficerName));
                    t5.Rows.Add(GetRow("Police Report Number", claimAdvice.PoliceReportNumber));
                }

                StringWriter sw5 = new StringWriter();
                t5.RenderControl(new HtmlTextWriter(sw5));

                html += sw5.ToString();
                html += "<br/>";
                html += CreateTablesList(claimAdvice.Claims);
                html += "<br/>";
                Table t6 = new Table();
                t6.CellPadding = 8;
                t6.CellSpacing = 0;
                t6.BorderWidth = 2;
                t6.Font.Name = "Arial";
                t6.Rows.Add(GetRow("Total Amount Claimed", "AUD$" + claimAdvice.TotalAmountClaimed.ToString()));
                StringWriter sw6 = new StringWriter();
                t6.RenderControl(new HtmlTextWriter(sw6));
                html += "<br/>";
                html += sw6.ToString();
                html += "<br/>";

                Table t6_1 = new Table();
                t6_1.CellPadding = 8;
                t6_1.CellSpacing = 0;
                t6_1.BorderWidth = 2;
                t6_1.Font.Name = "Arial";
                t6_1.Rows.Add(GetRow("Is this a Third Party Liability claim? ", claimAdvice.ThirdPartyLiabilityClaim));
                StringWriter sw6_1 = new StringWriter();
                t6_1.RenderControl(new HtmlTextWriter(sw6_1));
                html += "<br/>";
                html += sw6_1.ToString();
                if (claimAdvice.ThirdPartyLiabilityClaim == "Yes")
                {
                    html += "<br/>";
                    html += CreateTable(claimAdvice.ThirdPartyName, claimAdvice.ThirdPartyPermanentAddress, claimAdvice.InjuriesDamageDescription, claimAdvice.ThirdPartyCorrespondence);
                    html += "<br/>";
                    if (claimAdvice.ThirdPartyCorrespondence == "Yes")
                    {
                        html += CreateTablesList(claimAdvice.ThirdPartyFileUploads);
                        html += "<br/>";
                    }

                    html += "<br/>";
                    Table t7 = new Table();
                    t7.CellPadding = 8;
                    t7.CellSpacing = 0;
                    t7.BorderWidth = 2;
                    t7.Font.Name = "Arial";
                    t7.Rows.Add(GetRow("Have you made any admission of liability? ", claimAdvice.AdmissionofLiability));
                    if (claimAdvice.AdmissionofLiability == "Yes")
                    {
                        t7.Rows.Add(GetRow("Give details", claimAdvice.AdmissionofLiabilityDetails));
                        StringWriter sw7 = new StringWriter();
                        t7.RenderControl(new HtmlTextWriter(sw7));
                        html += "<br/>";
                        html += sw7.ToString();
                    }
                }
                html += "<br/>";
                html += CreateTablesList(claimAdvice.SupportingFileUploads);
                html += "<br/>";
                if (claimAdvice.OtherComments != null)
                {
                    Table t8 = new Table();
                    t8.CellPadding = 8;
                    t8.CellSpacing = 0;
                    t8.BorderWidth = 2;
                    t8.Font.Name = "Arial";
                    t8.Rows.Add(GetRow("Other Comments", claimAdvice.OtherComments));
                    StringWriter sw8 = new StringWriter();
                    t8.RenderControl(new HtmlTextWriter(sw8));
                    html += "<br/>";
                    html += sw8.ToString();
                }
                html += "<br/>";
                return html;
            }

        }

        /// <summary>
        /// Method to get row
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static TableRow GetRow(string fieldName, string value)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell
            {
                Text = "<strong>" + fieldName + "</strong>",
                Wrap = true,
                BorderColor = System.Drawing.Color.Black,
                BorderStyle = BorderStyle.Solid,
                BorderWidth = 1,

                Width = 217
            };
            td.Font.Bold = true;
            td.Font.Size = 12;
            TableCell td2 = new TableCell
            {
                Text = "<p class='TableText'>&nbsp;" + (String.IsNullOrWhiteSpace(value) ? "    " : value) + "</p>",
                Wrap = true,
                BorderColor = System.Drawing.Color.Black,
                BorderStyle = BorderStyle.Solid,
                BorderWidth = 1,
                Width = 435
            };

            tr.Cells.Add(td);
            tr.Cells.Add(td2);
            return tr;
        }

        /// <summary>
        /// Method to create table list details for third party blames
        /// </summary>
        /// <param name="thirdpartyBlames"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static string CreateTablesList(List<ThirdPartyBlame> thirdpartyBlames)
        {
            string html = "";
            foreach (var item in thirdpartyBlames)
            {
                html += "<h3 style=\"font-family:Arial;\">Third Party Details (" + (thirdpartyBlames.IndexOf(item) + 1).ToString() + ")</h3>";
                using (StringWriter sw = new StringWriter())
                {

                    Table t = new Table();
                    t.CellPadding = 8;
                    t.CellSpacing = 0;
                    t.BorderWidth = 2;
                    t.Font.Name = "Arial";
                    if (item.Name != null)
                    {
                        t.Rows.Add(GetRow("Name", item.Name));
                    }
                    if (item.Registration != null)
                    {
                        t.Rows.Add(GetRow("Registration", item.Registration));
                    }
                    if (item.Address != null)
                    {
                        t.Rows.Add(GetRow("Address", item.Address));
                    }
                    if (item.Phone != null)
                    {
                        t.Rows.Add(GetRow("Phone", item.Phone));
                    }
                    t.RenderControl(new HtmlTextWriter(sw));
                    html += "<br/>" + sw.ToString();
                }
            }
            return html;
        }

        /// <summary>
        /// Method to create table for scene witness
        /// </summary>
        /// <param name="sceneWitnesses"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static string CreateTablesList(List<SceneWitness> sceneWitnesses)
        {
            string html = "";
            foreach (var item in sceneWitnesses)
            {
                html += "<h3 style=\"font-family:Arial;\">Witnesses at Scene (" + (sceneWitnesses.IndexOf(item) + 1).ToString() + ")</h3>";
                using (StringWriter sw = new StringWriter())
                {
                    Table t = new Table();
                    t.CellPadding = 8;
                    t.CellSpacing = 0;
                    t.BorderWidth = 2;
                    t.Font.Name = "Arial";
                    if (item.Name != null)
                    {
                        t.Rows.Add(GetRow("Name", item.Name));
                    }
                    if (item.Registration != null)
                    {
                        t.Rows.Add(GetRow("Registration", item.Registration));
                    }
                    if (item.Address != null)
                    {
                        t.Rows.Add(GetRow("Address", item.Address));
                    }
                    if (item.Phone != null)
                    {
                        t.Rows.Add(GetRow("Phone", item.Phone));
                    }
                    t.RenderControl(new HtmlTextWriter(sw));
                    html += "<br/>" + sw.ToString();
                }
            }
            return html;
        }

        /// <summary>
        /// Method to create table for claim details
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static string CreateTablesList(List<Claim> claims)
        {
            string html = "";
            foreach (var item in claims)
            {
                html += "<h3 style=\"font-family:Arial;\">Claim Details (" + (claims.IndexOf(item) + 1).ToString() + ")</h3>";
                using (StringWriter sw = new StringWriter())
                {
                    Table t = new Table();
                    t.CellPadding = 8;
                    t.CellSpacing = 0;
                    t.BorderWidth = 2;
                    t.Font.Name = "Arial";
                    if (item.PropertyDescription != null)
                    {
                        t.Rows.Add(GetRow("Description of lost and / or damaged Property", item.PropertyDescription));
                    }
                    if (item.ItemAge != null)
                    {
                        t.Rows.Add(GetRow("Item Age", item.ItemAge));
                    }
                    if (item.OriginalCost != 0)
                    {
                        t.Rows.Add(GetRow("Original Cost", "AUD$" + item.OriginalCost.ToString()));
                    }
                    if (item.ReplacementValue != 0)
                    {
                        t.Rows.Add(GetRow("Replacement/Repair Cost", "AUD$" + item.ReplacementValue.ToString()));
                    }
                    if (item.AmountClaimed != 0)
                    {
                        t.Rows.Add(GetRow("Amount Claimed", "AUD$" + item.AmountClaimed.ToString()));
                    }
                    t.RenderControl(new HtmlTextWriter(sw));
                    html += "<br/>" + sw.ToString();
                }
            }
            return html;
        }

        /// <summary>
        /// Method to create table for third party name, address
        /// </summary>
        /// <param name="nameofThirdParty"></param>
        /// <param name="permanentAddress"></param>
        /// <param name="injuries"></param>
        /// <param name="anyCorrespondence"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static string CreateTable(string nameofThirdParty, string permanentAddress, string injuries, string anyCorrespondence)
        {
            string html = null;
            using (StringWriter sw = new StringWriter())
            {
                html = "<h3 style=\"font-family:Arial;\">Third Party Liability Claims</h3>";
                Table t = new Table();
                t.CellPadding = 8;
                t.CellSpacing = 0;
                t.BorderWidth = 2;
                t.Font.Name = "Arial";
                if (nameofThirdParty != null)
                {
                    t.Rows.Add(GetRow("Name of Third Party", nameofThirdParty));
                }
                if (permanentAddress != null)
                {
                    t.Rows.Add(GetRow("Permanent Address", permanentAddress));
                }
                if (injuries != null)
                {
                    t.Rows.Add(GetRow("Nature and extent of injuries/damage", injuries));
                }
                if (anyCorrespondence != null)
                {
                    t.Rows.Add(GetRow("Have you received any correspondence from Third Parties?", anyCorrespondence));
                }
                t.RenderControl(new HtmlTextWriter(sw));
                html = sw.ToString();
            }
            return html;

        }

        /// <summary>
        /// Method to create table for Third party file uploads
        /// </summary>
        /// <param name="thirdPartyFileUploads"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static string CreateTablesList(List<ThirdPartyFileUpload> thirdPartyFileUploads)
        {
            string html = "";
            foreach (var item in thirdPartyFileUploads)
            {
                html += "<h3 style=\"font-family:Arial;\">Third Party Correspondence File Upload (" + (thirdPartyFileUploads.IndexOf(item) + 1).ToString() + ")</h3>";
                using (StringWriter sw = new StringWriter())
                {
                    Table t = new Table();
                    t.CellPadding = 8;
                    t.CellSpacing = 0;
                    t.BorderWidth = 2;
                    t.Font.Name = "Arial";
                    if (item.UploadDescription != null)
                    {
                        t.Rows.Add(GetRow("Upload Description", item.UploadDescription));
                    }
                    if (item.UploadFilename != null)
                    {
                        t.Rows.Add(GetRow("Upload Filename", item.UploadFilename));
                    }
                    t.RenderControl(new HtmlTextWriter(sw));
                    html += "<br/>" + sw.ToString();
                }
            }
            return html;
        }

        /// <summary>
        /// Method to create table for Supporting file uploads
        /// </summary>
        /// <param name="supportingFileUploads"></param>
        /// <returns></returns>
        [ExceptionHandler]
        private static string CreateTablesList(List<SupportingFileUpload> supportingFileUploads)
        {
            string html = "";
            foreach (var item in supportingFileUploads)
            {
                html += "<h3 style=\"font-family:Arial;\">Additional Supporting Files (" + (supportingFileUploads.IndexOf(item) + 1).ToString() + ")</h3>";
                using (StringWriter sw = new StringWriter())
                {
                    Table t = new Table();
                    t.CellPadding = 8;
                    t.CellSpacing = 0;
                    t.BorderWidth = 2;
                    t.Font.Name = "Arial";
                    if (item.UploadDescription != null)
                    {
                        t.Rows.Add(GetRow("Upload Description", item.UploadDescription));
                    }
                    if (item.UploadFilename != null)
                    {
                        t.Rows.Add(GetRow("Upload Filename", item.UploadFilename));
                    }
                    t.RenderControl(new HtmlTextWriter(sw));
                    html += "<br/>" + sw.ToString();
                }
            }
            return html;
        }
    }
}