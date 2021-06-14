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
        public int Id { get; set; } 

        [Required]            
        public string Title { get; set; }

        [Required]
        public string Description { get; set; } 

        public string Start { get; set; } 
        public string StartTime { get; set;}

        public string End { get; set; }  
        public string EndTime { get; set;}

        public int RoomId { get; set; }      
        public int UserId { get; set; }
        
        public IList<Event> Events { get; set; } = new List<Event>();
        
        // calendar json data
        public string EventsJson { get; set; } = "[]";        
        
        public string EventsAsJson => JsonSerializer.Serialize(
            Events.Select(e => new {
                    id = e.Id,
                    title = e.Title,
                    description = e.Description,
                    start = e.Start.ToString("yyyy-MM-dd HH:mm"),                   
                    end = e.End.ToString("yyyy-MM-dd HH:mm"),
                    userId = e.UserId,
                    roomId= e.RoomId,
                    display = UserId != e.UserId ? "background" : ""
                }).ToList()
        );
        
        public string Serialize()
        {
            var transformed = Events.Select(e => new {
                    id = e.Id,
                    title = e.Title,
                    description = e.Description,
                    start = e.Start.ToString("yyyy-MM-dd HH:mm"),                   
                    end = e.End.ToString("yyyy-MM-dd HH:mm"),
                    userId = e.UserId,
                    roomId= e.RoomId,
                    display = UserId != e.UserId ? "background" : ""
                }).ToList();
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