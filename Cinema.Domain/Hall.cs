using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain
{
    public class Hall
    {
        public int Id { get; }
        public string Name { get; }
        public int Rows { get; }
        public int SeatsPerRow { get; }

        public Hall(int id, string name, int rows, int seatsPerRow)
        {
            if (id < 1) throw new ArgumentException("Id must be >= 1.", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (rows < 1) throw new ArgumentException("Rows must be >= 1.", nameof(rows));
            if (seatsPerRow < 1) throw new ArgumentException("SeatsPerRow must be >= 1.", nameof(seatsPerRow));

            Id = id;
            Name = name.Trim();
            Rows = rows;
            SeatsPerRow = seatsPerRow;
        }

        public IEnumerable<Seat> GetAllSeats()
        {
            for (int r = 1; r <= Rows; r++)
                for (int s = 1; s <= SeatsPerRow; s++)
                    yield return new Seat(r, s);
        }
    }
}
