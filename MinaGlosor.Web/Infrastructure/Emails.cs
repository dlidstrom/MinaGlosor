using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Postal;

namespace MinaGlosor.Web.Infrastructure
{
    public static class Emails
    {
        public static void InviteUser(string email, Guid activationCode)
        {
            Send(
                "InviteUser",
                email,
                "Inbjudan",
                o =>
                {
                    o.ActivationCode = activationCode;
                });
        }

        public static void ResetPassword(string email, string activationCode)
        {
            Send(
                "ResetPassword",
                email,
                "Återställ lösenord",
                o =>
                {
                    o.ActivationCode = activationCode;
                });
        }

        private static void Send(
            string view,
            string recipient,
            string subject,
            Action<dynamic> action)
        {
            dynamic email = new Email(view);
            email.To = recipient;
            email.From = ConfigurationManager.AppSettings["OwnerEmail"];
            email.Subject = string.Format("=?utf-8?B?{0}?=", Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)));

            // add moderators
            var moderators = new MailAddressCollection();
            var moderatorEmails = ConfigurationManager.AppSettings["OwnerEmail"].Split(';')
                .Select(e => new MailAddress(e.Trim()))
                .ToList();
            moderatorEmails.ForEach(moderators.Add);
            email.Bcc = moderators;
            action.Invoke(email);
            email.Send();
        }
    }
}