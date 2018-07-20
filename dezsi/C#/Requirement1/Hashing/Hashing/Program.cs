using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashing{

    public class PerformanceMeasurements {


        //Reusable component from measurements
        public static void RunWithMeasurements(Action<Insertions.Loading> loadFunction, Insertions.Loading loading) {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            loadFunction(loading);
            watch.Stop();

            Console.WriteLine("Time elapsed while running insertion of type {0} is {1}. \n Press any key to continue...",loading, watch.Elapsed);
            Console.ReadLine();

        }

    }


    class Program
    {
            static void Main(string[] args){



           Insertions ins = new Insertions(5);

            PerformanceMeasurements.RunWithMeasurements(ins.LoadIntoDatabase, Insertions.Loading.InsertOneByOne);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine(HashesClass.ReverseHash("79a5d99c57bc9556dc76d3605f103e66", "MD5"));
            Console.WriteLine("Time elapsed while getting password: " + watch.Elapsed);
            watch.Stop();
            Console.ReadLine();



            /*
            watch.Start();
            ins.LoadIntoDatabase(Insertions.Loading.InsertAll);
            watch.Stop();
            Console.WriteLine("Time elapsed while inserting all one by one: " + watch.Elapsed);
            watch.Reset();
            */
            /*
            watch.Start();
            ins.LoadIntoDatabase(Insertions.Loading.MultipleRows);
            watch.Stop();
            Console.WriteLine("Time elapsed while inserting multiple rows: "+watch.Elapsed);
            watch.Reset();
            */
            /*
            watch.Start();
            ins.LoadIntoDatabase(Insertions.Loading.SingleRowMultiple);
            watch.Stop();
            Console.WriteLine("Time elapsed while inserting single row multiple times: ",watch.Elapsed);
            watch.Reset();
            */
            /*
             watch.Start();
             ins.LoadIntoDatabase(Insertions.Loading.BulkInsert);
             watch.Stop();
             Console.WriteLine("Time elapsed while bulk-inserting all rows: ", watch.Elapsed);
             Console.WriteLine(watch.Elapsed);

             watch.Reset();
               */


        }



    }
}
