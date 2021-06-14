﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Core.Models;

namespace Scheduler.Web.ViewModels
{
    public class UserManageViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
 
        [Required]
        [EmailAddress]
        [Remote(action: "GetUserByEmailAddress", controller: "User")]
        public string Email { get; set; }

        [Required]
        public Role Role { get; set; }

    }
}