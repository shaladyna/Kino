using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cinema.Domain
{
    public class Screening : IComparable<Screening>
    {
        public int Id { get; private set; }
        public int MovieId { get; private set; }
        public int HallId { get; private set; }
        public DateTime Start { get; private set; }
        public decimal BasePrice { get; private set; }

        public HashSet<Seat> ReservedSeats { get; private set; }

        public Screening(int id, int movieId, int hallId, DateTime start, decimal basePrice)
        {
            if (basePrice <= 0)
                throw new ArgumentException("Price must be greater than 0.", nameof(basePrice));

            Id = id;
            MovieId = movieId;
            HallId = hallId;
            Start = start;
            BasePrice = basePrice;

            ReservedSeats = new HashSet<Seat>();
        }

        public int CompareTo(Screening? other)
        {
            if (other == null) return 1;
            return Start.CompareTo(other.Start);
        }
    }
    public class ScreeningByPriceComparer : IComparer<Screening>
    {
        public int Compare(Screening? x, Screening? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            return x.BasePrice.CompareTo(y.BasePrice);
        }
    }
}