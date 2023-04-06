
using loveproject.entityy;
using LoveProject.Views;
using Microsoft.AspNetCore.Mvc;
using loveproject.dataa.Abstract;
using loveproject.dataa.Concrete.EfCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LoveProject.Identity;

namespace LoveProject.Controllers
{
    public class HomeController:Controller
    {

        private ILoverMatchRepository _ILoverRepository;
        private UserManager<User> _userManager;
        public HomeController(UserManager<User> userManager,ILoverMatchRepository loverRepository) {

            _ILoverRepository = loverRepository;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Calculate()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(string name1,string name2)
        {
            int Score = Calculator.Calculate(name1, name2);

            if (User.Identity.IsAuthenticated)
            {
                var match = new loveproject.entityy.LoverMatch()
                {
                    Name1 = name1,
                    Name2 = name2,
                    Score = Score.ToString(),
                    UserID = _userManager.GetUserId(User)
                };
                _ILoverRepository.Add(match);
            }
            return View("Result",Score);
           
        }



        [Authorize]
        public IActionResult List(string? q) {
            List<LoverMatch> lovers = _ILoverRepository.GetAll();
            var userMatches = new List<LoverMatch>();
            foreach (var love in lovers)
            {
                if(love.UserID==_userManager.GetUserId(User))
                {
                    userMatches.Add(love);
                }
            }

            if (!string.IsNullOrEmpty(q)) //Search 
            {
                userMatches = _ILoverRepository.Find(q);
            }

            return View(userMatches);
        }
        [HttpPost]
        public IActionResult Delete(LoverMatch loverMatch)
        {
            _ILoverRepository.Delete(loverMatch);
           return RedirectToAction("List");
        }
    }
}
