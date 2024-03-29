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
    public class ExperienceRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        private UserExperience _experience1;
        private UserExperience _updatedExperience1;
        private UserExperience _experience2;

        public ExperienceRepositoryTests()
        {
            _experience1 = new UserExperience()
            {
                Id = new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                CompanyName = "Earth Solution",
                StartYear = new DateTime(2023, 05, 31),
                EndYear = new DateTime(2024, 02, 28),
                CompanyUrl = "www.earthsolutions.com",
                Designation = "Site Manager",
                JobDescription = "Random Description for the job",
            };

            _experience2 = new UserExperience()
            {
                Id = new Guid("00D6EE71-9751-42AD-4201-08DC4A4939FD"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                CompanyName = "Leaf Village Ninja School",
                StartYear = new DateTime(2019, 12, 31),
                EndYear = new DateTime(2022, 12, 30),
                CompanyUrl = "www.leafvillageschool.com",
                Designation = "Teacher",
                JobDescription = "Random Description for the Teacher Job",
            };

            _updatedExperience1 = new UserExperience()
            {
                Id = new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                CompanyName = "Construction Group",
                StartYear = new DateTime(2022, 05, 31),
                EndYear = new DateTime(2023, 02, 28),
                CompanyUrl = "www.constructiongroup.com",
                Designation = "Senior Site Manager",
                JobDescription = "Random Description for the senior job",
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_JobSeeker").Options;
        }

        [Test]
        public async Task GetAllByUserIdAsync_Experience1And2_ReturnsExperienceListFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserExperiences.AddAsync(_experience1);
                await context.UserExperiences.AddAsync(_experience2);
                await context.SaveChangesAsync();
            }
            Guid userId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A");
            List<UserExperience> userExperiences = new List<UserExperience>() { _experience1, _experience2 };

            // Act 
            List<UserExperience> userExperienceFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                userExperienceFromDb = await repository.GetAllByUserIdAsync(userId);
            }

            // Assert
            CollectionAssert.AreEqual(userExperiences, userExperienceFromDb, new UserExperienceCompare());
        }

        [Test]
        public async Task GetByIdAsync_UserExperienceExists_ReturnUserExperienceFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserExperiences.AddAsync(_experience1);
                await context.SaveChangesAsync();
            }
            Guid Id = new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E");

            // Act 
            UserExperience userExperienceFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                userExperienceFromDb = await repository.GetByIdAsync(Id);
            }

            // Assert
            userExperienceFromDb.Should().BeEquivalentTo(_experience1);
        }

        [Test]
        public async Task GetByUserIdAsync_UserDoesNotExists_ReturnsNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }
            Guid Id = new Guid("8A74A371-5BD2-484D-D653-08DC3E924DC5");

            // Act 
            UserExperience userExperienceFromDb;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                userExperienceFromDb = await repository.GetByIdAsync(Id);
            }

            // Assert
            Assert.That(userExperienceFromDb, Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddUserExperience_CheckThatValueFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                await repository.CreateAsync(_experience1);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var userFromDb = context.UserExperiences.FirstOrDefault(u => u.Id == new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"));
                userFromDb.Should().BeEquivalentTo(_experience1);
            }
        }

        [Test]
        public async Task UpdateAsync_Experience1Exists_CheckUpdatedValuesFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserExperiences.AddAsync(_experience1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                await repository.UpdateAsync(_updatedExperience1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var userExperienceFromDb = context.UserExperiences.FirstOrDefault(u => u.Id == new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"));
                userExperienceFromDb.Should().BeEquivalentTo(_updatedExperience1);
                userExperienceFromDb.Should().NotBeEquivalentTo(_experience1);
            }
        }

        [Test]
        public async Task UpdateAsync_UserExperienceDoesNotExists_ReturnNull()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            UserExperience result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                result = await repository.UpdateAsync(_updatedExperience1);
            }

            //assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_DeleteExperience1_CheckExperienceDeletedFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserExperiences.AddAsync(_experience1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ExperienceRepository(context);
                await repository.DeleteAsync(_experience1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var experienceFromDb = context.UserExperiences.FirstOrDefault(u => u.Id == new Guid("F32A812A-4190-4F00-3A8F-08DC3E97F46E"));
                Assert.That(experienceFromDb, Is.Null);
            }
        }

        private class UserExperienceCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var userExperience1 = (UserExperience)x;
                var userExperience2 = (UserExperience)y;
                if (userExperience1.Id != userExperience2.Id)
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
