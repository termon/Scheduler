using System;
using System.Text.Json.Serialization;

namespace Scheduler.Core.Models 
{

    public class Room 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; } 
    }
    
}