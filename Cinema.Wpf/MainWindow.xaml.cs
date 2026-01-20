using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Cinema.Application;
using Cinema.Domain;

namespace Cinema.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly CinemaService _cinemaService;

        public MainWindow(CinemaService cinemaService)
        {
            InitializeComponent();
            _cinemaService = cinemaService;
            RefreshList();
        }

        private void RefreshList()
        {
            var screenings = _cinemaService.GetScreenings(DpDate.SelectedDate, TxtSearch.Text);
            var itemsForView = new List<object>();

            foreach (var s in screenings)
            {
                var movie = _cinemaService.GetMovie(s.MovieId);
                itemsForView.Add(new
                {
                    Id = s.Id,
                    Start = s.Start,
                    BasePrice = s.BasePrice,
                    MovieTitle = movie.Title,
                    DisplayDate = s.Start.ToString("dd.MM.yyyy HH:mm")
                });
            }

            IcScreenings.ItemsSource = itemsForView;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void BtnSelectSeats_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is int screeningId)
            {
                var screening = _cinemaService.GetScreening(screeningId);
                var hall = _cinemaService.GetHall(screening.HallId);

                var seatWindow = new SeatSelectionWindow(_cinemaService, screening, hall);
                seatWindow.Owner = this;
                seatWindow.ShowDialog();
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var state = _cinemaService.ExportState();
                MessageBox.Show($"Wyeksportowano dane! Liczba filmów: {state.Movies.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd eksportu: " + ex.Message);
            }
        }

        private void BtnManager_Click(object sender, RoutedEventArgs e)
        {
            var managerWin = new ManagerPanelWindow(_cinemaService);
            managerWin.Owner = this;
            managerWin.ShowDialog();
            RefreshList();
        }
    }
}