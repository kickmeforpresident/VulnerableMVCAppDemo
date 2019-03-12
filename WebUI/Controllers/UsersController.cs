using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebUI.MockRepository;
using WebUI.Models.ViewModels;

namespace WebUI.Controllers
{
    public class UsersController : Controller
    {
        private readonly MockRepo _repository;

        public UsersController()
        {
            _repository = new MockRepo();
        }

        public IActionResult Index()
        {
            return View(new UsersViewModel());
        }

        [HttpPost]
        public IActionResult Index(UsersViewModel model)
        {
            var filteredUsers = FilterUsers(model);

            model.Users = filteredUsers;

            return View(model);
        }

        private List<User> FilterUsers(UsersViewModel model)
        {
            return _repository.Users
                                .Where(u => u.Name.Contains(model.QueryString))
                                .ToList();
        }
    }
}