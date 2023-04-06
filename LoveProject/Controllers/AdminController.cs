using LoveProject.Identity;
using LoveProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace LoveProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller

    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole() { Name = model.Name });
            if (result.Succeeded)
            {
                return RedirectToAction("RoleList");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(RoleModel model)
        {
            if (!ModelState.IsValid)
            {

                return RedirectToAction("RoleList");
            }

            var role = _roleManager.Roles.Where(i=>i.Name == model.Name).FirstOrDefault();
           var result = await  _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
;
                return RedirectToAction("RoleList");
            }
            
            return RedirectToAction("RoleList");
        }

        public async Task<IActionResult> RoleEdit(string name)
        {
            var role = _roleManager.Roles.Where(i => i.Name == name).FirstOrDefault();
            if(role == null)
            {
                Console.WriteLine("role null");
            }
            List<User> users = _userManager.Users.ToList();
            List<User> authUsers = new List<User>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    authUsers.Add(user);
                }
            }

            RoleEditModel roleEditModel = new RoleEditModel()
            {
                RoleName = role.Name,
                allUsers = users,
                authUsers = authUsers
            };
            return View(roleEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleDetail model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            if(model == null)
            {
                return View(model);
            }
            foreach (var userId in model.ToAdds)
            {
               var user = await _userManager.FindByIdAsync(userId);
               await _userManager.AddToRoleAsync(user, model.RoleName);
            }
            var ToAdds = new List<User>();

            foreach(var userId in model.ToAdds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                ToAdds.Add(user);
            } 
            foreach(var user in _userManager.Users.ToList().Except(ToAdds))
            {
                await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            }
            return RedirectToAction("RoleList");
            
        }


        public async Task<IActionResult> UserList()
        {
            var UserList = _userManager.Users.ToList();
            return View(UserList);
        }

        public async Task<IActionResult> UserEdit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return View(user);

        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(User model)
        {
            Console.WriteLine(model.UserName);

            var user = await _userManager.FindByIdAsync(model.Id);

            if(model.UserName != null )
            {
                user.UserName = model.UserName;
            }
            if (model.LastName != null)
            {
                user.LastName = model.LastName;
            }
            if (model.FirstName != null)
            {
                user.FirstName = model.FirstName;
            }
            if (model.Email != null)
            {
                user.Email = model.Email;
            }

            await _userManager.UpdateAsync(user);


            return RedirectToAction("UserList");

        }



    }
}
