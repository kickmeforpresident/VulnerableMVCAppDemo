using System.Collections.Generic;

namespace WebUI.MockRepository
{
    public class MockRepo
    {
        public List<User> Users { get; set; }

        public MockRepo()
        {
            Users = new List<User>();

            Users.Add(new User { Id = 1, Name = "Jóska Pista" });
            Users.Add(new User { Id = 2, Name = "Nagy Béla" });
            Users.Add(new User { Id = 3, Name = "Kiss Gábor" });
            Users.Add(new User { Id = 4, Name = "Kelemen József" });
            Users.Add(new User { Id = 5, Name = "Hamar Ferenc" });
        }
    }
}
