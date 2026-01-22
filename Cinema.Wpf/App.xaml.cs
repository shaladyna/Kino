using System;
using System.Windows;
using Cinema.Application;
using Cinema.Domain;

namespace Cinema.Wpf
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var cinemaService = new CinemaService();
            SeedData(cinemaService);
            var window = new MainWindow(cinemaService);
            window.Show();
        }

        private void SeedData(CinemaService s)
        {
            try
            {
                var m1 = new Movie(1, "Avatar: Istota Wody", 192);
                var m2 = new Movie(2, "Oppenheimer", 180);
                s.AddMovie(m1);
                s.AddMovie(m2);

                s.AddHall(new Hall(1, "Sala IMAX", 8, 12));

                s.AddScreening(new Screening(101, 1, 1, DateTime.Now.AddHours(2), 25.00m));
                s.AddScreening(new Screening(102, 2, 1, DateTime.Now.AddDays(1).AddHours(10), 30.00m));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas ładowania danych: " + ex.Message);
            }
        }
    }
}