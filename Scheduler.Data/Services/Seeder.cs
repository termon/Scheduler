using System;
using System.Text;
using System.Collections.Generic;

using Scheduler.Core.Models;

namespace Scheduler.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy 
        // test data using an IUserService 
        public static void Seed(IBookingService svc)
        {
            svc.Initialise();

            // add users
            var u1 = svc.AddUser("Administrator", "admin@mail.com", "admin", Role.Admin);
            var u2 = svc.AddUser("Manager", "manager@mail.com", "manager", Role.Manager);
            var u3 = svc.AddUser("Guest", "guest@mail.com", "guest", Role.Guest);    
        
            // add rooms
            var r1 = svc.AddRoom( new Room { Name = "Room1", Capacity=20 });
            var r2 = svc.AddRoom( new Room { Name = "Room2", Capacity=50 });
            var r3 = svc.AddRoom( new Room { Name = "Room3", Capacity=40 });

            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day < 4 ? 4 : DateTime.Now.Day;         
            day = DateTime.Now.Day > 25 ? 25 : DateTime.Now.Day;
            
            // add events
            svc.AddEvent(
                new Event { 
                    Title = "User 1 Room 1 Event 1", Description = "Conference", 
                    Start= new DateTime(2021,month,day-2,10,0,0), 
                    End = new DateTime(2021,month,day-2,12,0,0),
                    RoomId = r1.Id, UserId = u1.Id 
                }
            );
            svc.AddEvent(
                new Event { 
                    Title = "User 3 Room 1 Event 1", Description = "Party", 
                    Start= new DateTime(2021,month,day,11,0,0), 
                    End = new DateTime(2021,month,day,14,0,0),
                    RoomId = r1.Id, UserId = u3.Id 
                }
            );
            svc.AddEvent(
                new Event { 
                    Title = "User 3 Room 1 Event 2", Description = "Conference", 
                    Start= new DateTime(2021,month,day+1,11,0,0), 
                    End = new DateTime(2021,month,day+1,14,0,0),
                    RoomId = r1.Id, UserId = u3.Id 
                }
            );
            svc.AddEvent(
                new Event { 
                    Title = "User 3 Room 2 Event 3", Description = "Seminar", 
                    Start= new DateTime(2021,month,day-3,11,0,0), 
                    End = new DateTime(2021,month,day-3,14,0,0),
                    RoomId = r2.Id, UserId = u3.Id 
                }
            );
        }
    }
}
