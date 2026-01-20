using System;

namespace Cinema.Domain
{
    public class Seat : IEquatable<Seat>
    {
        public int Row { get; }
        public int Number { get; }
        public Seat(int row, int number)
        {
            if (row < 1) throw new ArgumentException("Row must be > 0");
            if (number < 1) throw new ArgumentException("Number must be > 0");

            Row = row;
            Number = number;
        }
        public bool Equals(Seat? other)
        {
            if (other is null) return false;
            return Row == other.Row && Number == other.Number;
        }
        public override bool Equals(object? obj) => Equals(obj as Seat);
        public override int GetHashCode() => (Row, Number).GetHashCode();
        public override string ToString() => $"Rząd {Row}, Miejsce {Number}";
    }
}