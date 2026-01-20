using Cinema.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application
{
    public class CinemaState
    {
        public List<Movie> Movies { get; set; } = new();
        public List<Screening> Screenings { get; set; } = new();
        public List<Reservation> Reservations { get; set; } = new();
        public List<Hall> Halls { get; set; } = new();
    }
}
