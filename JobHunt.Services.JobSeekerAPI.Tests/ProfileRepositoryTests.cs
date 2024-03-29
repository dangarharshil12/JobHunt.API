using FluentAssertions;
using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections;

namespace JobHunt.Services.JobSeekerAPI.Tests
{
    [TestFixture]
    public class ProfileRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        private User _user1;
        private User _updatedUser1;
        private User _user2;

        public ProfileRepositoryTests()
        {
            _user1 = new User()
            {
                Id = new Guid("30D4466A-5127-46C1-8327-1BB9D4816434"),
                FirstName = "Mark",
                LastName = "Jhonson",
                Email = "markjhonson345@email.com",
                Phone = "8576940123",
                Address = "House No: 1002,\nRandom Street,\nRandom City",
                TotalExperience = 2.5,
                ExpectedSalary = 40000,
                DateOfBirth = new DateTime(1996, 06, 12),
                ResumeUrl = "https://localhost:7246/Resumes/8edbd66b-3289-4535-90ee-77448716c03a.pdf",
                ImageUrl = "https://localhost:7246/Images/8edbd66b-3289-4535-90ee-77448716c03a.jpg"
            };

            _updatedUser1 = new User()
            {
                Id = new Guid("30D4466A-5127-46C1-8327-1BB9D4816434"),
                FirstName = "Joey",
                LastName = "Tribiani",
                Email = "joetrib@outlook.com",
                Phone = "9865741230",
                Address = "House No: 1001,\nRandom Street,\nRandom City",
                TotalExperience = 7,
                ExpectedSalary = 80000,
                DateOfBirth = new DateTime(1998, 06, 12),
                ResumeUrl = "https://localhost:7246/Resumes/8edbd66b-3289-4535-90ee-77448716c03a.pdf",
                ImageUrl = "https://localhost:7246/Images/8edbd66b-3289-4535-90ee-77448716c03a.jpg"
            };

            _user2 = new User()
            {
                Id = new Guid("3E5ECF05-83FD-4386-8B41-263D3DB4A7AF"),
                FirstName = "James",
                LastName = "Watson",
                Email = "jameswatson@gmail.com",
                Phone = "9587126340",
                Address = "House No: 1002,\nRandom Street,\nRandom City",
                TotalExperience = 5.5,
                ExpectedSalary = 50000,
                DateOfBirth = new DateTime(2001, 04, 20),
                ResumeUrl = "https://localhost:7246/Resumes/8edbd66b-3289-4535-90ee-77448716c03a.pdf",
                ImageUrl = "https://localhost:7246/Images/8edbd66b-3289-4535-90ee-77448716c03a.jpg"
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_JobSeeker").Options;
        }

        [Test]
        public async Task GetUsers_InputUserIdList_ReturnsUserListFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Users.AddAsync(_user1);
                await context.Users.AddAsync(_user2);
                await context.SaveChangesAsync();
            }
            List<Guid> userIds = new List<Guid> { new Guid("30D4466A-5127-46C1-8327-1BB9D4816434"), new Guid("3E5ECF05-83FD-4386-8B41-263D3DB4A7AF") };
            List<User> expectedUserList = new List<User> { _user1, _user2 };

            // Act 
            List<User> userListFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                userListFromDb = await repository.GetUsersAsync(userIds);
            }

            // Assert
            CollectionAssert.AreEqual(expectedUserList, userListFromDb, new UserCompare());
        }

        [Test]
        public async Task GetByEmailAsync_UserExists_ReturnUserFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Users.AddAsync(_user1);
                await context.SaveChangesAsync();
            }
            string email = "markjhonson345@email.com";

            // Act 
            User userFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                userFromDb = await repository.GetByEmailAsync(email);
            }

            // Assert
            userFromDb.Should().BeEquivalentTo(_user1);
        }

        [Test]
        public async Task GetByEmailAsync_UserDoesNotExists_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }
            string email = "markjhonson345@email.com";

            // Act 
            User userFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                userFromDb = await repository.GetByEmailAsync(email);
            }

            // Assert
            Assert.That(userFromDb, Is.Null);
        }
        [Test]
        public async Task GetByUserIdAsync_UserExists_ReturnUserFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Users.AddAsync(_user1);
                await context.SaveChangesAsync();
            }
            Guid userId = new Guid("30D4466A-5127-46C1-8327-1BB9D4816434");

            // Act 
            User userFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                userFromDb = await repository.GetByUserIdAsync(userId);
            }

            // Assert
            userFromDb.Should().BeEquivalentTo(_user1);
        }

        [Test]
        public async Task GetByUserIdAsync_UserDoesNotExists_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }
            Guid userId = new Guid("30D4466A-5127-46C1-8327-1BB9D4816434");

            // Act 
            User userFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                userFromDb = await repository.GetByUserIdAsync(userId);
            }

            // Assert
            Assert.That(userFromDb, Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddUser1_CheckUserInDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                await repository.CreateAsync(_user1);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var userFromDb = context.Users.FirstOrDefault(u => u.Id == new Guid("30D4466A-5127-46C1-8327-1BB9D4816434"));
                userFromDb.Should().BeEquivalentTo(_user1);
            }
        }

        [Test]
        public async Task UpdateAsync_User11_CheckUpdatedValuesFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Users.AddAsync(_user1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                await repository.UpdateAsync(_updatedUser1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.Users.FirstOrDefault(u => u.Id == new Guid("30D4466A-5127-46C1-8327-1BB9D4816434"));
                vacancyFromDb.Should().BeEquivalentTo(_updatedUser1);
                vacancyFromDb.Should().NotBeEquivalentTo(_user1);
            }
        }

        [Test]
        public async Task UpdateAsync_VacancyDoesNotExists_ReturnNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            User result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ProfileRepository(context);
                result = await repository.UpdateAsync(_updatedUser1);
            }

            //assert
            Assert.That(result, Is.Null);
        }

        private class UserCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var user1 = (User)x;
                var user2 = (User)y;
                if (user1.Id != user2.Id)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
