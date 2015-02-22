using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PageRank
{
    class Program
    {
        static void Main(string[] args)
        {
            //var m = new double[,] {     { 0, 0.5, 0.5 }, 
            //                            { 0,   0,    1}, 
            //                            { 1,   0,    0}
            //                    };



            //var rank = Rank.Calculate(m);


            //List<Node> graph = new List<Node> { 
            //    new Node { OutgoingLinks = new List<int> { 1, 2 },  IncomingLinks = new List<int> { 2  } }, 
            //    new Node { OutgoingLinks = new List<int> { 2 },     IncomingLinks = new List<int> { 1  } },
            //    new Node { OutgoingLinks = new List<int> { 0 },     IncomingLinks = new List<int> { 0, 1 } },
            //};

            //var graphRank = RankGraph.Calculate(graph);

            var nodesCount = 950000;
            var graph = new List<Node>(nodesCount - 1);
            for (var i = 0; i < nodesCount; i++)
            {
                graph.Add(new Node());
            }

            int totalLinks = 0;
            using (var sr = File.OpenText(@"C:\Users\Alex\Desktop\web-Google.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("#"))
                        continue;

                    var nums = line.Split('\t');


                    int fromNodeId = int.Parse(nums[0]);
                    int toNodeId = int.Parse(nums[1]);

                    graph[fromNodeId].OutgoingLinksCount++;
                    graph[toNodeId].IncomingLinks.Add(fromNodeId);

                    totalLinks++;
                };

            }

            var graphRank = RankGraph.Calculate(graph, 0.8, 1e-10);

            double sum = 0;
            for (int i = 0; i < graphRank.Length; i++)
            {
                sum += graphRank[i];
            }
        }
    }
}
