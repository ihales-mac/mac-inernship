using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowTables
{
    class Utils
    {
        public Utils()
        {

        }

        static List<string> allCombinations = new List<string>();


        public static List<string> getPermutations()
        {
            char[] alphabet = new char[26];
            for (char x = 'a'; x <= 'z'; x++)
            {
                alphabet[x - 'a'] = x;
            }

            return getAllKLength(alphabet, 2);
        }


        // The method that prints all 
        // possible strings of length k.
        // It is mainly a wrapper over 
        // recursive function printAllKLengthRec()
        static List<string> getAllKLength(char[] set, int k)
        {
            int n = set.Length;
            getAllKLengthRec(set, "", n, k);
            return allCombinations;
        }

        // The main recursive method
        // get all possible 
        // strings of length k
        static void getAllKLengthRec(char[] set,
                                       String prefix,
                                       int n, int k)
        {

            // Base case: k is 0,
            // print prefix
            if (k == 0)
            {
                allCombinations.Add(prefix);
                return;
            }

            // One by one add all characters 
            // from set and recursively 
            // call for k equals to k-1
            for (int i = 0; i < n; ++i)
            {

                // Next character of input added
                String newPrefix = prefix + set[i];

                // k is decreased, because 
                // we have added a new character
                getAllKLengthRec(set, newPrefix,
                                        n, k - 1);
            }
        }
    }
}
