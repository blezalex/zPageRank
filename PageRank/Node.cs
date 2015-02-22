using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    public class Node
    {
        public int OutgoingLinksCount = 0;

        public List<int> IncomingLinks = new List<int>();
    }
}
