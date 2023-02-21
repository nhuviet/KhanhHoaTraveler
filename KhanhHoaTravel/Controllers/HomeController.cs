using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;

namespace KhanhHoaTravel.Controllers
{
    public class HomeController : Controller
    {
        

        private IKHTravelRepository repository;
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public HomeController(IKHTravelRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index()
        {
            _User user = new _User();
            user = DataProvider.getUser(HttpContext);
            if(user.Role == "admin")
            {
                return RedirectToAction("Account", "Admin");
            }
            ViewBag.LoginUser = DataProvider.getUser(HttpContext);
            return View(repository.tblPlaceDeltail);
        }
       

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult UserInfo()
        {
            ViewBag.LoginUser = DataProvider.getUser(HttpContext);
            if (DataProvider.getUser(HttpContext).Id == 0)
                return RedirectToAction("SignIn", "Authentication");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Authentication");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public void updateUserImageFile(IFormFile Image, string FileName)
        {
            if (Image != null && Image.Length > 0)
            {
                var fileName = Path.GetFileName(Image.FileName);
                fileName = FileName;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "User", fileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateUserInfo(IFormFile Image, int Id , string FullName, string Email, string Phone, string FaceBook ,string Website)
        {
            _User user = new _User();
            user.FullName = FullName;
            user.Id = Id;
            user.Phone = Phone;
            user.Email = Email;
            user.FaceBook = FaceBook;
            user.Website = Website;
            user.Image = "";
            if(Image != null)
            {
                string FileName = string.Format("{0}.jpg", user.Id.ToString());
                user.Image = FileName;
                updateUserImageFile(Image,FileName);
            }
            
            DataProvider.updateUserData(user);
            return RedirectToAction("UserInfo", "Home");
        }
    }
}
