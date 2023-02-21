using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KhanhHoaTravel.Views.Authentication
{
    public class RegisteModel : PageModel
    {
        private readonly SignInManager<_User> _signInManager;
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogin { get; set; }

        [HttpGet]
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //ExternalLogin = ((IList<AuthenticationScheme>)await _signInManager.GetExternalAuthenticationSchemesAsync());
        }
    }
}
