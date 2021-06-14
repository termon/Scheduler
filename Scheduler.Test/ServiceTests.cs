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
        public void LoginWithInvalidCredentialsShouldNotWork()
        {
            // arrange
            service.AddUser("admin", "admin@mail.com", "admin", Role.Admin );

            // act      
            var user = service.Authenticate("admin@mail.com","xxx");

            // assert
            Assert.Null(user);
           
        }

    }
}
