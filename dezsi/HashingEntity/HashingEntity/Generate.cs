using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashingEntity
{
    class Generate
    {
        static List<string> values = new List<string>();

        public static List<string> GenerateArrangements(int passlen)
        {

            DiveArrangements("", 0, passlen);

            return values;

        }



        private static void DiveArrangements(string prefix, int level, int maxlength)
        {
            level += 1;

            char[] alphas = "abcdefghijklmnopqrstuvwxyz".ToArray();
            foreach (char c in alphas)
            {
                // Console.WriteLine(prefix + c);
                if (level < maxlength)
                {
                    DiveArrangements(prefix + c, level, maxlength);
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
