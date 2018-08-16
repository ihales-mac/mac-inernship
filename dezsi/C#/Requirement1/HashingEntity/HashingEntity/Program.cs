using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashingEntity
{
    public class PerformanceMeasurements
    {


        //Reusable component from measurements
        public static void RunWithMeasures(Action<Insertions.Loading> loadFunction, Insertions.Loading loading)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            loadFunction(loading);
            watch.Stop();

            Console.WriteLine("Time elapsed while running insertion of type {0} is {1}. \n Press any key to continue...", loading, watch.Elapsed);
            Console.ReadLine();

        }
    }


        class Program
    {
        static void Main(string[] args)
        {



            Insertions ins = new Insertions(3);

  
            PerformanceMeasurements.RunWithMeasures(ins.LoadIntoDatabase, Insertions.Loading.InsertSome);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine(HashesClass.ReverseHash("79a5d99c57bc9556dc76d3605f103e66", "MD5"));
            watch.Stop();
            Console.ReadLine();



        }



    }
}
