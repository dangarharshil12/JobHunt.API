using FluentAssertions;
using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Repository;
using JobHunt.Services.EmployerAPI.Utility;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections;

namespace JobHunt.Services.EmployerAPI.Tests
{
    [TestFixture]
    public class ApplicationRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        private UserVacancyRequest _application1;
        private UserVacancyRequest _updatedApplication1;
        private UserVacancyRequest _application2;
        private UserVacancyRequest _application3;
        private Vacancy _vacancy1;
        private Vacancy _vacancy2;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_Employer").Options;
        }

        public ApplicationRepositoryTests()
        {
            _application1 = new UserVacancyRequest()
            {
                Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                UserId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22"),
                AppliedDate = new DateTime(2024, 04, 03),
                ApplicationStatus = SD.Status_Submitted
            };

            _updatedApplication1 = new UserVacancyRequest()
            {
                Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                UserId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22"),
                AppliedDate = new DateTime(2024, 04, 03),
                ApplicationStatus = SD.Status_Accepted
            };

            _application2 = new UserVacancyRequest()
            {
                Id = new Guid("79312940-DFF1-4CF0-DA80-08DC3C4CCEFD"),
                VacancyId = new Guid("674DA545-6231-476E-E018-08DC3C4D373A"),
                UserId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22"),
                AppliedDate = new DateTime(2024, 03, 11),
                ApplicationStatus = SD.Status_Submitted
            };

            _application3 = new UserVacancyRequest()
            {
                Id = new Guid("297D6625-08CB-49A7-DA81-08DC3C4CCEFD"),
                VacancyId = new Guid("674DA545-6231-476E-E018-08DC3C4D373A"),
                UserId = new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"),
                AppliedDate = new DateTime(2024, 03, 11),
                ApplicationStatus = SD.Status_Submitted
            };

            _vacancy1 = new Vacancy()
            {
                Id = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                PublishedBy = "Medi Solutions",
                PublishedDate = new DateTime(2024, 03, 04),
                NoOfVacancies = 2,
                MinimumQualification = "B. Pharm",
                JobTitle = "Quality Manager",
                JobDescription = "job description for quality manager job",
                ExperienceRequired = "3 to 5 years",
                LastDate = new DateTime(2024, 03, 27),
                MinimumSalary = 30000,
                MaximumSalary = 37000,
            };

            _vacancy2 = new Vacancy()
            {
                Id = new Guid("674DA545-6231-476E-E018-08DC3C4D373A"),
                PublishedBy = "Green Solutions",
                PublishedDate = new DateTime(2024, 03, 05),
                NoOfVacancies = 5,
                MinimumQualification = "MBA",
                JobTitle = "Senior Customer Executive",
                JobDescription = "job description for Senior Customer Executive job",
                ExperienceRequired = "3 to 5 years",
                LastDate = new DateTime(2024, 03, 10),
                MinimumSalary = 25000,
                MaximumSalary = 30000,
            };
        }

        [Test]
        public async Task GetAllByUserIdAsync_ApplicationswithThatUserIdExists_ReturnsListFromDatabase()
        {
            // Arrange
            _application1.Vacancy = _vacancy1;
            _application2.Vacancy = _vacancy2;
            var expecedList = new List<UserVacancyRequest> { _application1, _application2 };
            Guid userId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22");

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(_application1);
                await context.UserVacancyRequests.AddAsync(_application2);
                await context.SaveChangesAsync();
            }

            // Act
            List<UserVacancyRequest> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                actualList = await repository.GetAllByUserIdAsync(userId);
            }

            //  Assert
            CollectionAssert.AreEqual(expecedList, actualList, new UserVacancyRequestCompare());

            // Cleanup
            _application1.Vacancy = null;
            _application2.Vacancy = null;
        }

        [Test]
        public async Task GetAllByUserIdAsync_ApplicationswithThatUserIdDoesNotExists_ReturnsEmptyListFromDatabase()
        {
            // Arrange
            Guid userId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22");

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            List<UserVacancyRequest> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                actualList = await repository.GetAllByUserIdAsync(userId);
            }

            //  Assert
            CollectionAssert.AreEqual(new List<UserVacancyRequest>([]), actualList, new UserVacancyRequestCompare());
        }

        [Test]
        public async Task GetAllByVacancyIdAsync_ApplicationswithThatVacancyIdExists_ReturnsListFromDatabase()
        {
            // Arrange
            _application2.Vacancy = _vacancy2;
            _application3.Vacancy = _vacancy2;
            var expecedList = new List<UserVacancyRequest> { _application2, _application3 };
            Guid vacancyId = new Guid("674DA545-6231-476E-E018-08DC3C4D373A");

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(_application2);
                await context.UserVacancyRequests.AddAsync(_application3);
                await context.SaveChangesAsync();
            }

            // Act
            List<UserVacancyRequest> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                actualList = await repository.GetAllByVacancyIdAsync(vacancyId);
            }

            //  Assert
            CollectionAssert.AreEqual(expecedList, actualList, new UserVacancyRequestCompare());

            // Cleanup
            _application2.Vacancy = null;
            _application3.Vacancy = null;
        }

        [Test]
        public async Task GetAllByVacancyIdAsync_ApplicationswithThatVacancyIdDoesNotExists_ReturnsEmptyListFromDatabase()
        {
            // Arrange
            Guid vacancyId = new Guid("3660948D-570C-488B-4458-08DC3CCC67B1");

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            List<UserVacancyRequest> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                actualList = await repository.GetAllByVacancyIdAsync(vacancyId);
            }

            //  Assert
            CollectionAssert.AreEqual(new List<UserVacancyRequest>([]), actualList, new UserVacancyRequestCompare());
        }

        [Test]
        public async Task GetDetailAsync_ApplicationExists_ReturnApplicationFromDatabase()
        {
            // Arrange
            Guid userId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22");
            Guid vacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7");
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(_application1);
                await context.SaveChangesAsync();
            }

            // Act
            UserVacancyRequest result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                result = await repository.GetDetailAsync(userId, vacancyId);
            }

            // Assert
            result.Should().BeEquivalentTo(_application1);
        }

        [TestCase("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7", "8EDBD66B-3289-4535-90EE-77448716C03A")]
        [TestCase("3660948D-570C-488B-4458-08DC3CCC67B1", "8EDBD66B-3289-4535-90EE-77448716C03A")]
        [TestCase("51B1BBE3-14B5-4000-BBE7-08DC3EA06D85", "41E68188-4D6C-49DB-964A-B0C64EF4C57B")]
        public async Task GetDetailAsync_ApplicationDoesNotExists_ReturnNull(string user, string vacancy)
        {
            // Arrange
            Guid userId = new Guid(user);
            Guid vacancyId = new Guid(vacancy);
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(_application1);
                await context.UserVacancyRequests.AddAsync(_application2);
                await context.SaveChangesAsync();
            }

            // Act
            UserVacancyRequest result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                result = await repository.GetDetailAsync(userId, vacancyId);
            }

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetDetailsById_ApplicationExists_ReturnsApplicationFromDatabase()
        {
            // Arrange
            Guid Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E");
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(_application1);
                await context.SaveChangesAsync();
            }

            // Act
            UserVacancyRequest result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                result = await repository.GetDetailByIdAsync(Id);
            }

            // Assert
            result.Should().BeEquivalentTo(_application1);
        }

        [Test]
        public async Task GetDetailsById_ApplicationDoesNotExists_ReturnsNull()
        {
            // Arrange
            Guid Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E");
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            UserVacancyRequest result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                result = await repository.GetDetailByIdAsync(Id);
            }

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddApplication1_CheckThatValueFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                await repository.CreateAsync(_application1);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.UserVacancyRequests.FirstOrDefault(u => u.Id == new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"));
                vacancyFromDb.Should().BeEquivalentTo(_application1);
            }
        }

        [Test]
        public async Task UpdateAsync_Vacancy1_CheckUpdatedValuesFromDatabase()
        {
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(_application1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                await repository.UpdateAsync(_updatedApplication1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var applicationFromDb = await context.UserVacancyRequests.FirstOrDefaultAsync(u => u.Id == new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"));
                applicationFromDb.Should().BeEquivalentTo(_updatedApplication1);
                applicationFromDb.Should().NotBeEquivalentTo(_application1);
            }
        }

        [Test]
        public async Task UpdateAsync_VacancyDoesNotExists_ReturnNull()
        {
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            UserVacancyRequest result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new ApplicationRepository(context);
                result = await repository.UpdateAsync(_updatedApplication1);
            }

            //assert
            Assert.That(result, Is.Null);
        }

        private class UserVacancyRequestCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var application1 = (UserVacancyRequest)x;
                var application2 = (UserVacancyRequest)y;
                if (application1.Id != application2.Id)
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
