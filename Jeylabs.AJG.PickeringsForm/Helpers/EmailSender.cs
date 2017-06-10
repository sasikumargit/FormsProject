using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using Jeylabs.AJG.PickeringsForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Helpers
{
    /// <summary>
    /// Class to send email to initiator and AJG
    /// </summary>
    public static class EmailSender
    {
        /// <summary>
        /// Method to send email to initiator and AJG
        /// </summary>
        /// <param name="EmailDetails">Authetication Details</param>
        /// <param name="body"> Email body</param>
        /// <param name="subject"> Email Subject</param>
        /// <param name="FileCollections">File Collections to be attached</param>
        /// <returns>Return email status </returns>
        [ExceptionHandler]
        public static bool SendMail(IDictionary<string, string> EmailDetails, string body, string subject, List<string> FileCollections = null)
        {
            bool emailSendStatus = false;
            MailMessage Mail = new MailMessage();
            try
            {
                SmtpClient SmtpServer = new SmtpClient(EmailDetails["SmtpClient"]);
                Mail.From = new MailAddress(EmailDetails["PickeringsEmailID"]);
                Mail.To.Add(EmailDetails["ToAddress"]);
                Mail.Subject = subject;
                Mail.IsBodyHtml = true;
                var inlineLogo = new LinkedResource(AppDomain.CurrentDomain.BaseDirectory + "Content\\Images\\AJG_Email_Logo.png");
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                body = body.Replace("###IMG###", string.Format(@"<img border=""0"" width=""310"" height=""73"" src=""cid:{0}"" id=""AJGEmailLogo"" alt=""Arthur J.Gallagher"" />", inlineLogo.ContentId));
                Mail.Body = body;
                var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                Mail.AlternateViews.Add(view);

                if (FileCollections != null)
                {
                    foreach (string file in FileCollections)
                    {
                        Mail.Attachments.Add(new Attachment(file));
                    }
                }
                SmtpServer.Port = Convert.ToInt16(EmailDetails["PortId"]);

                if (string.IsNullOrEmpty(EmailDetails["PickeringsEmailPassword"]))
                {
                    SmtpServer.UseDefaultCredentials = true;
                }
                else
                {
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(EmailDetails["PickeringsEmailID"], EmailDetails["PickeringsEmailPassword"]);
                }                
                SmtpServer.Send(Mail);
                Mail.Dispose();
                emailSendStatus = true;
            }

            catch (Exception ex)
            {
                WriteCustomErrorLog.WriteLog(ex.StackTrace);
                Mail.Dispose();
                return emailSendStatus;
            }
            return true;
        }
    }
}