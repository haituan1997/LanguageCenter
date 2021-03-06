using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using LanguageCenter.App_Start;
using LanguageCenter.Areas.Home.Models;
using LanguageCenter.Models;
using LanguageCenter.Repository;
using AutoMapper;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.Object;

namespace LanguageCenter.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _UserRepository;
        private readonly StudentAccountRepository _StudentAccountRepository;
        private readonly StudentRepository _StudentRepository;
         
        public AccountController()
        {
            _UserRepository = new UserRepository();
            _StudentAccountRepository = new StudentAccountRepository();
            _StudentRepository = new StudentRepository();
            Mapper.CreateMap<RegisterModel, StudentAccount>();
            Mapper.CreateMap<RegisterModel, Student>();

        }
        // GET: Account
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.
                if (this.Request.IsAuthenticated)
                {
                    // Info.
                    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    String userName = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                                              .Select(c => c.Value).SingleOrDefault();
                    String pass = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                                              .Select(c => c.Value).SingleOrDefault();

                    var loginUser = _UserRepository.Get_Users(userName,pass);

                    return this.RedirectToPage(loginUser, returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            var model = new LoginViewModel();
            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                // Verification.
                if (ModelState.IsValid)
                {
                    // Initialization.
                    var logindetails = _UserRepository.Get_Users(model.UserName, model.Password) ;
                   
                    // Verification.
                    if (logindetails != null )
                    { 
                        // Login In.
                        this.SignInUser(logindetails, false);

                        // Info.return this.RedirectToAction("Index", "Home");
                     
                        return this.RedirectToPage(logindetails, returnUrl);
                        
                        
                    }
                    else
                    {
                        // Setting.
                        ModelState.AddModelError("UserName", "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                // Verification.
                if (ModelState.IsValid)
                {
                    var checkUserName = _StudentAccountRepository.Get_StudentAccountByUserName(model.UserLogin);
                    if(checkUserName == null)
                    {
                        var data = Mapper.Map<RegisterModel, Student>(model);

                        var studentID = _StudentRepository.Insert(data);

                        var account = Mapper.Map<RegisterModel, StudentAccount>(model);
                        account.StudentID = Convert.ToInt64(studentID);

                        _StudentAccountRepository.Insert(account);

                        // Verification.
                        return this.RedirectToAction("Login", "Account");
                    }    
                    else
                    {
                        ModelState.AddModelError("UserLogin", "username đã tồn tại.");
                    }    
                    
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void SignInUser(User username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, username.FullName));
                claims.Add(new Claim("UserID", username.UserID.ToString())); 
                claims.Add(new Claim("Password", username.Password.ToString())); 
                claims.Add(new Claim("TypeUser", username.TypeUser.ToString())); 
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        private void ClaimIdentities(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();
            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return this.RedirectToAction("LogOff", "Account");
        }

        private ActionResult RedirectToPage(User user, string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return this.RedirectToAction("Index", "Home");
            }
            return this.Redirect(returnUrl);

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            model.DateOfBirth = DateTime.Now;
            return View("Register", model);
        }
    }
}