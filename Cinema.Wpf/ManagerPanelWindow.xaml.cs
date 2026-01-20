using System;
using System.Linq;
using System.Windows;
using Cinema.Application;
using Cinema.Domain;

namespace Cinema.Wpf
{
    public partial class ManagerPanelWindow : Window
    {
        public ManagerPanelWindow(CinemaService service)
        {
            InitializeComponent();
            CalculateStats(service);
        }

        private void CalculateStats(CinemaService s)
        {
            var reservations = s.GetReservations();

            if (reservations == null || !reservations.Any()) return;

            decimal total = 0;
            foreach (var res in reservations)
            {
                var screening = s.GetScreening(res.ScreeningId);
                foreach (var ticket in res.Tickets)
                {
                    total += ticket.GetPrice(screening.BasePrice);
                }
            }
            TxtTotalEarnings.Text = $"{total:C}";

            var movieStats = reservations
                .GroupBy(r => s.GetMovie(s.GetScreening(r.ScreeningId).MovieId).Title)
                .Select(g => new { Title = g.Key, TicketCount = g.Sum(r => r.Tickets.Count) })
                .OrderByDescending(x => x.TicketCount)
                .FirstOrDefault();

            if (movieStats != null)
            {
                TxtPopularMovie.Text = movieStats.Title;
                TxtTicketCount.Text = $"{movieStats.TicketCount} sprzedanych biletów";
            }

            int normal = reservations.Sum(r => r.Tickets.Count(t => t.Name == "Normal"));
            int student = reservations.Sum(r => r.Tickets.Count(t => t.Name == "Student"));
            int vip = reservations.Sum(r => r.Tickets.Count(t => t.Name == "VIP"));

            TxtTicketStats.Text = $"Normalne: {normal} | Studenckie: {student} | VIP: {vip}";
        }
    }
}