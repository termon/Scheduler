using System;
using Xunit;
using Scheduler.Core.Models;
using Scheduler.Core.Security;

using Scheduler.Data.Services;

namespace Scheduler.Test
{
    public class ServiceTests
    {
        private IBookingService service;

        public ServiceTests()
        {
            service = new BookingService();
            service.Initialise();
        }

        [Fact]
        public void EmptyDbShouldReturnNoUsers()
        {
            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(0, users.Count);
        }
        
        [Fact]
        public void AddingUsersShouldWork()
        {
            // arrange
            service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );
            service.AddUser("guest", "guest@mail.com", "guest", Role.Guest);

            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void UpdatingUserShouldWork()
        {
            // arrange
            var user = service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );
            
            // act
            user.Name = "administrator";
            user.Email = "admin@mail.com";            
            var updatedUser = service.UpdateUser(user);

            // assert
            Assert.Equal("administrator", user.Name);
            Assert.Equal("admin@mail.com", user.Email);
        }

        [Fact]
        public void LoginWithValidCredentialsShouldWork()
        {
            // arrange
            service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );
            
            // act            
            var user = service.Authenticate("admin@mail.com","admin");

            // assert
            Assert.NotNull(user);
           
        }

       
        [Fact]
        public void AddEventForInvalidUserShouldNotWork()
        {
            // arrange
            var userId = 0; // invalid user

            // act      
            var calendarEvent = service.AddEvent(
                new Event
                    { Description = "Demo", Start = DateTime.Now, End = DateTime.Now + TimeSpan.FromHours(1), UserId = userId }
            );

            // assert
            Assert.NotNull(calendarEvent);
        }
        
        [Fact]
        public void AddEventThatDoesNotOverlapShouldWork()
        {
            // arrange
            var user = service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );

            // act      
            var calendarEvent = service.AddEvent(
                new Event
                { Description = "Demo", Start = DateTime.Now, End = DateTime.Now + TimeSpan.FromHours(1), UserId = user.Id }
            );

            // assert
            Assert.NotNull(calendarEvent);
        }
        
        [Fact]
        public void AddEventThatOverlapsStartShouldNotWork()
        {
            // arrange
            var user = service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );

            var start = DateTime.Now;
            var end = DateTime.Now + TimeSpan.FromHours(1);
            var calendarEvent1 = service.AddEvent(
                new Event { Description = "Demo1", Start = start, End = end, UserId = user.Id }
            );
            
            // act  - create overlapping event
            var calendarEvent2 = service.AddEvent(
                new Event { Description = "Demo2", Start = start - TimeSpan.FromMinutes(30), End = end - TimeSpan.FromMinutes(30), UserId = user.Id }
            );

            // assert
            Assert.Null(calendarEvent2);
        }
        
        [Fact]
        public void AddEventThatOverlapsEndShouldNotWork()
        {
            // arrange
            var user = service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );

            var start = DateTime.Now;
            var end = DateTime.Now + TimeSpan.FromHours(1);
            var calendarEvent1 = service.AddEvent(
                new Event { Description = "Demo1", Start = start, End = end, UserId = user.Id }
            );
            
            // act  - create overlapping event
            var calendarEvent2 = service.AddEvent(
                new Event { Description = "Demo2", Start = start + TimeSpan.FromMinutes(30), End = end + TimeSpan.FromMinutes(30), UserId = user.Id }
            );

            // assert
            Assert.Null(calendarEvent2);
        }
        
        [Fact]
        public void AddEventThatOverlapsCompletelyShouldNotWork()
        {
            // arrange
            var user = service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );

            var start = DateTime.Now;
            var end = DateTime.Now + TimeSpan.FromHours(1);
            var calendarEvent1 = service.AddEvent(
                new Event { Description = "Demo1", Start = start, End = end, UserId = user.Id }
            );
            
            // act  - create overlapping event
            var calendarEvent2 = service.AddEvent(
                new Event { Description = "Demo2", Start = start - TimeSpan.FromMinutes(30), End = end + TimeSpan.FromMinutes(30), UserId = user.Id }
            );

            // assert
            Assert.Null(calendarEvent2);
        }

    }
}
