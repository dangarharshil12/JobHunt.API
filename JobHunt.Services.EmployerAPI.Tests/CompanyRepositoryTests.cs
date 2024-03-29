using FluentAssertions;
using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace JobHunt.Services.EmployerAPI.Tests
{
    [TestFixture]
    public class CompanyRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> options;

        private Models.Employer _employer1;
        private Models.Employer _updatedEmployer1;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_Employer").Options;
        }

        public CompanyRepositoryTests()
        {
            _employer1 = new Models.Employer
            {
                Id = new Guid("59878AA6-BB5C-4A90-5A69-08DC39204FE5"),
                Organization = "Green Solutions",
                OrganizationType = "Agriculture",
                CompanyEmail = "info@greensolutions.com",
                CompanyPhone = "9988776655",
                NoOfEmployees = 100,
                StartYear = 2020,
                About = "Random Description",
                CreatedBy = "demoemployer@email.com",
                ImageUrl = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg",
            };

            _updatedEmployer1 = new Models.Employer
            {
                Id = new Guid("59878AA6-BB5C-4A90-5A69-08DC39204FE5"),
                Organization = "Medi Solutions",
                OrganizationType = "Healthcare",
                CompanyEmail = "info@medisolutions.com",
                CompanyPhone = "9988776655",
                NoOfEmployees = 50,
                StartYear = 2020,
                About = "Random Description",
                CreatedBy = "demoemployer@email.com",
                ImageUrl = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg",
            };
        }

        [Test]
        public async Task GetByNameAsync_OrganizationDoesNotExists_ReturnsNull()
        {
            // Arrange
            string name = "Medi Solutions";

            // Act
            Models.Employer result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                result = await repository.GetByNameAsync(name);
            }

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByNameAsync_OrganizationExists_ReturnsEmployerFromDatabase()
        {
            // Arrange
            string name = "Green Solutions";
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Employers.AddAsync(_employer1);
                await context.SaveChangesAsync();
            }

            // Act
            Models.Employer result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                result = await repository.GetByNameAsync(name);
            }

            // Assert
            Assert.That(result, Is.Not.Null);
            result.Should().BeEquivalentTo(_employer1);
        }

        [Test]
        public async Task GetByEmailAsync_OrganizationDoesNotExists_ReturnsNull()
        {
            // Arrange
            string email = "employerdoesnotexists@email.com";

            // Act
            Models.Employer result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                result = await repository.GetByEmailAsync(email);
            }

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByEmailAsync_OrganizationExists_ReturnsEmployerFromDatabase()
        {
            // Arrange
            string email = "demoemployer@email.com";
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Employers.AddAsync(_employer1);
                await context.SaveChangesAsync();
            }

            // Act
            Models.Employer result;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                result = await repository.GetByEmailAsync(email);
            }

            // Assert
            Assert.That(result, Is.Not.Null);
            result.Should().BeEquivalentTo(_employer1);
        }

        [Test]
        public async Task CreateAsync_AddEmployer1_CheckTheValuesFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                await repository.CreateAsync(_employer1);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.Employers.FirstOrDefault(u => u.Id == new Guid("59878AA6-BB5C-4A90-5A69-08DC39204FE5"));
                vacancyFromDb.Should().BeEquivalentTo(_employer1);
            }
        }

        [Test]
        public async Task UpdateAsync_OrganizationDoesNotExists_ReturnsNull()
        {
            // Arrange
            Models.Employer result;
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                result = await repository.UpdateAsync(_updatedEmployer1);
            }

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateAsync_OrganizationExists_ReturnsUpdatedEmployerFromDatabase()
        {
            // Arrange
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                await context.Employers.AddAsync(_employer1);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CompanyRepository(context);
                await repository.UpdateAsync(_updatedEmployer1);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var vacancyFromDb = context.Employers.FirstOrDefault(u => u.Id == new Guid("59878AA6-BB5C-4A90-5A69-08DC39204FE5"));
                vacancyFromDb.Should().BeEquivalentTo(_updatedEmployer1);
            }
        }
    }
}
