using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain
{
    public class Reservation : ICloneable
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }
        public Customer Customer { get; set; }
        public List<Seat> Seats { get; set; }
        public List<Ticket> Tickets { get; set; }
        public DateTime CreatedAt { get; set; }

        public Reservation(int id, int screeningId, Customer customer, List<Seat> seats, List<Ticket> tickets, DateTime createdAt)
        {

            if (seats == null || seats.Count == 0)
                throw new ArgumentException("At least one seat must be selected.");

            if (tickets == null || tickets.Count == 0)
                throw new ArgumentException("At least one ticket must be assigned.");

            if (tickets.Count != seats.Count)
                throw new ArgumentException("Number of tickets must match the number of seats.");

            Id = id;
            ScreeningId = screeningId;
            Customer = customer;
            Seats = seats;
            Tickets = tickets;
            CreatedAt = createdAt;


        }
        public object Clone()
        {
            return new Reservation(
                this.Id,
                this.ScreeningId,
                this.Customer,
                new List<Seat>(this.Seats),
                new List<Ticket>(this.Tickets),
                this.CreatedAt);
        }
    }
}