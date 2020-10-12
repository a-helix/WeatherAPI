using System;
using System.Collections.Generic;
using Credentials;
using IRepo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DatabaseClient
{
    public class MongoDatabaseClient : IRepository<ApiResponse>
    {
        private MongoClient _client;
        private string _url;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoDatabaseClient(string configPath, string database, string collection)
        {
            var databaseConfig = new JsonFileContent(configPath);
            _url = (string)databaseConfig.selectedParameter("databaseUrl");
            _client = new MongoClient(_url);
            _database = _client.GetDatabase(database);
            _collection = _database.GetCollection<BsonDocument>(collection);
        }

        public MongoDatabaseClient(string configPath, string database)
        {
            var databaseConfig = new JsonFileContent(configPath);
            _url = (string)databaseConfig.selectedParameter("databaseUrl");
            _client = new MongoClient(_url);
            _database = _client.GetDatabase(database);
            _collection = _database.GetCollection<BsonDocument>(database);
        }

        public bool Contains(string location)
        {
            try
            {
                var test = Get(location);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Create(ApiResponse coordinates)
        {
            BsonDocument document = BsonDocument.Parse(coordinates.ToString());
            _collection.InsertOne(document);
        }

        public void Delete(string location)
        {
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("geolocation", location);
            _collection.DeleteOne(deleteFilter);
        }

        public ApiResponse Get(string location)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("area", location);
            var results = _collection.Find(filter).FirstOrDefault();
            Dictionary<string, Object> dict = results.ToDictionary();
            var stringDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Object> entry in dict)
            {
                stringDict.Add(entry.Key, (string)entry.Value);
            }
            ApiResponse response = new ApiResponse(stringDict);
            return response;
        }

        public void Update(string geocoordinates, string newArea)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("geolocation", geocoordinates);
            var update = Builders<BsonDocument>.Update.Set("area", newArea);
            _collection.UpdateOne(filter, update);
        }
    }
}
