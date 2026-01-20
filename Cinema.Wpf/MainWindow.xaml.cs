using Cinema.Application;
using Cinema.Application.Storage;
using Cinema.Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

        private void BtnManager_Click(object sender, RoutedEventArgs e)
        {
            var managerWin = new ManagerPanelWindow(_cinemaService);
            managerWin.Owner = this;
            managerWin.ShowDialog();
            RefreshList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "JSON (*.json)|*.json",
                    FileName = "cinema-state.json"
                };

                if (dlg.ShowDialog() != true) return;

                var storage = new JsonStorageService();
                var state = _cinemaService.ExportState();
                storage.Save(dlg.FileName, state);

                MessageBox.Show("Zapisano stan kina do pliku.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu: " + ex.Message);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new OpenFileDialog
                {
                    Filter = "JSON (*.json)|*.json"
                };

                if (dlg.ShowDialog() != true) return;

                if (!System.IO.File.Exists(dlg.FileName))
                {
                    MessageBox.Show("Wybrany plik nie istnieje.");
                    return;
                }

                var storage = new JsonStorageService();
                var state = storage.Load(dlg.FileName);

                _cinemaService.ImportState(state);
                RefreshList();

                MessageBox.Show("Wczytano stan kina z pliku.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu: " + ex.Message);
            }
        }

    }
}