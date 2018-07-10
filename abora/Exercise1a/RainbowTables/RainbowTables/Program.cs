using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowTables
{

    class Program
    {
        static void Main(string[] args)
        {
            UI ui = new UI(new Repository());
            ui.readInput();
        }     
    }

}
