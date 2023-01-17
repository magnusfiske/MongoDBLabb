using Microsoft.Extensions.Configuration;

namespace MongoDBLabb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("MongoSettings")["ConnectionString"];
            
            IDAO db = new MongoDAO(connectionString, "BookStore", "Books");
            IUserIO userIO = new ConsoleUserIO();


            Controller controller = new Controller(db, userIO);

            controller.Start();
        }
    }
}