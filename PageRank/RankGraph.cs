using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    public class RankGraph
    {
        public static double[] Calculate(IList<Node> graph, double beta = 0.85, double epsilon = 0.001, int actualNodesCount = -1)
        {
            const double totalRank = 1;

            if (actualNodesCount == -1)
                actualNodesCount = graph.Count;

            double[] prevRank = new double[graph.Count];
            double[] newRank  = new double[graph.Count];

            for (int i = 0; i < graph.Count; i++)
            {
                if (graph[i] == null)
                {
                    prevRank[i] = 0;
                }
                else
                {
                    prevRank[i] = totalRank / actualNodesCount;
                }
            }

            object rankLock = new object();
            double distributedRank = 0; //  leakedRank = totalRank - distributedRank
            int changedRanksCount = 1;

            while (changedRanksCount > 0)
            {
                distributedRank = 0;
                Parallel.For(0, graph.Count, i=> // calculate rank based on incoming links
                {
                    var node = graph[i];
                    if (node == null)
                        return;

                    double nodeNewRank = 0;
                    foreach (var incomingNodeIdx in node.IncomingLinks)
                    {
                        nodeNewRank += prevRank[incomingNodeIdx] / graph[incomingNodeIdx].OutgoingLinksCount;
                    }

                    nodeNewRank *= beta;

                    newRank[i] = nodeNewRank;

                    lock (rankLock)
                    {
                        distributedRank += nodeNewRank;
                    }
                });

                var leakedRankPerNode = (totalRank - distributedRank) / actualNodesCount;
                changedRanksCount = 0;
                for (int i = 0; i < graph.Count; i++) //re-insert leaked rank
                {
                    if (graph[i] == null)
                        continue;

                    newRank[i] += leakedRankPerNode;

                    if (changedRanksCount == 0) // calc how much ranks have changed
                    {
                        if (Math.Abs(newRank[i] - prevRank[i]) > epsilon)
                            changedRanksCount++;
                    }
  
                    prevRank[i] = newRank[i]; //  copy newRank to prev to prepare for next iteration
                }
            }

            return newRank;
        }
    }
}
