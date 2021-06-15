using System;
using System.Text.Json.Serialization;

namespace Scheduler.Core.Models
{

    public class Event 
    {
        public int Id { get; set; }       
        public string Title { get; set; }
        public string Description { get; set; } 
        public DateTime Start { get; set; } 
        public DateTime End { get; set; }  
        public int RoomId { get; set; }
        public int UserId { get; set; }

    }    
}