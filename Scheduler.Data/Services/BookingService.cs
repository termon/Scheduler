using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Scheduler.Core.Models;
using Scheduler.Core.Security;
using Scheduler.Data.Repositories;

namespace Scheduler.Data.Services
{
    public class BookingService : IBookingService
    {
        private readonly DatabaseContext  _ctx;
       
        public BookingService()
        {
            _ctx = new DatabaseContext();            
        }
        
        public BookingService(DatabaseContext ctx)
        {           
            _ctx = ctx;
        }
       
        public void Initialise()
        {
           _ctx.Initialise(); 
        }
       

        // ------------------ Event Related Operations ------------------------

        // retrieve list of all Events
        public IList<Event> GetEvents()
        {   
            return _ctx.Events.ToList();
        }

        // Retrive Event by Id 
        public Event GetEvent(int id)
        {
            return _ctx.Events.FirstOrDefault(s => s.Id == id);           
        }

        public IEnumerable<Event> GetEventsQuery(Func<Event,bool> q)
        {
            return _ctx.Events.Where(q);          
        } 
 
        // Return events for room/user (if isAuth then can view all events)
        public IList<Event> GetUserEventsForRoom(int roomId, int userId, bool isAuth)
        {
            return _ctx.Events
                        .Where(e => e.RoomId == roomId)
                        // only display event title/description if user owns event or isAuth
                        .Select(e => new Event { 
                            Id = e.Id, 
                            Title = (e.UserId == userId || isAuth) ? e.Title  : "", 
                            Description =  (e.UserId == userId || isAuth) ? e.Description : "",
                            Start = e.Start,
                            End = e.End,
                            UserId = e.UserId,
                            RoomId = e.RoomId
                        })
                        .ToList();            
        } 

        // Add a new Event checking a Event with same email does not exist
        public Event AddEvent(Event e)
        {
            if (e == null) {  
                return null;
            }
            
            // verify event business logic here - no overlapping events etc.

            var s = new Event
            {              
                Title = e.Title,
                Description = e.Description,
                Start = e.Start,
                End = e.End,
                UserId = e.UserId,
                RoomId = e.RoomId
            };
            _ctx.Events.Add(s);
            _ctx.SaveChanges(); // write to database        
            return s; // return newly added Event
        }

        // Delete the Event identified by Id returning true if deleted and false if not found
        public bool DeleteEvent(int id)
        {
            var s = GetEvent(id);
            if (s == null)
            {
                return false;
            }
            _ctx.Events.Remove(s);
            _ctx.SaveChanges(); // write to database
            return true;
        }

        // Update the Event with the details in updated 
        public Event UpdateEvent(Event updated)
        {
            // verify the Event exists
            var e = GetEvent(updated.Id);
            if (e == null)
            {
                return null;
            }

            // verify event business logic here - no overlapping events etc.
            if (!IsValidEvent(updated))
            {
                return null;
            }

            // update the details of the Event retrieved and save
            e.Title = updated.Title;
            e.Description = updated.Description;
            e.Start = updated.Start;
            e.End = updated.End;
            e.RoomId = updated.RoomId;
            e.UserId = updated.UserId;

            _ctx.SaveChanges(); // write to database
            return e;
        }

        public bool IsValidEvent(Event n)
        {
            if (n.Start > n.End)
            {
                return false;
            }
            // find all events that overlap
            var count = _ctx.Events.Count(
                e => e.Id != n.Id &&  
                     (n.Start < e.Start && n.End > e.Start ||
                     n.Start >= e.Start & n.Start < e.End && n.End > e.Start)
            );
            return (count) == 0;
        }

        // ---------------------- Room Related Operations ------------------------

        public IList<Room> GetRooms()
        {
            return _ctx.Rooms.ToList();
        }

        public Room GetRoom(int id)
        {
            return _ctx.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public Room AddRoom(Room room)
        {
            var exists = _ctx.Rooms.FirstOrDefault(r => r.Name == room.Name);
            if (exists != null) 
            {
                return null;
            }

            _ctx.Rooms.Add(room);
            _ctx.SaveChanges();
            return room;
        }

         // ------------------ User Related Operations ------------------------

        // retrieve list of Users
        public IList<User> GetUsers()
        {
            return _ctx.Users.ToList();
        }

        // Retrive User by Id 
        public User GetUser(int id)
        {
            return _ctx.Users.FirstOrDefault(s => s.Id == id);
        }

        // Add a new User checking a User with same email does not exist
        public User AddUser(string name, string email, string password, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var user = new User
            {            
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password), // can hash if required 
                Role = role              
            };
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
            return user; // return newly added User
        }

        // Delete the User identified by Id returning true if deleted and false if not found
        public bool DeleteUser(int id)
        {
            var s = GetUser(id);
            if (s == null)
            {
                return false;
            }
            _ctx.Users.Remove(s);
            _ctx.SaveChanges();
            return true;
        }

        // Update the User with the details in updated 
        public User UpdateUser(User updated)
        {
            // verify the User exists
            var User = GetUser(updated.Id);
            if (User == null)
            {
                return null;
            }
            // update the details of the User retrieved and save
            User.Name = updated.Name;
            User.Email = updated.Email;
            User.Password = Hasher.CalculateHash(updated.Password);  
            User.Role = updated.Role; 

            _ctx.SaveChanges();          
            return User;
        }

        public User GetUserByEmail(string email, int? id=null)
        {
            return _ctx.Users.FirstOrDefault(u => u.Email == email && ( id == null || u.Id != id));
        }

        public IList<User> GetUsersQuery(Func<User,bool> q)
        {
            return _ctx.Users.Where(q).ToList();
        }

        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
            //return (user != null && user.Password == password ) ? user: null;
        }
   
    }
}