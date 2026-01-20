using Cinema.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application
{
        public class CinemaService
        {
            private readonly Dictionary<int, Movie> _movies = new();
            private readonly List<Screening> _screenings = new();
            private readonly List<Reservation> _reservations = new();
            private readonly Dictionary<int, Hall> _halls = new();

            public IReadOnlyList<Screening> GetScreenings(DateTime? date = null, string? titleContains = null)
            {
                IEnumerable<Screening> query = _screenings;

                if (date.HasValue)
                {
                    var d = date.Value.Date;
                    query = query.Where(s => s.Start.Date == d);
                }

                if (!string.IsNullOrWhiteSpace(titleContains))
                {
                    var text = titleContains.Trim();
                    query = query.Where(s =>
                    {
                        if (!_movies.TryGetValue(s.MovieId, out var movie)) return false;
                        return movie.Title.Contains(text, StringComparison.OrdinalIgnoreCase);
                    });
                }

                return query.OrderBy(s => s.Start).ToList();
            }

            public Screening GetScreening(int screeningId)
            {
                var screening = _screenings.FirstOrDefault(s => s.Id == screeningId);
                if (screening is null)
                    throw new EntityNotFoundException($"Screening with Id={screeningId} not found.");

                return screening;
            }

            public Movie GetMovie(int movieId)
            {
                if (!_movies.TryGetValue(movieId, out var movie))
                    throw new EntityNotFoundException($"Movie with Id={movieId} not found.");
                return movie;
            }

            public bool IsSeatReserved(int screeningId, Seat seat)
            {
                var screening = GetScreening(screeningId);
                return screening.ReservedSeats.Contains(seat);
            }

            public Reservation CreateReservation(int screeningId, Customer customer, List<Seat> seats, List<Ticket> tickets)
            {
                if (customer is null) throw new ArgumentNullException(nameof(customer));
                if (seats is null || seats.Count == 0) throw new ArgumentException("Seats list cannot be empty.", nameof(seats));
                if (tickets is null || tickets.Count == 0) throw new ArgumentException("Tickets list cannot be empty.", nameof(tickets));
                if (tickets.Count != seats.Count) throw new ArgumentException("Tickets.Count must equal Seats.Count.");

                var screening = GetScreening(screeningId);


                var hall = GetHall(screening.HallId);

                foreach (var seat in seats)
                {
                    if (seat.Row < 1 || seat.Row > hall.Rows ||
                        seat.Number < 1 || seat.Number > hall.SeatsPerRow)
                    {
                        throw new ArgumentException(
                            $"Seat {seat.Row}-{seat.Number} does not exist in hall {hall.Name}.",
                            nameof(seats)
                        );
                    }
                }

                if (seats.Distinct().Count() != seats.Count)
                    throw new ArgumentException("Seats list contains duplicates.", nameof(seats));

                foreach (var seat in seats)
                {
                    if (screening.ReservedSeats.Contains(seat))
                        throw new SeatAlreadyReservedException($"Seat {seat.Row}-{seat.Number} is already reserved.");
                }


                foreach (var seat in seats)
                    screening.ReservedSeats.Add(seat);


                int newId = (_reservations.Count == 0) ? 1 : _reservations.Max(r => r.Id) + 1;

                var reservation = new Reservation(
                    id: newId,
                    screeningId: screeningId,
                    customer: customer,
                    seats: seats,
                    tickets: tickets,
                    createdAt: DateTime.Now
                );

                _reservations.Add(reservation);
                return reservation;
            }

            public decimal CalculateReservationTotalPrice(int screeningId, List<Ticket> tickets)
            {
                var screening = GetScreening(screeningId);
                return tickets.Sum(t => t.GetPrice(screening.BasePrice));
            }

            public decimal GetReservationTotalPrice(Reservation reservation)
                => CalculateReservationTotalPrice(reservation.ScreeningId, reservation.Tickets);

  

            public CinemaState ExportState()
                => new CinemaState
                {
                    Movies = _movies.Values.ToList(),
                    Halls = _halls.Values.ToList(),
                    Screenings = _screenings.ToList(),
                    Reservations = _reservations.ToList()
                };

            public void ImportState(CinemaState state)
            {
                if (state is null) throw new ArgumentNullException(nameof(state));

                _movies.Clear();
                _halls.Clear();
                _screenings.Clear();
                _reservations.Clear();

                foreach (var m in state.Movies) _movies[m.Id] = m;
                foreach (var h in state.Halls) _halls[h.Id] = h;

                _screenings.AddRange(state.Screenings);
                _reservations.AddRange(state.Reservations);

            }


            public void AddMovie(Movie movie)
            {
                if (movie is null) throw new ArgumentNullException(nameof(movie));
                _movies[movie.Id] = movie;
            }

            public void AddScreening(Screening screening)
            {
                if (screening is null) throw new ArgumentNullException(nameof(screening));
                _screenings.Add(screening);
            }

            public void AddHall(Hall hall)
            {
                if (hall is null) throw new ArgumentNullException(nameof(hall));
                _halls[hall.Id] = hall;
            }

            public Hall GetHall(int hallId)
            {
                if (!_halls.TryGetValue(hallId, out var hall))
                    throw new EntityNotFoundException($"Hall with Id={hallId} not found.");
                return hall;
            }

            public List<Reservation> GetReservations()
            {
                return _reservations;
            }

        }
    }
}
