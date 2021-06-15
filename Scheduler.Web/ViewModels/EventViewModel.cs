using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Core.Extensions;
using Scheduler.Core.Models;

namespace Scheduler.Web.ViewModels
{
    public class EventViewModel 
    {
        // non editable by user
        public int Id { get; set; } 
        public int RoomId { get; set; }      
        public int UserId { get; set; }

        [Required]            
        public string Title { get; set; }

        [Required]
        public string Description { get; set; } 

        [Remote(action: "ValidateDate", controller: "Event", AdditionalFields = "Start,StartTime,End,EndTime")]
        public string Start { get; set; } 
        public string StartTime { get; set;}

        [Remote(action: "ValidateDate", controller: "Event", AdditionalFields = "Start,StartTime,End,EndTime")]      
        public string End { get; set; }
        public string EndTime { get; set;}
        
        public IList<Event> Events { get; set; } = new List<Event>();    

        // generate calendar event json for user
        public string SerializeEventsForUser(int userId, bool isAuth)
        {                  
            var transformed = Events.Select(e =>
                new  { 
                    id = e.Id,
                    userId = e.UserId,
                    roomId = e.RoomId,
                    title = (e.UserId == userId || isAuth) ? e.Title  : "",              // only display title if user owns event or isAuth
                    description =  (e.UserId == userId || isAuth) ? e.Description : "",  // only display description if user owns event or isAuth
                    start = e.Start.ToString("yyyy-MM-dd HH:mm"),                        // format start date into string                   
                    end = e.End.ToString("yyyy-MM-dd HH:mm"),                            // format end date into string
                    url = $"/event/edit/{e.Id}",                                         // url to navigate to when event clicked
                    classNames = userId == e.UserId ? "owner" : "other",                 // style based on ownership (also done via calendar config property eventClassNames: ),
            }
            ).ToList(); 
            return JsonSerializer.Serialize(transformed);
        }           

        // ---------------------- Mappers --------------------
        // make an viewmodel from an event
        public static EventViewModel FromEvent(Event e)
        {
            var v = new EventViewModel {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Start = e.Start.ToString("yyyy-MM-dd"),            
                StartTime = e.Start.ToString("HH:mm"),
                End = e.End.ToString("yyyy-MM-dd"),
                EndTime = e.End.ToString("HH:mm"),
                UserId = e.UserId,
                RoomId= e.RoomId,  
            };
            return v;
        }

        // Make an Event from a ViewModel
        public Event ToEvent() {
            var e = new Event {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                Start = DateTime.Parse($"{this.Start} {this.StartTime}"),   
                //Start = DateTime.ParseExact($"{this.Start}T{this.StartTime}","yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-UK")),
                End = DateTime.Parse($"{this.End} {this.EndTime}"),         
                //End = DateTime.ParseExact($"{this.End}T{this.EndTime}","yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-UK")),
                UserId = this.UserId,
                RoomId= this.RoomId  
            };
            return e;
        }
    }    
}