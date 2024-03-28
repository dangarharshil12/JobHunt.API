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
    public class VacancyRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        private Vacancy _vacancy1;
        private Vacancy _vacancy2;
        private Vacancy _vacancy3;

        public VacancyRepositoryTests()
        {
            _vacancy1 = new Vacancy()
            {
                Id = new Guid("674da545-6231-476e-e018-08dc3c4d373a"),
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
                Id = new Guid("3660948d-570c-488b-4458-08dc3ccc67b1"),
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

            _vacancy3 = new Vacancy()
            {
                Id = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                PublishedBy = "Green Solutions",
                PublishedDate = new DateTime(2024, 02, 29),
                NoOfVacancies = 2,
                MinimumQualification = "Site Manager",
                JobTitle = "Senior Customer Executive",
                JobDescription = "job description for Senior Customer Executive job",
                ExperienceRequired = "3 to 5 years",
                LastDate = new DateTime(2024, 03, 08),
                MinimumSalary = 30000,
                MaximumSalary = 50000,
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_Employer").Options;
        }

        [Test]
        public async Task GetAllAsync_Vacancy1AndVacancy2_CheckBoththeVacancyFromDatabase()
        {
            // Arrange
            var expecedList = new List<Vacancy> { _vacancy1, _vacancy2 };

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new VacancyRepository(context);
                await repository.CreateAsync(_vacancy1);
                await repository.CreateAsync(_vacancy2);
            }

            // Act
            List<Vacancy> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                actualList = await repository.GetAllAsync();
            }

            //  Assert
            CollectionAssert.AreEqual(expecedList, actualList, new VacancyCompare());
        }

        [Test]
        public async Task GetByIdAsync_Vacancy1_ChecktheVacancyFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new VacancyRepository(context);
                await repository.CreateAsync(_vacancy1);
            }

            // Act 
            Vacancy actualVacancy;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                actualVacancy = await repository.GetByIdAsync(new Guid("674da545-6231-476e-e018-08dc3c4d373a"));
            }

            // Assert
            actualVacancy.Should().BeEquivalentTo(_vacancy1);
        }

        [Test]
        public async Task GetByNameAsync_Vacancy1_ChecktheVacancyFromDatabase()
        {
            // Arrange
            var expecedList = new List<Vacancy> { _vacancy2, _vacancy3 };

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new VacancyRepository(context);
                await repository.CreateAsync(_vacancy2);
                await repository.CreateAsync(_vacancy3);
            }

            // Act 
            List<Vacancy> actualVacancyList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                actualVacancyList = await repository.GetByNameAsync("Green Solutions");
            }

            //  Assert
            CollectionAssert.AreEqual(expecedList, actualVacancyList, new VacancyCompare());
        }

        [Test]
        public async Task CreateAsync_Vacancy1_CheckTheValuesFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                await repository.CreateAsync(_vacancy1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.VacancyDetails.FirstOrDefault(u => u.Id == new Guid("674da545-6231-476e-e018-08dc3c4d373a"));
                vacancyFromDb.Should().BeEquivalentTo(_vacancy1);
            }
        }

        [Test]
        public async Task UpdateAsync_Vacancy1_CheckUpdatedValuesFromDatabase()
        {
            // Arrange
            Vacancy _updatedVacancy1 = new Vacancy()
            {
                Id = new Guid("674da545-6231-476e-e018-08dc3c4d373a"),
                PublishedBy = "Medi Solutions",
                PublishedDate = new DateTime(2024, 03, 04),
                NoOfVacancies = 3,
                MinimumQualification = "B. Tech",
                JobTitle = "Junior Quality Manager",
                JobDescription = "job description for junior quality manager job",
                ExperienceRequired = "3 to 6 years",
                LastDate = new DateTime(2024, 03, 29),
                MinimumSalary = 40000,
                MaximumSalary = 47000,
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new VacancyRepository(context);
                await repository.CreateAsync(_vacancy1);
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                await repository.UpdateAsync(_updatedVacancy1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.VacancyDetails.FirstOrDefault(u => u.Id == new Guid("674da545-6231-476e-e018-08dc3c4d373a"));
                vacancyFromDb.Should().BeEquivalentTo(_updatedVacancy1);
                vacancyFromDb.Should().NotBeEquivalentTo(_vacancy1);
            }
        }

        [Test]
        public async Task UpdateAsync_VacancyDoesNotExists_ReturnNull()
        {
            // Arrange
            Vacancy _updatedVacancy1 = new Vacancy()
            {
                Id = new Guid("674da545-6231-476e-e018-08dc3c4d373a"),
                PublishedBy = "Medi Solutions",
                PublishedDate = new DateTime(2024, 03, 04),
                NoOfVacancies = 3,
                MinimumQualification = "B. Tech",
                JobTitle = "Junior Quality Manager",
                JobDescription = "job description for junior quality manager job",
                ExperienceRequired = "3 to 6 years",
                LastDate = new DateTime(2024, 03, 29),
                MinimumSalary = 40000,
                MaximumSalary = 47000,
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new VacancyRepository(context);
            }

            // Act
            Vacancy result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                result = await repository.UpdateAsync(_updatedVacancy1);
            }

            //assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_Vacancy1_CheckVacancyDeletedFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new VacancyRepository(context);
                await repository.CreateAsync(_vacancy1);
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                await repository.DeleteAsync(_vacancy1);
            }

            //assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.VacancyDetails.FirstOrDefault(u => u.Id == new Guid("674da545-6231-476e-e018-08dc3c4d373a"));
                Assert.That(vacancyFromDb, Is.Null);
            }
        }

        [Test]
        public async Task CheckApplicationAsync_Application1ExistsAndApplication2DoesNotExists_ReturnAppropriateResponse()
        {
            // Arrange
            var application1 = new UserVacancyRequest()
            {
                Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                UserId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22"),
                ApplicationStatus = SD.Status_Submitted,
                AppliedDate = new DateTime(2024, 03, 04),
                TotalRecords = null,
            };
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.UserVacancyRequests.AddAsync(application1);
                await context.SaveChangesAsync();
            }

            // Act
            bool result1, result2;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new VacancyRepository(context);
                result1 = await repository.CheckApplicationAsync(new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22"), new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"));
                result2 = await repository.CheckApplicationAsync(new Guid("8EDBD66B-3289-4535-90EE-77448716C03A"), new Guid("674DA545-6231-476E-E018-08DC3C4D373A"));
            }

            // Assert
            Assert.That(result1, Is.True);
            Assert.That(result2, Is.False);
        }

        private class VacancyCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var vacancy1 = (Vacancy)x;
                var vacancy2 = (Vacancy)y;
                if (vacancy1.Id != vacancy2.Id)
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
