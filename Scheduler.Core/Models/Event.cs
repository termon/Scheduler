using System;
using System.Text.Json.Serialization;

namespace Scheduler.Core.Models
{

    public class Event 
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }       

        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; } 

        [JsonPropertyName("start")]
        public DateTime Start { get; set; } 

        [JsonPropertyName("end")] 
        public DateTime End { get; set; }  

        [JsonPropertyName("roomId")]
        public int RoomId { get; set; }
        
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

    }    
}