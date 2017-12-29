using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ContactController : Controller
    {
       
        public IActionResult Send(MessageModel message)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(message.Email, SMTP.Login));
                emailMessage.To.Add(new MailboxAddress("", SMTP.Email));
                emailMessage.Subject = message.Topic;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message.Text
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SMTP.Host, SMTP.Port, false);
                    client.Authenticate(SMTP.Login, SMTP.Password);
                    client.Send(emailMessage);

                    client.Disconnect(true);
                }

                ViewBag.SuccessMessage = "Сообщение отправлено успешно!";
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return View("Index");
        }
    }
}
