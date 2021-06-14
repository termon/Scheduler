using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Core.Models;

namespace Scheduler.Web.ViewModels
{
    // Authenticated User Model returned by Api
    public class UserAuthenticatedViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public string Token { get; set; }

    }
}