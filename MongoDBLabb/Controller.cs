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
                io.Output("----------------------------\nVälj vad du vill göra genom att ange en bokstav:" +
                    "\nc - create\nr - read\nu - update\nd - delete\ne - exit program\nx - clear window\n");
                string input = io.Input();
                switch (input)
                {
                    case "c":
                        AddProduct();
                        break;
                    case "r":
                        Read();
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
                    case "x":
                        io.Clear();
                        break;
                    default:
                        io.Output("Felaktig inmatning, försök igen");
                        break;
                }
            }
        }

        private void Read()
        {
            bool isOn = true;
            do
            {
                io.Output("Välj hur du vill läsa genom att ange en bokstav:\na - hämta alla poster\n" +
                    "b - hämta en specifik titel\nc - sök på författare");
                try
                {
                    string input = io.Input().ToLower();
                    isOn = false;
                    switch (input)
                    {
                        case "a":
                            GetAllProducts();
                            break;
                        case "b":
                            GetOneProduct();
                            break;
                        case "c":
                            Search();
                            break;
                        default:
                            io.Output("Felaktig inmatning, försök igen");
                            isOn = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    io.Output(ex.Message);
                }
            } while (isOn);
        }

        private void AddProduct()
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

        private void GetAllProducts()
        {
            var products = db.ReadAll();

            foreach (var product in products)
            {
                io.Output(product.ToString());
            }
        }

        private void GetOneProduct()
        {
            io.Output("Ange titeln på den produkt du vill hämta");
            string title = io.Input();
            var result = db.ReadOne(title);
            if (result is null)
                io.Output("Ingen matchande titel i batabasen");
            else
                io.Output(result.ToString());
            
        }

       private void Search()
        {
            io.Output("Ange söksträng:");
            var result = db.Search(io.Input());
            if (result is null)
                io.Output("Ingen matchande produkt i databasen");
            else
            {
                foreach (Book book in result)
                {
                    io.Output(book.ToString());
                }
            }
        }

        private void UpdateProduct()
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
            try
            {
                var result = db.UpdateStock(bookToUpdate, bookToUpdate.stock + addedStock);
                if (result is null)
                    io.Output("Något gick fel");
                else
                    io.Output("Produkten uppdaterad:\n" + db.ReadOne(title).ToString());
            }
            catch (Exception ex)
            {
                io.Output(ex.Message);
            }
        }

        private void DeleteProduct()
        {
            bool isOn = true;
            while (isOn)
            {
                io.Output("Ange titeln för produkten du vill ta bort:");
                string title = io.Input();

                try
                {
                    var bookToDelete = db.ReadOne(title);
                    if (bookToDelete is null)
                        io.Output("Ingen matchande produkt i databasen");
                    else
                    {
                        io.Output("Är du säker på att du vill ta bort:");
                        io.Output(bookToDelete.ToString());
                        io.Output("Åtgärden går inte att ångra. (j/n)");
                        bool isValid = false;
                        while (!isValid)
                        if (io.Input() == "j")
                        {
                            isValid = true;
                            db.Delete(bookToDelete);
                            io.Output("Produkten raderad!");
                            isOn = false;
                        }
                        else if (io.Input() != "n")
                        {
                            io.Output("Felaktig inmatning");
                        }
                    }
                }
                catch (Exception ex)
                {
                    io.Output(ex.Message);
                }
            }
        }
    }
}
