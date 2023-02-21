using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhanhHoaTravel.Models;

namespace KhanhHoaTravel.Controllers
{
    public class AdminController : Controller
    {
        List<_User> users = new List<_User>();
        List<Post> posts = new List<Post>();
        List<EntertainmentPlace> places = new List<EntertainmentPlace>();

        public IActionResult Account()
        {
            _User user = DataProvider.getUser(HttpContext);
            if(user.Role != "admin")
                return RedirectToAction("Index", "Home");
            users = DataProvider.getListUser();
            ViewBag.userList = users;
            return View();
        }

        public IActionResult Post()
        {
            _User user = DataProvider.getUser(HttpContext);
            if (user.Role != "admin")
                return RedirectToAction("Index", "Home");
            posts = DataProvider.getAllPost();
            ViewBag.postList = posts;
            return View();
        }

        public IActionResult Place()
        {
            _User user = DataProvider.getUser(HttpContext);
            if (user.Role != "admin")
                return RedirectToAction("Index", "Home");
            places = DataProvider.getListPlace();
            ViewBag.postList = places;
            return View();
        }

        public IActionResult AccountDetail(int id)
        {
            _User UD = new _User();
            UD = DataProvider.getUserInfoById(id);
            ViewBag.UserDetail = UD;
            return View();
        }

        public IActionResult ChangeStatusUser(int id, int status)
        {
            DataProvider.changeStatusUser(id, status);
            return RedirectToAction("Account", "Admin");
        }

        public IActionResult ChangeStatusPost(int id, int status)
        {
            DataProvider.changeStatusPost(id, status);
            return RedirectToAction("Post", "Admin");
        }
        public IActionResult ChangeStatusPlace(int id, int status)
        {
            DataProvider.changeStatusPlace(id, status);
            return RedirectToAction("Place", "Admin");
        }

    }
}
