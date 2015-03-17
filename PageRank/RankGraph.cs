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
            return CalculateRank(graph, (i, leakedRank, totalNodes) => leakedRank / totalNodes, beta, epsilon, actualNodesCount);
        }

        public static double[] CalculateTopicSpecific(IList<Node> graph, IDictionary<int, double> nodeJumpWeights, double beta = 0.85, double epsilon = 0.001, int actualNodesCount = -1)
        {
            var sumJumpWeights = nodeJumpWeights.Values.Sum();
            if (Math.Abs(sumJumpWeights - 1) > epsilon)
            {
                throw new ArgumentException("jump weights must add up to 1", "nodeJumpWeights");
            }

            return CalculateRank(graph, (i, leakedRank, totalNodes) =>
            {
                if (nodeJumpWeights.ContainsKey(i))
                {
                    return leakedRank * nodeJumpWeights[i];
                }

                return 0;
            }, beta, epsilon, actualNodesCount);
        }

        private static double[] CalculateRank(IList<Node> graph, Func<int, double, int, double> distributeLeakedRank, double beta, double epsilon, int actualNodesCount)
        {
            const double totalRank = 1;

            if (actualNodesCount == -1)
                actualNodesCount = graph.Count;

            double[] prevRank = new double[graph.Count];
            double[] newRank = new double[graph.Count];

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
                Parallel.For(0, graph.Count, i => // calculate rank based on incoming links
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

                var totalLeakedRank = (totalRank - distributedRank);
                changedRanksCount = 0;
                for (int i = 0; i < graph.Count; i++) //re-insert leaked rank
                {
                    if (graph[i] == null)
                        continue;

                    newRank[i] += distributeLeakedRank(i, totalLeakedRank, actualNodesCount);

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
