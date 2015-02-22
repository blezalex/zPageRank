using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingles
{
    class Program
    {
        static void Main(string[] args)
        {
            int shingleSize = 2;
            var shinglesDoc1 = GetShingles("ABRACADABRA", shingleSize);
            var shinglesDoc2 = GetShingles("BRICABRAC", shingleSize);

            var common = shinglesDoc1.Keys.Where(k=> shinglesDoc2.ContainsKey(k)).ToArray();
        }

        private static new Dictionary<string, int> GetShingles(string doc, int shingleSize)
        {
            Dictionary<string, int> shingles = new Dictionary<string, int>();

            for (int i = 0; i < doc.Length - shingleSize + 1; i++)
            {
                string shingle = doc.Substring(i, shingleSize);
                if (shingles.ContainsKey(shingle))
                    shingles[shingle]++;
                else
                    shingles.Add(shingle, 1);
            }

            return shingles;
        }
    }
}
