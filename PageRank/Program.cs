using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace PageRank
{
    class Program
    {
        public static int IntParseFast(string value, int offset = 0, int len = -1)
        {
            if (len == -1)
            {
                len = value.Length - offset;
            }

            // An optimized int parse method.
            int result = 0;
            for (int i = 0; i < len; i++)
            {
                result = 10 * result + (value[i + offset] - 48);
            }
            return result;
        }

        static void MainTopicSpecific(string[] args)
        {
            var graph = new [] { 
                new Node { IncomingLinks = new List<int> { 1 },    OutgoingLinksCount = 2 }, 
                new Node { IncomingLinks = new List<int> { 0 },    OutgoingLinksCount = 1 }, 
                new Node { IncomingLinks = new List<int> { 0, 3 }, OutgoingLinksCount = 1 }, 
                new Node { IncomingLinks = new List<int> { 2 },    OutgoingLinksCount = 1 }, 
            };

            var graphRank = RankGraph.CalculateTopicSpecific(graph, new Dictionary<int, double> { {0, 2/3.0}, {1, 1/3.0} }, 0.7, 0.0001);


            double sumRank = 0;
            for (int i = 0; i < graph.Length; i++)
            {
                sumRank += graphRank[i];
                Console.WriteLine("TSPR({0}) = {1}", i +1, graphRank[i]);
            }
        }

        static void Main(string[] args)
        {
            var graph = new Node[920000];


            int totalLinks = 0;
            int totalNodes = 0;
            var sw = new Stopwatch();
            sw.Start();

            using (var sr = File.OpenText(@"C:\Users\Alex\Desktop\web-Google.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line[0] == '#')
                        continue;

                    var separatorIdx = line.IndexOf('\t');

                    int fromNodeId = IntParseFast(line, 0, separatorIdx);
                    int toNodeId = IntParseFast(line, separatorIdx + 1);

                    if (graph[fromNodeId] == null)
                    {
                        graph[fromNodeId] = new Node();
                        totalNodes++;
                    }

                    if (graph[toNodeId] == null)
                    {
                        graph[toNodeId] = new Node();
                        totalNodes++;
                    }

                    graph[fromNodeId].OutgoingLinksCount++;
                    graph[toNodeId].IncomingLinks.Add(fromNodeId);

                    totalLinks++;
                };

            }

            TimeSpan readTime = sw.Elapsed;
            Console.WriteLine(sw.Elapsed);

            var graphRank = RankGraph.Calculate(graph, 0.8, 1e-10, totalNodes);
            TimeSpan calcTime = sw.Elapsed - readTime;
            sw.Stop();

            double sum = 0;
            for (int i = 0; i < graphRank.Length; i++)
            {
                sum += graphRank[i];
            }

            Console.WriteLine("99th rank: {0:e} Sum: {1} ReadTime: {2} CalcTime: {3}", graphRank[99], sum, readTime, calcTime);
        }
    }
}
