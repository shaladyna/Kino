using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cinema.Application;
using Cinema.Domain;

namespace Cinema.Wpf
{
    public class SeatSelectionItem
    {
        public Seat Seat { get; set; } = null!;

        public string SeatDisplay => $"Rząd {Seat.Row}, M {Seat.Number}";
        public List<string> AvailableTicketTypes { get; } = new List<string> { "Normalny", "Studencki", "VIP" };
        public string SelectedTicketType { get; set; } = "Normalny";
    }

    public partial class SeatSelectionWindow : Window
    {
        private readonly CinemaService _service;
        private readonly Screening _screening;
        private readonly Hall _hall;

        private ObservableCollection<SeatSelectionItem> _basket = new ObservableCollection<SeatSelectionItem>();

        public SeatSelectionWindow(CinemaService service, Screening screening, Hall hall)
        {
            InitializeComponent();
            _service = service;
            _screening = screening;
            _hall = hall;

            TxtMovieTitle.Text = _service.GetMovieById(_screening.MovieId).Title;

            IcSelectedSeats.ItemsSource = _basket;
            GenerateSeats();
        }

        private void GenerateSeats()
        {
            GridSeats.Rows = _hall.Rows;
            GridSeats.Columns = _hall.SeatsPerRow;

            for (int r = 1; r <= _hall.Rows; r++)
            {
                for (int s = 1; s <= _hall.SeatsPerRow; s++)
                {
                    var seat = new Seat(r, s);
                    var btn = new Button { Content = $"{r}-{s}", Margin = new Thickness(3), Tag = seat };

                    if (_screening.ReservedSeats.Contains(seat))
                    {
                        btn.Background = Brushes.Crimson;
                        btn.IsEnabled = false;
                    }
                    else
                    {
                        btn.Background = Brushes.White;
                        btn.Click += Seat_Click;
                    }
                    GridSeats.Children.Add(btn);
                }
            }
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var seat = (Seat)btn.Tag;

            var existing = _basket.FirstOrDefault(x => x.Seat.Equals(seat));
            if (existing != null)
            {
                _basket.Remove(existing);
                btn.Background = Brushes.White;
            }
            else
            {
                _basket.Add(new SeatSelectionItem { Seat = seat });
                btn.Background = Brushes.SkyBlue;
            }
            UpdateTotalPrice();
        }

        private void TicketType_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateTotalPrice();

        private void UpdateTotalPrice()
        {
            decimal total = 0;
            foreach (var item in _basket)
            {
                Ticket t = CreateTicketObject(item.SelectedTicketType);
                total += t.GetPrice(_screening.BasePrice);
            }
            TxtTotalPrice.Text = $"{total:F2} zł";
        }

        private Ticket CreateTicketObject(string type)
        {
            switch (type)
            {
                case "Studencki":
                    return new StudentTicket();
                case "VIP":
                    return new VipTicket();
                default:
                    return new NormalTicket();
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_basket.Count == 0) throw new Exception("Koszyk jest pusty!");

                var customer = new Customer(TxtCustName.Text, TxtCustEmail.Text, TxtCustPhone.Text);

                var seats = _basket.Select(x => x.Seat).ToList();
                var tickets = _basket.Select(x => CreateTicketObject(x.SelectedTicketType)).ToList();

                var reservation = _service.CreateReservation(_screening.Id, customer, seats, tickets);

                MessageBox.Show($"Rezerwacja pomyślna!\nKoszt całkowity: {TxtTotalPrice.Text}");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message);
            }
        }
    }
}