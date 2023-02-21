using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KhanhHoaTravel.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index() {
            ViewBag.LoginUser = DataProvider.getUser(HttpContext);
            return View();
        }

        public IActionResult MailGo()
        {
            String From_email = "nhatrangtravel2022@gmail.com";
            //String From_email = "itjob2002@gmail.com";

            //mail to Contact person
            String To_email = Request.Form["email"];
            String sub = "Hỗ trợ người dùng";
            MailMessage mail = new MailMessage();
            mail.To.Add(To_email.Trim());
            mail.From = new MailAddress(From_email);
            mail.Subject = sub;
            mail.Body = "Cảm ơn bạn đã liên hệ, chúng tôi sẽ phản hồi với bạn sớm nhất có thể";
            mail.IsBodyHtml = true;

            //mail to host
            String Host_mail = "nhuviet2012@gmail.com";
            String name = Request.Form["name"];
            String content = Request.Form["message"];

            MailMessage mailToHost = new MailMessage();
            mailToHost.To.Add(Host_mail.Trim());
            mailToHost.From = new MailAddress(From_email);
            mailToHost.Subject = string.Format("NhaTrangTravel: Người dùng {0} đã liên hệ", name);
            mailToHost.Body = content;
            mailToHost.IsBodyHtml = true;

            //Define Text
            mailToHost.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("html", new System.Net.Mime.ContentType("text/html")));
            mailToHost.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(content, new System.Net.Mime.ContentType("text/plain")));
            mailToHost.SubjectEncoding = System.Text.Encoding.UTF8;
            mailToHost.BodyEncoding = System.Text.Encoding.UTF8;

            //Send mail
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(From_email, "lrjuefjbjmyupdgy");
            //smtp.Credentials = new NetworkCredential(From_email, "cjbjalkmivsckcbg"); //itjob
            smtp.Send(mail);
            smtp.Send(mailToHost);

            return View("Index");
        }

        
    }
}
