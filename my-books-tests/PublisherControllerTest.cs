using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;
using NUnit.Framework;
using System.Collections.Generic;
using my_books.Controllers;
using my_books.Data.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace my_books_tests
{
    public class PublishersControllerTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        AppDbContext context;
        PublishersService publishersService;
        PublishersController publishersController;


        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(context);
            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>());
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllPublishers_WithSortyBySearchStringPageNr_ReturnOk_Test()
        {
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc", "Publisher", 1);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            //Getting the publishers data from the return as a list of publishers
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;

            Assert.That(actionResultData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResultData.First().Id, Is.EqualTo(6));
            Assert.That(actionResultData.Count(), Is.EqualTo(5));


            IActionResult actionResultSecondPage = publishersController.GetAllPublishers("name_desc", "Publisher", 2);

            Assert.That(actionResultSecondPage, Is.TypeOf<OkObjectResult>());

            //Getting the publishers data from the return as a list of publishers for the second page
            var actionResultDataSecondPage = (actionResultSecondPage as OkObjectResult).Value as List<Publisher>;

            Assert.That(actionResultDataSecondPage.First().Name, Is.EqualTo("Publisher 1"));
            Assert.That(actionResultDataSecondPage.First().Id, Is.EqualTo(1));
            Assert.That(actionResultDataSecondPage.Count(), Is.EqualTo(1));
        }

        [Test, Order(2)]
        public void HTTPGET_GetPublisherById_Test()
        {
            var testPublisher = (publishersController.GetPublisherById(1) as OkObjectResult).Value as Publisher;

            Assert.That(testPublisher.Name, Is.EqualTo("Publisher 1"));
        }


        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {

            var publishers = new List<Publisher>
            {
                    new Publisher() {
                        Id = 1,
                        Name = "Publisher 1"
                    },
                    new Publisher() {
                        Id = 2,
                        Name = "Publisher 2"
                    },
                    new Publisher() {
                        Id = 3,
                        Name = "Publisher 3"
                    },
                    new Publisher() {
                        Id = 4,
                        Name = "Publisher 4"
                    },
                    new Publisher() {
                        Id = 5,
                        Name = "Publisher 5"
                    },
                    new Publisher() {
                        Id = 6,
                        Name = "Publisher 6"
                    },
            };
            context.Publishers.AddRange(publishers);

            context.SaveChanges();
        }

    }
}