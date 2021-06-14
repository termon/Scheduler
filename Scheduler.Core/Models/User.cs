using System;
namespace Scheduler.Core.Models
{
    // Add User roles relevant to your application
    public enum Role { Admin, Manager, Guest }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // User role within application
        public Role Role { get; set; }

        // used to store jwt auth token 
        public string Token { get; set; }
    }
}
