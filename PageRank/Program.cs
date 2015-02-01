using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new double[,] {     { 0, 0.5, 0.5 }, 
                                        { 0,   0,    1}, 
                                        { 1,   0,    0}
                                };



            var rank = Rank.Calculate(m);
        }
    }
}
