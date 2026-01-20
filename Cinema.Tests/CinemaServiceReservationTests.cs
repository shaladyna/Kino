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
    public class CinemaServiceReservationTests
    {
        [TestMethod]
        public void CreateReservation_WhenSeatIsFree_ShouldReserveSeat()
        {
            var service = new CinemaService();

            var movie = new Movie(1, "Matrix", 136);
            service.AddMovie(movie);

            var screening = new Screening(1, movieId: 1, hallId: 1, start: DateTime.Today.AddHours(18), basePrice: 30m);
            service.AddScreening(screening);

            var customer = new Customer("Jan", "jan@test.com", "123456789");
            var seats = new List<Seat> { new Seat(1, 1) };
            var tickets = new List<Ticket> { new NormalTicket() };

            var reservation = service.CreateReservation(1, customer, seats, tickets);

            Assert.IsNotNull(reservation);
            Assert.IsTrue(service.IsSeatReserved(1, new Seat(1, 1)));
        }

        [TestMethod]
        public void CreateReservation_WhenSeatAlreadyReserved_ShouldThrow()
        {
            var service = new CinemaService();

            var movie = new Movie(1, "Matrix", 136);
            service.AddMovie(movie);

            var screening = new Screening(1, movieId: 1, hallId: 1, start: DateTime.Today.AddHours(18), basePrice: 30m);
            service.AddScreening(screening);

            var customer = new Customer("Jan", "jan@test.com", "123456789");
            var seats = new List<Seat> { new Seat(1, 1) };
            var tickets = new List<Ticket> { new NormalTicket() };

            service.CreateReservation(1, customer, seats, tickets);

            Assert.ThrowsException<SeatAlreadyReservedException>(() =>
                service.CreateReservation(1, customer, seats, tickets));
        }
    }
}
