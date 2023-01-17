using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal class ConsoleUserIO : IUserIO
    {
        public string Input()
        {
            try
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    throw new IOException("Falaktig inmatning");
                else
                    return input;
            }
            catch(Exception ex)
            {
                return "Felaktig inmatning\n" + ex.Message;
            }
        }

        public void Output(string output)
        {
            Console.WriteLine(output);
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}
