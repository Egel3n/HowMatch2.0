using LoveProject.EmailService;
using LoveProject.Identity;
using LoveProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoveProject.Controllers
{
    [AutoValidateAntiforgeryToken] //formlardaki tokenları serverdan gelen tokenla aynı mı diye karşılaştırır
    public class AccountController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }


        public IActionResult Login(string? ReturnUrl)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            }); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

             if (!ModelState.IsValid)
            {
                
                return View(loginModel);
            }

            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user == null)
            {
                
                ModelState.AddModelError("","Kullanıcı Adı Bulunamadı");//validation'a gider ilk parametre boş bırakılırsa direkt ALL'a gider.
                return View(loginModel);
            }

            
            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);//3. parametre false bırakılırsa tarayıcı kapatılınca cookie silinir.
            
            if(await _userManager.CheckPasswordAsync(user, loginModel.Password)){

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Lütfen Hesabınızı Onaylayınız");
                    return View(loginModel);
                }

            }

            if (result.Succeeded)
            {
                return Redirect(loginModel.ReturnUrl??"~/");
            }

            ModelState.AddModelError("", "Girilen Parola Yanlış");
            return View(loginModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("valid");
                return View(model);
            }

            var user = new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
            };



            var result = await _userManager.CreateAsync(user, model.Password);
           
           
            if (result.Succeeded)
            {
                //token creation
              var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail","Account", new
                {
                    userId= user.Id,
                    token = code
                });


                //Generate Token and Send It to via Mail
                Console.WriteLine(url);
                await _emailService.Send(model.Email,"Hesap Doğrulama",$"Lütfen Hesabınızı <a href='https://localhost:7051{url}'> Buraya </a> Tıklayarak Onaylayınız");
                return RedirectToAction("Login");
            }

            return View(model);
            
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string? userId,string? token)
        {

            string message="";

            if(userId == null || token == null)
            {
                message = "Geçersiz Token";
                ViewBag.message = message;
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                message = "Geçersiz Kullanıcı Adı";
                ViewBag.message = message;
                return View();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                message = "Hesap Onaylandı";
                ViewBag.message = message;
                return View();
            }

            message = "bilinmeyen bir hata oluştu";
            ViewBag.message = message;
            return View();
        }

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if(email == null)
            {
                ModelState.AddModelError("", "Mailinizi Giriniz");

            }

            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
            }


             var code =  await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });
            _emailService.Send(email, "Şifrenizi Sıfırlayın", $"Lütfen Hesabınızı <a href='https://localhost:7051{url}'> Buraya </a> Tıklayarak Onaylayınız");

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ResetPassword(string? userId, string? token)
        {
            if (userId == null || token == null)
            {
                ViewBag.Message = "Geçersiz Link";
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Message = "Kullanıcı Bulunamadı";
                return View();
            }


            ViewBag.Message = null;
            return View(new ForgotPasswordModel()
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token,
            }) ;
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgotPasswordModel model)
        {
            if (model.Token == null || model.Email == null)
            {
                Console.WriteLine("bunlar boş amına goyim"); 
                return View();
            }


            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            { 
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token,model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordResult");

            }

            return View(model);
        }

        public IActionResult ResetPasswordResult()
        {
            return View();
        }


        public async Task<IActionResult> UserPage()
        {

            var roles = await _userManager.GetRolesAsync(await _userManager.FindByNameAsync(User.Identity.Name));

            return View(roles);
        }


    }
}
