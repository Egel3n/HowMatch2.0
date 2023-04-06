using LoveProject.Identity;
using Microsoft.AspNetCore.Identity;

namespace LoveProject.Models
{
    public class RoleModel
    {
        public string Name { get; set; }

    }

    public class RoleEditModel
    {

        public string RoleName { get; set; }
        public List<User> allUsers { get; set; }
        public List<User> authUsers { get; set; }
    }

    public class RoleDetail
    {
        public string RoleName { get; set; }

        public List<string> ToAdds { get; set; }

    }
}
