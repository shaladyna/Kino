using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain
{
    public class Movie
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int DurationMinutes { get; private set; }

        public Movie(int id, string title, int durationMinutes)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (durationMinutes <= 0)
                throw new ArgumentException("Duration must be greater than 0.", nameof(durationMinutes));

            Id = id;
            Title = title;
            DurationMinutes = durationMinutes;
        }
        public void UpdateInfo(string title, int durationMinutes)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            Title = title;
            DurationMinutes = durationMinutes;
        }
    }
}