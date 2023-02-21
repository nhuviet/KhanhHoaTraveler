using KhanhHoaTravel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KhanhHoaTravel.Controllers
{
    public class AuthenticationController : Controller
    {
        static string connStr = "Server=DESKTOP-6OQ42LL;Database=KhanhHoaTraveler;Trusted_Connection=True;MultipleActiveResultSets=true ";
        //static string connStr = ConfigurationManager.ConnectionStrings["DataConnect"].ConnectionString;
        //static string connStr = ConfigurationManager.GetSection("ConnectionStrings:DataConnect").ToString();
        static SqlConnection conn = new SqlConnection();
        static SqlCommand cmd = new SqlCommand();
        static SqlDataReader reader = null;
        //private readonly SignInManager<_User> _signInManager;
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogin { get; set; }
        public static void connectDb()
        {
            conn = new SqlConnection();
            conn.ConnectionString = connStr;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            else
                return;
        }

        [HttpGet]
        public IActionResult OutLog()
        {
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Registe()
        {
            return View();
        }

        //[HttpGet]
        //[Route("ExternalLoginCallback")]
        //public async Task<IActionResult> ExternalLoginCallback()
        //{
        //    var _signInManager = HttpContext.RequestServices.GetRequiredService<SignInManager<_User>>();
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        // If the user doesn't have an account, create a new one
        //        var user = new _User { UserName = email, Email = email };
        //        var createResult = await _userManager.CreateAsync(user);
        //        if (createResult.Succeeded)
        //        {
        //            // Add the external login to the new user
        //            var addLoginResult = await _userManager.AddLoginAsync(user, info);
        //            if (addLoginResult.Succeeded)
        //            {
        //                // Sign in the user with the external login
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                return RedirectToAction("Index");
        //            }
        //        }

        //        return RedirectToAction("Login");
        //    }
        //}


        //public IActionResult GoogleAuth()
        //{
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("ExternalLoginCallback", "Account", new { returnUrl }));
        //    return new ChallengeResult("Google", properties);
        //}
        public void GoogleAuth(string returnUrl = "/")
        {
            GoogleLogin();
        }

        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleRespose", "Authentication", null, Request.Scheme)
            });
        }

        //public async Task GoogleLogin()
        //{
        //    await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
        //    {
        //        RedirectUri = Url.Action(nameof(GoogleResponse), "Authentication")
        //    });
        //}
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        //    var claims = authenticateResult.Principal.Claims.Select(c => new { c.Type, c.Value });

        //    return Json(claims);
        //}
        public async Task<IActionResult> GoogleRespose()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            return Json(claims);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public void Save_UserLogin(HttpContext context,_User u)
        {
            // Lấy ISession
            var session = context.Session;
            string key_access = "UserLogin";

            // Lưu vào  Session thông tin truy cập
            // Định nghĩa cấu trúc dữ liệu lưu trong Session
            _User user = new _User();

            // Đọc chuỗi lưu trong Sessin với key = info_access
            string json = session.GetString(key_access);
            dynamic lastAccessInfo;
            if (json != null) {
                // Convert chuỗi Json - thành đối tượng có cấu trúc như accessInfoType
                lastAccessInfo = JsonConvert.DeserializeObject(json, user.GetType());
            }
            else {
                // json chưa từng lưu trong Session, accessInfo lấy bằng giá trị khởi  tạo
                lastAccessInfo = user;
            }
            _User accessUserSave = new _User();
            accessUserSave.UserName = u.UserName;
            accessUserSave.Password = u.Password;
            
            // Convert accessInfo thành chuỗi Json và lưu lại vào Session
            string jsonSave = JsonConvert.SerializeObject(accessUserSave);
            session.SetString(key_access, jsonSave);
        }

        [HttpPost]
        public IActionResult Verify(_User u)
        {
            if (DataProvider.isUserExist(u)) {
                Save_UserLogin(HttpContext, u);
                return RedirectToAction("Index", "Home");
            }
            else {
                ViewBag.FLog = 1;
                return View("SignIn");
            }
        }

        [HttpPost]
        public IActionResult Create(_User user)
        {
            connectDb();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT * FROM dbo.VerifyUser WHERE UserName = '" + user.UserName + "' AND Password = '" + user.Password + "'";
            object r = cmd.ExecuteScalar();
            if (r != null) {
                conn.Close();
                return View("Register");
            }

            else
            {
                string addDataTbl_User = "insert into _User values(2,1)";
                cmd.CommandText = addDataTbl_User;
                cmd.ExecuteNonQuery();
                
                string addDataTblVerifyUser = "insert into VerifyUser values ((select max(id) from dbo._User),'" + user.UserName + "','" + user.Password + "')";
                cmd.CommandText = addDataTblVerifyUser;
                cmd.ExecuteNonQuery();

                string addDataTblUserDetail = "insert into UserDetail values ((select max(id) from dbo._User),N'" + user.FullName + "',GETDATE(),'" + user.Email + "','" + user.Phone + "','" + user.FaceBook + "','" + user.Website + "')";
                cmd.CommandText = addDataTblUserDetail;
                cmd.ExecuteNonQuery();

                string addDataTblUserImage = "INSERT INTO dbo.UserImage VALUES((select max(id) from dbo._User),'/image/User/default.jpg',1)";
                cmd.CommandText = addDataTblUserImage;
                cmd.ExecuteNonQuery();

                conn.Close();
                return View("SignIn");
            }

        }

        //public AuthenticationController(SignInManager<IdentityUser> signInManager)
        //{
        //    _signInManager = signInManager;
        //}
    }
}
