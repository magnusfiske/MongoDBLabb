using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal class Controller
    {
        private IDAO db;
        private IUserIO io;

        public Controller(IDAO db, IUserIO io)
        {
            this.db = db;
            this.io = io;
        }

        public void Start()
        {
            bool isOn = true;

            while (isOn)
            {
                io.Output("Välj vad du vill göra genom att ange en bokstav:\nc - create\nr - read\nu - update\nd - delete\ne - exit program");
                string input = io.Input();
                switch (input)
                {
                    case "c":
                        AddProduct();
                        break;
                    case "r":
                        Search();//GetProducts();
                        break;
                    case "u":
                        UpdateProduct();
                        break;
                    case "d":
                        DeleteProduct();
                        break;
                    case "e":
                        io.Exit();
                        break;
                    default:
                        io.Output("Felaktig inmatning, försök igen");
                        break;
                }
            }
        }

        public void AddProduct()
        {
            string title;
            string author;
            int stock = 0;
            io.Output("Ange titel:");
            title = io.Input();
            io.Output("Ange författare:");
            author = io.Input();

            bool isInt = false;
            while (isInt == false)
            {
                io.Output("Ange lagervärde:");
                isInt = Int32.TryParse(io.Input(), out stock);
                if (isInt == false)
                    io.Output("Ange lagervärde med ett numeriskt heltal.");
            }
                Book newBook = new Book(title, author, stock);
            db.Create(newBook);

            var created = db.ReadLatest();
            if (created.id == newBook.id)
                io.Output($"{created.title} sparades i databasen");
            else
                io.Output("Något gick fel, produkten kunde inte sparas i databasen.");
        }

        public void GetProducts()
        {
            var products = db.ReadAll();

            foreach (var product in products)
            {
                io.Output(product.ToString());
            }
        }

       public void Search()
        {
            db.Search(io.Input());
        }

        public void UpdateProduct()
        {
            io.Output("Ange titeln för produkten du vill uppdatera");
            string title = io.Input();
            int addedStock = 0;
            bool isInt = false;
            while (!isInt)
            {
                io.Output("Ange hur många ex som ska läggas till");
                isInt = Int32.TryParse(io.Input(), out addedStock);
            }

            var bookToUpdate = db.ReadOne(title);

            int newStock = bookToUpdate.stock + addedStock;

            db.UpdateStock(bookToUpdate, bookToUpdate.stock + addedStock);

        }

        public void DeleteProduct()
        {
            io.Output("Ange titeln för produkten du vill ta bort:");
            string title = io.Input();
            var bookToUpdate = db.ReadOne(title);
            io.Output("Är du säker på att du vill ta bort:");
            io.Output(bookToUpdate.ToString());
            io.Output("Åtgärden går inte att ångra. (j/n)");
            if (io.Input() == "j")
            {
                db.Delete(bookToUpdate);
                io.Output("Produkten raderad!");
            }

        }
    }
}
