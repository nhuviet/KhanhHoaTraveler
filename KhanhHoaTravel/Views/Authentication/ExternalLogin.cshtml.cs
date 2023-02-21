using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;



namespace KhanhHoaTravel.Views.Authentication
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<_User> signInManager;
        private readonly UserManager<_User> userManager;
        //private readonly IEmailSender
        public void OnGet()
        {
        }
    }
}
