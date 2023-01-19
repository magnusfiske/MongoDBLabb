﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal interface IDAO
    {
        void Create(Book book);
        List<Book> ReadAll();
        Book ReadOne(string title);
        Book ReadOneById(Book book);
        public Book ReadLatest();
        UpdateResult UpdateStock(Book objectToUpdate, int newStock);
        void Delete(Book book);
        public List<Book> Search(string searchValue);
    }
}
