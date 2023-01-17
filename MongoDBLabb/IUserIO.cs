using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBLabb
{
    internal interface IUserIO
    {
        string Input();
        void Output(string output);
        void Exit();
    }
}
