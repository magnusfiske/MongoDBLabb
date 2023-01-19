using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBLabb
{
    internal class Book
    {
        public Book(string title, string author, int stock)
        {
            id = new ObjectId();
            this.title = title;
            this.author = author;
            this.stock = stock;
        }

        [BsonId]
        public ObjectId id { get; set; }
        [BsonElement]
        public string title { get; set; }
        [BsonElement]
        public string author { get; set; }
        [BsonElement]
        public int stock { get; set; }

        public override string ToString()
        {
            return $"\nTitle: {title}\nAuthor: {author}\nStock: {stock}\n---------------";
        }
    }
}
