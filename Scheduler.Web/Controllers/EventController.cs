using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using Scheduler.Data.Services;
using Scheduler.Core.Models;
using Scheduler.Web.ViewModels;
using Scheduler.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Scheduler.Web.Controllers
{
  
    [Authorize]
    public class EventController : BaseController
    {
        private IBookingService _svc;

        public EventController(IBookingService svc)
        {
            _svc = svc;
        }

        public IActionResult Room(int id, string starts=null)
        {  
            // get logged in user
            var userId = User.GetSignedInUserId();

            // if no start date default to today
            if (starts == null) 
            {
                // uses startofweek extension method from Core project
                starts = DateTime.Now.StartOfWeek(DayOfWeek.Monday).ToString("yyyy-MM-dd");
            }       

            // get user events for specific room
            var events = _svc.GetUserEventsForRoom(id);
           
            var vm = new EventViewModel { UserId = userId, RoomId = id, Start = starts, End = starts, Events = events };  
            return View(vm);

        }

        public IActionResult Add(int id, DateTime start, DateTime end)
        {
            var userId = User.GetSignedInUserId();
            var room = _svc.GetRoom(id);

            var e = new Event { RoomId = room.Id, UserId = userId, Start = start, End = end };
            var v = EventViewModel.FromEvent(e);
            return View(v);
        }
        
        [HttpPost]
        public IActionResult Add([Bind]EventViewModel vm) 
        {
            if (ModelState.IsValid) {
                var updated = _svc.AddEvent(vm.ToEvent());
                if (updated != null)
                {
                    Alert("Event Successfully Created", AlertType.info);
                    return RedirectToAction(nameof(Room), new { Id = updated.RoomId });
                }
                else 
                {
                    Alert("Event could not be created", AlertType.warning);
                }
            }
            return View(vm);
        }

        public IActionResult Edit(int id) 
        {
            // extract users id and role from Identity
            var userId = User.GetSignedInUserId();
               
            // load the event
            var e = _svc.GetEvent(id);
            if (e == null) {
                Alert("Event Does not exist", AlertType.warning); 
                return RedirectToAction(nameof(Index), nameof(Room));  
            }
            // check user has privilege to edit event
            if (userId != e.UserId && !User.HasOneOfRoles(Role.Admin.ToString())) {
                Alert("You cannot edit this event", AlertType.warning);
                return RedirectToAction("Room", new { Id = e.RoomId }); 
            }

            var vm = EventViewModel.FromEvent(e);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind]EventViewModel vm) 
        {
            if (ModelState.IsValid) {
                var updated = _svc.UpdateEvent(vm.ToEvent());
                if (updated != null)
                {
                    Alert("Event Successfully Updated", AlertType.info);
                    return RedirectToAction("Room", new { Id = updated.RoomId });
                }
                else 
                {
                    Alert("Event could not be updated", AlertType.warning);
                }
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Delete(int id) 
        {
            var e = _svc.GetEvent(id);
            if (e == null) {
                Alert("No Such Event", AlertType.warning);
                return RedirectToAction(nameof(Index),nameof(Room));
            }
            
            if (_svc.DeleteEvent(id)) 
            {
                Alert("Event Successfully Deleted", AlertType.info);
            }
            else
            {
                Alert("Event Could not be Deleted", AlertType.warning);
            }
            return RedirectToAction(nameof(Room), new { Id = e.RoomId }); 
        }

        // Remote validator to validate event
        [AcceptVerbs("GET", "POST")]
        public IActionResult ValidateDate([BindRequired, FromQuery]EventViewModel vm)
        {
            if (!_svc.IsValidEvent(vm.ToEvent()))          
            {
                return Json($"Event overlaps another event.");
            }

            return Json(true);
        }

    }
}
