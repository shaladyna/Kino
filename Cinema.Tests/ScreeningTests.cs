using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cinema.Application;
using Cinema.Domain;
using System;
using System.Linq;

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

            var screeningToday = new Screening(1, movie.Id, 10, today, 20m);
            var screeningTomorrow = new Screening(2, movie.Id, 10, tomorrow, 20m);

            service.AddScreening(screeningToday);
            service.AddScreening(screeningTomorrow);

            var results = service.GetScreenings(date: DateTime.Today);

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(today, results[0].Start);
        }

        [TestMethod]
        public void GetScreenings_FilterByTitle_ReturnsOnlyMatchingMovies()
        {
            var service = new CinemaService();

            var movie1 = new Movie(1, "Shrek", 90);
            var movie2 = new Movie(2, "Matrix", 130);

            service.AddMovie(movie1);
            service.AddMovie(movie2);

            service.AddScreening(new Screening(1, 1, 1, DateTime.Now, 20m));
            service.AddScreening(new Screening(2, 2, 1, DateTime.Now, 25m));

            var results = service.GetScreenings(titleContains: "rek");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MovieId); 
        }
    }
}