using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;

namespace MongoDBLabb
{
    internal class MongoDAO : IDAO
    {
        private readonly MongoClient dbClient;
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Book> books;

        public MongoDAO(string connectionString, string database, string collection)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.LinqProvider = LinqProvider.V3;
            dbClient = new MongoClient(settings);
            this.database = dbClient.GetDatabase(database);
            books = this.database.GetCollection<Book>(collection);
        }

        public void Create(Book book)
        {
            books.InsertOneAsync(book).Wait();
        }

        public async void Delete(Book book)
        {
            var filter = Builders<Book>.Filter.Eq("_id", book.id);
            await books.DeleteOneAsync(filter);
        }

        public List<Book> ReadAll()
        {
            var result = books.Find(_ => true);
            return new List<Book>(result.ToList());
        }

        public Book ReadOne(string title)
        {
            var filter = Builders<Book>.Filter.Eq("title", title);
            return books.Find(filter).FirstOrDefault();

        }

        public Book ReadOneById(Book book)
        {
            var filter = Builders<Book>.Filter.Eq("_id", book.id);
            return books.Find(filter).FirstOrDefault();
        }

        public Book ReadLatest()
        {
            var sort = Builders<Book>.Sort.Descending("_id");
            var result = books.Find(_ => true).Sort(sort).Limit(1);
            return result.FirstOrDefault();
        }


        public void Search(string searchValue)
        {
            IMongoQueryable<Book> results = from book in books.AsQueryable()
                                            where book.author.Contains(searchValue)
                                            select book;

            foreach (Book book in results)
            {
                Console.WriteLine(book.ToString());
            }
            // Regex från Adam
            //    searchValue = Regex.Escape(searchValue);
            //    var expr = new BsonRegularExpression(new Regex(searchValue, RegexOptions.IgnoreCase));
        }

        public async void UpdateStock(Book objectToUpdate, int newStock)
        {
            var filter = Builders<Book>.Filter.Eq("_id", objectToUpdate.id);
            var update = Builders<Book>.Update.Set("stock", newStock);
            await books.UpdateOneAsync(filter, update);
        }
    }
}
