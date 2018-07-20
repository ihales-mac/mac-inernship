using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hashing
{


    class Generate
    {
        static List<string> values = new List<string>();

        public static List<string> GeneratePermutations(int passlen) {
           DivePermutations("", 0, passlen);

            return values;

        }

       
       
        private static void DivePermutations(string prefix, int level, int maxlength)
        {
            level += 1;
 
            char[] alphas = "abcdefghijklmnopqrstuvwxyz".ToArray();
            foreach (char c in alphas){
                // Console.WriteLine(prefix + c);
                if (level < maxlength)
                {
                    DivePermutations(prefix + c, level, maxlength);
                }
                else
                {
                   // Console.WriteLine(prefix + c);







                    values.Add(prefix + c);
                }
            }
        }


    }


}
