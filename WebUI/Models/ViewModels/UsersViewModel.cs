using System.Collections.Generic;
using WebUI.MockRepository;

namespace WebUI.Models.ViewModels
{
    public class UsersViewModel
    {
        public string QueryString { get; set; }

        public List<User> Users { get; set; }
    }
}
