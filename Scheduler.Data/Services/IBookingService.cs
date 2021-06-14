using System;
using System.Collections.Generic;
using Scheduler.Core.Models;
using Scheduler.Data.Repositories;

namespace Scheduler.Data.Services
{
    public interface IBookingService
    {
         
        public void Initialise();    
        public Event GetEvent(int id);

        public IList<Event> GetEvents();       
        public IList<Event> GetUserEventsForRoom(int roomId, int userId, bool isAuth);

        public Event AddEvent(Event e);
        public bool DeleteEvent(int id);
        public Event UpdateEvent(Event updated);
       
        public IEnumerable<Event> GetEventsQuery(Func<Event,bool> q);
        public bool IsValidEvent(Event e);
        
        public IList<Room> GetRooms();
        public Room GetRoom(int id);
        public Room AddRoom(Room r);


        // ---------------- User Management --------------
        IList<User> GetUsers();
        User GetUser(int id);
        User GetUserByEmail(string email, int? id);
        User AddUser(string name, string email, string password, Role role);
        User UpdateUser(User user);
        bool DeleteUser(int id);
        User Authenticate(string email, string password);

    }
}