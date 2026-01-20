using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Cinema.Application;

namespace Cinema.Application.Storage
{
    public class JsonStorageService : ISaveLoad<CinemaState>
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true
        };

        public void Save(string path, CinemaState data)
        {
            var json = JsonSerializer.Serialize(data, Options);
            File.WriteAllText(path, json);
        }

        public CinemaState Load(string path)
        {
            var json = File.ReadAllText(path);
            var state = JsonSerializer.Deserialize<CinemaState>(json, Options);
            if (state is null) throw new InvalidDataException("Could not deserialize cinema state.");
            return state;
        }
    }
}