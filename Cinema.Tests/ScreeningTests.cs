using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cinema.Application;
using Cinema.Domain;
using System;
using System.Linq;
using System.Collections.Generic;


namespace Cinema.Tests
{
    [TestClass]
    public class ScreeningTests
    {
        [TestMethod]
        public void GetScreenings_FilterByDate_ReturnsOnlyMatchingDay()
        {
            var service = new CinemaService();

            var movie = new Movie(1, "Avatar", 180);
            service.AddMovie(movie);

            var today = DateTime.Today.AddHours(14);
            var tomorrow = DateTime.Today.AddDays(1).AddHours(14);

            service.AddScreening(new Screening(101, 1, 1, today, 20m));
            service.AddScreening(new Screening(102, 1, 1, tomorrow, 20m));

            var results = service.GetScreenings(date: DateTime.Today);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(today.Date, results[0].Start.Date);
        }

        [TestMethod]
        public void GetScreenings_FilterByTitle_ReturnsOnlyMatchingMovies()
        {
            var service = new CinemaService();

            service.AddMovie(new Movie(1, "Shrek", 90));
            service.AddMovie(new Movie(2, "Matrix", 130));

            service.AddScreening(new Screening(201, 1, 1, DateTime.Now, 20m));
            service.AddScreening(new Screening(202, 2, 1, DateTime.Now, 25m));

            var results = service.GetScreenings(titleContains: "rek");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MovieId);
        }

        [TestMethod]
        public void CreateReservation_IncreasesReservationCount()
        {
            var service = new CinemaService();
            service.AddMovie(new Movie(1, "Film", 120));
            service.AddHall(new Hall(1, "Sala 1", 5, 5));
            service.AddScreening(new Screening(1, 1, 1, DateTime.Now, 20m));

            var customer = new Customer("Jan Kowalski", "jan@test.com", "123456789");
            var seats = new List<Seat> { new Seat(1, 1) };
            var tickets = new List<Ticket> { new NormalTicket() };

            service.CreateReservation(1, customer, seats, tickets);

            Assert.AreEqual(1, service.GetReservations().Count);
        }
    }
}