using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext context;

        private ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
        {
            context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByNameAsync(model.Username);
                    Session["user_id"] = user.Id;
                    string sql = "Select r.Name from AspNetRoles r JOIN AspNetUserRoles ur ON ur.RoleId = r.Id JOIN AspNetUsers u ON u.Id = ur.UserId Where u.Id = '" + user.Id + "'";
                    DataTable dt = db.List(sql);
                    string roles = dt.Rows[0]["Name"].ToString();

                    Session["roles"] = roles;
                    System.Diagnostics.Debug.WriteLine("WE are Done===> " + roles);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                           .ToList(), "Name", "Name");
            ViewBag.UserRoles = new SelectList(context.Roles.ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                          .ToList(), "Name", "Name");
            ViewBag.UserRoles = new SelectList(context.Roles.ToList(), "Name", "Name");
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = model.Username, Email = model.Email.ToUpperInvariant() };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, model.UserRoles );

                }

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword

        [Authorize]
        public ActionResult ResetPassword(string code)
        {
            if (Session["roles"].ToString() == "Admin")
            {
                Debug.WriteLine("Admin entered");
                string sql = "Select * From AspNetUsers";
                DataTable dt = db.List(sql);

                List<SelectListItem> listItems = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listItems.Add(new SelectListItem
                    {
                        Text = dt.Rows[i]["Email"].ToString(),
                        Value = dt.Rows[i]["Email"].ToString()
                    });

                }
                ViewBag.Email = listItems;
                ViewBag.Visible = "block";
                return View();

            }
            else
            {
                Debug.WriteLine("Non entered");
                List<SelectListItem> listItems = new List<SelectListItem>();

                ViewBag.Email = listItems;
                ViewBag.Visible = "hidden";
                return View();
            }
            //code == null ? View("Error") 

        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            System.Diagnostics.Debug.WriteLine(model.Email);
            System.Diagnostics.Debug.WriteLine(model.ConfirmPassword);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            System.Diagnostics.Debug.WriteLine("This will be displayed in output window=================================>");
            System.Diagnostics.Debug.WriteLine(user.ToString());


            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }



            UserManager.RemovePassword(user.Id);

            UserManager.AddPassword(user.Id, model.ConfirmPassword);
            return RedirectToAction("ResetPasswordConfirmation", "Account");

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["user_id"] = "";
            Session["roles"] = "";
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        
       
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        public ActionResult ListUser()
        {
            string sql = "Select u.Id,u.UserName,u.Email,ur.Name,ur.Id as rid from ASPNetUsers u Join ASPNetUserRoles us ON us.UserId = u.Id JOIN ASPNetRoles ur on us.RoleId = ur.Id";
            DataTable dt = db.List(sql);

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> dd = new Dictionary<string, string>();
                dd.Add("Id", dt.Rows[i]["Id"].ToString());
                dd.Add("UserName",dt.Rows[i]["UserName"].ToString());
                dd.Add("Email", dt.Rows[i]["Email"].ToString());
                dd.Add("Name", dt.Rows[i]["Name"].ToString());
                dd.Add("r_id", dt.Rows[i]["rid"].ToString());
                list.Add(dd);
            }

            ViewBag.list = list;
            return View();
        }
        public ActionResult ChangeUserDetail(string id, string roles_id)
        {
            string sql = "Select u.Id,u.UserName,u.Email,ur.Name from ASPNetUsers u Join ASPNetUserRoles us ON us.UserId = u.Id JOIN ASPNetRoles ur on us.RoleId = ur.Id where u.Id = '"+id+"'";
            DataTable dt = db.List(sql);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("UserName",dt.Rows[0]["Username"].ToString());
            dic.Add("Email", dt.Rows[0]["Email"].ToString());
            dic.Add("Role", dt.Rows[0]["Name"].ToString());

            ViewBag.dic = dic;

            string sql1 = "Select * from AspNetRoles";
            DataTable dt1 = db.List(sql1);
            List<SelectListItem> listItems = new List<SelectListItem>();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                listItems.Add(new SelectListItem
                {
                    Text = dt1.Rows[i]["Name"].ToString(),
                    Value = dt1.Rows[i]["Id"].ToString()
                });
                
            }
            ViewBag.selectedRoles = listItems;
            return View();

        }
        #endregion
        [HttpPost]
        public ActionResult ChangeUserDetail(string id,string UserName,string Email,string selectedRoles, string roles_id)
        {
            Debug.WriteLine("Selected Rples: "+selectedRoles);
            

            string sql1 = "Update AspNetUsers Set UserName='"+UserName+"',Email='"+Email+"' where Id='" + id + "'";
            db.Edit(sql1);

            if (selectedRoles != "")
            {
                string sql = "Delete From AspNetUserRoles Where UserId = '" + id + "' and RoleId='" + roles_id + "'";
                db.Delete(sql);
                string sql2 = "Insert into AspNetUserRoles (UserId,RoleId) Values ('" + id + "','" + selectedRoles + "')";
                db.Create(sql2);
            }
            
           

            Debug.WriteLine(id + UserName + Email+  selectedRoles);
            return RedirectToAction("ListUser");
        }

        public ActionResult DeleteUser(string id,  string roles_id)
        {
            string sql = "Delete From AspNetUserRoles Where UserId = '" + id + "' and RoleId='" + roles_id + "'";
            db.Delete(sql);

            string sql1 = "Delete From AspNetUsers Where Id = '" + id + "'";
            db.Delete(sql1);


            return RedirectToAction("ListUser");
        }

        public ActionResult AssignRole()
        {
            string sql = "Select * from AspNetRoles";
            DataTable dt = db.List(sql);
            List<SelectListItem> listItems = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listItems.Add(new SelectListItem
                {
                    Text = dt.Rows[i]["Name"].ToString(),
                    Value = dt.Rows[i]["Id"].ToString()
                });

            }
            ViewBag.listItems = listItems;

            string sql1 = "Select * from AspNetUsers";
            DataTable dt1 = db.List(sql1);
            List<SelectListItem> listItems1 = new List<SelectListItem>();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                listItems1.Add(new SelectListItem
                {
                    Text = dt1.Rows[i]["UserName"].ToString(),
                    Value = dt1.Rows[i]["Id"].ToString()
                });

            }
            ViewBag.listItems1 = listItems1;



            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(string listItems1, string listItems)
        {
            string sql2 = "Insert into AspNetUserRoles (UserId,RoleId) Values ('" + listItems1 + "','" + listItems + "')";
            db.Create(sql2);

            string sql = "Select * from AspNetRoles";
            DataTable dt = db.List(sql);
            List<SelectListItem> k1 = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                k1.Add(new SelectListItem
                {
                    Text = dt.Rows[i]["Name"].ToString(),
                    Value = dt.Rows[i]["Id"].ToString()
                });

            }
            ViewBag.listItems = k1;

            string sql1 = "Select * from AspNetUsers";
            DataTable dt1 = db.List(sql1);
            List<SelectListItem> k2 = new List<SelectListItem>();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                k2.Add(new SelectListItem
                {
                    Text = dt1.Rows[i]["UserName"].ToString(),
                    Value = dt1.Rows[i]["Id"].ToString()
                });

            }
            ViewBag.listItems1 = k2;
            return View();
        }

    }
}