using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Vinaday.Web.Framework.EmailHelpers
{
    public static class MailClient
    {
        private static readonly SmtpClient Client;

        static MailClient()
        {
            MailClient.Client = new SmtpClient()
            {
                Host = ConfigurationManager.AppSettings["SmtpServer"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"], ConfigurationManager.AppSettings["SmtpPass"])
            };
        }

        public static bool ApproveDinningClubMember(string email, string body)
        {
            string str = string.Format("{0}", "Chào mừng đến với Dinningclub.com");
            return MailClient.SendMessage(email, email, str, body);
        }

        public static bool BookingHotelVn(string email, string body, string pnr)
        {
            string str = string.Format("{0} #{1}", "VINADAY goREISE", pnr);
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, str, body);
            return flag;
        }

        public static bool GeneratorLottoNumber(string email, string body)
        {
            string str = string.Format("{0}", "Mã số may mắn");
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, str, body);
            return flag;
        }

        public static bool Inquiry(string body, string subject)
        {
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], ConfigurationManager.AppSettings["adminBCC"], subject, body);
            return flag;
        }

        public static bool Register(string email, string body)
        {
            string str = string.Format("{0}", "Welcome to vinaday.com!");
            return MailClient.SendMessage(email, email, str, body);
        }

        public static bool RequestBookingForHotel(string body, string subject, string cc)
        {
            bool flag;
            List<MailAddress> mailAddresses = new List<MailAddress>();
            if (!string.IsNullOrEmpty(cc))
            {
                List<string> list = cc.Split(new char[] { ',' }).ToList<string>();
                if (list.Any<string>())
                {
                    string str = list.First<string>();
                    list.RemoveAt(1);
                    for (int i = 0; i < list.Count<string>(); i++)
                    {
                        string item = list[i];
                        if (!string.IsNullOrEmpty(item))
                        {
                            mailAddresses.Add(new MailAddress(item));
                        }
                    }
                    flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], str, subject, body, mailAddresses);
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static bool RequestBookingHotel(string body, string subject)
        {
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], ConfigurationManager.AppSettings["adminBCC"], subject, body);
            return flag;
        }

        public static bool RequestBookingHotel(string email, string body, string subject)
        {
            //send to admin and customer
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, subject, body);
            //send to customer
//            flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, subject, body);
            return flag;
        }

        public static bool RequestRegister(string email, string body)
        {
            string str = string.Format("{0}", "[Vinaday] Activation Account");
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, str, body);
            return flag;
        }

        public static bool SendInquireEmail(string title, string body)
        {
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], ConfigurationManager.AppSettings["inquireEmail"], title, body);
            return flag;
        }

        public static bool SendLostPassword(string email, string password)
        {
            string str = string.Concat("Your password is: ", password);
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, "Lost Password", str);
            return flag;
        }

        public static bool SendMessage(string from, string to, string subject, string body)
        {
            MailMessage mailMessage = null;
            bool flag = true;
            try
            {
                mailMessage = new MailMessage(from, to, subject, body)
                {
                    IsBodyHtml = true,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                mailMessage.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["adminReceive"]));
//                mailMessage.CC.Add(new MailAddress(ConfigurationManager.AppSettings["adminBCC"]));
                MailClient.Client.Send(mailMessage);
                flag = true;
            }
            catch (Exception exception)
            {
                string message = exception.Message;
            }

            return flag;
        }

        public static bool SendMessage(string from, string to, string subject, string body, List<MailAddress> ccs)
        {
            MailMessage mailMessage = null;
            bool flag = false;
            {
                try
                {
                    mailMessage = new MailMessage(from, to, subject, body)
                    {
                        IsBodyHtml = true,
                        DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                    };
                    if (ccs.Count > 0)
                    {
                        foreach (MailAddress mailAddress in
                            from cc in ccs
                            where cc != null
                            select cc)
                        {
                            mailMessage.CC.Add(mailAddress);
                        }
                    }
                    mailMessage.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["adminReceive"]));
                    mailMessage.CC.Add(new MailAddress(ConfigurationManager.AppSettings["adminBCC"]));
                    MailClient.Client.Send(mailMessage);
                    flag = true;
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
            return flag;
        }

        public static bool SendMessage(string from, string to, string subject, string body, Attachment attach)
        {
            MailMessage mailMessage = null;
            bool flag = false;
            {
                try
                {
                    mailMessage = new MailMessage(from, to, subject, body)
                    {
                        IsBodyHtml = true,
                        DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                    };
                    mailMessage.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["adminReceive"]));
                    mailMessage.CC.Add(new MailAddress(ConfigurationManager.AppSettings["adminBCC"]));
                    if (attach != null)
                    {
                        mailMessage.Attachments.Add(attach);
                    }
                    MailClient.Client.Send(mailMessage);
                    flag = true;
                }
                catch (Exception exception)
                {
                    string message = exception.Message;
                }
            }
            return flag;
        }

        public static bool SendWelcome(string title, string body)
        {
            bool flag = MailClient.SendMessage(ConfigurationManager.AppSettings["adminEmail"], ConfigurationManager.AppSettings["adminReceive"], title, body);
            return flag;
        }
    }
}