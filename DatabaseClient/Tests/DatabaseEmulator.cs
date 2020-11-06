using System;
using System.Collections.Generic;
using Credentials;
using Repository;

namespace DatabaseClients.Tests
{
    public class DatabaseEmulator : IRepository<ApiResponse>
    {
        protected Dictionary<string, ApiResponse> _database;

        public DatabaseEmulator()
        {
            _database = new Dictionary<string, ApiResponse>();
        }

        public void Create(ApiResponse coordinates)
        {
            JsonStringContent geolocation = new JsonStringContent(coordinates.ToString());
            var key = geolocation.Value("area").ToString();
            _database.Add(key, coordinates);
        }

        public void Delete(string location)
        {
            _database.Remove(location);
        }

        public ApiResponse Read(string location)
        {
            if(_database.ContainsKey(location))
                return _database[location];
            return null;
        }

        public void Update(string oldArea, string newArea)
        {
            try
            {
                var oldUnit = Read(oldArea);
                var newUnit = new Dictionary<string, string>()
                {
                    { "latitude",  oldUnit.Value("latitude") },
                    { "longitude", oldUnit.Value("longitude") },
                    { "geolocation", oldUnit.Value("geolocation") },
                    { "area", newArea }
                };
                _database.Remove(oldArea);
                _database.Add(newArea, new ApiResponse(newUnit));
            }
            catch(Exception)
            {
                throw new KeyNotFoundException();
            }

        }

        public bool Contains(string area)
        {
            return _database.ContainsKey(area);
        }
    }
}
