using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain
{
    public class Seat : System.IEquatable<Seat>
    {
        public int Row { get; set; }
        public int Number { get; set; }
        public bool Equals(Seat? other) => other != null && Row == other.Row && Number == other.Number;
        public override int GetHashCode() => (Row, Number).GetHashCode();
    }
}