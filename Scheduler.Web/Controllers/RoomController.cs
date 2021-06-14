using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Scheduler.Data.Services;
using Scheduler.Core.Models;
using Scheduler.Web.ViewModels;
using Scheduler.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Scheduler.Web.Controllers
{
  
    [Authorize]
    public class RoomController : BaseController
    {
        private IBookingService _svc;

        public RoomController(IBookingService svc)
        {
            _svc = svc;
        }
   
        public IActionResult Index()
        {
            var rooms = _svc.GetRooms();
            return View(rooms);
        }

    }
}
