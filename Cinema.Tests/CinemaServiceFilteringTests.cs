using Cinema.Application;
using Cinema.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Tests
{
    [TestClass]
    public class CinemaServiceFilteringTests
    {
        [TestMethod]
        public void GetScreenings_FilterByDate_ShouldReturnOnlyThatDate()
        {
            var service = new CinemaService();

            service.AddMovie(new Movie(1, "Matrix", 136));
            service.AddMovie(new Movie(2, "Inception", 148));

            service.AddScreening(new Screening(1, 1, 1, DateTime.Today.AddHours(18), 30m));
            service.AddScreening(new Screening(2, 2, 1, DateTime.Today.AddDays(1).AddHours(18), 30m));

            var result = service.GetScreenings(date: DateTime.Today);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }
    }
}
