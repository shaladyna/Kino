using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
    }
    public class SeatAlreadyReservedException : Exception
    {
        public SeatAlreadyReservedException(string message) : base(message) { }

    }
    public class InvalidCustomerDataException : Exception
    {
        public InvalidCustomerDataException(string message) : base(message) { }
    }

}
