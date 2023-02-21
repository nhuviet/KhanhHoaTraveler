using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhanhHoaTravel.Models.ViewModels;

namespace KhanhHoaTravel.Controllers
{
    public class ServiceController : Controller
    {
        private IKHTravelRepository repository;
        //private readonly ILogger<HomeController> _logger;
        public int PageSize = 6;

        public ServiceController(IKHTravelRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index(int page = 1)
        {
            ViewBag.LoginUser = DataProvider.getUser(HttpContext);
            return View(new PlaceListViewModel {
                tblPlaceDeltail = repository.tblPlaceDeltail
                                   .Where(p => p.Status == 1)
                                   .OrderBy(p => p.Id)
                                   .Skip((page - 1) * PageSize)
                                   .Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.tblPlaceDeltail.Count()
                }
            });
        }
        public IActionResult PlaceDetail(int id)
        {
            _User user = new _User();
            EntertainmentPlace place = new EntertainmentPlace();
            List<string> imageList = new List<string>();

            user = DataProvider.getUser(HttpContext);
            place = DataProvider.getPlaceDetail(id);
            imageList = DataProvider.getListPlaceImage(id);

            ViewBag.LoginUser = DataProvider.getUser(HttpContext);
            ViewBag.place = place;
            ViewBag.imgLst = imageList;
            return View();
        }

        


    }

}

