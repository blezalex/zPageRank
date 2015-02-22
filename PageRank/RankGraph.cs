﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageRank
{
    public class RankGraph
    {
        public static double[] Calculate(List<Node> graph, double beta = 0.85, double epsilon = 0.001)
        {
            const double totalRank = 1;

            double[] prevRank = new double[graph.Count];
            double[] newRank  = new double[graph.Count];

            for (int i = 0; i < graph.Count; i++)
            {
                prevRank[i] = totalRank / graph.Count;
            }

            double distributedRank = 0; //  leakedRank = totalRank - distributedRank
            double rankRedistributionAmount = 1;

            while (rankRedistributionAmount > epsilon)
            {
                distributedRank = 0;
                for (int i = 0; i < graph.Count; i++) // calculate rank based on incoming links
                {
                    var node = graph[i];

                    double nodeNewRank = 0;
                    foreach (var incomingNodeIdx in node.IncomingLinks)
                    {
                        nodeNewRank += beta * prevRank[incomingNodeIdx] / graph[incomingNodeIdx].OutgoingLinksCount;
                    }

                    newRank[i] = nodeNewRank;

                    distributedRank += nodeNewRank;
                }

                var leakedRankPerNode = (totalRank - distributedRank) / graph.Count;
                rankRedistributionAmount = 0;
                for (int i = 0; i < graph.Count; i++) //re-insert leaked rank
                {
                    newRank[i] += leakedRankPerNode;
                 
                    rankRedistributionAmount = Math.Max(rankRedistributionAmount, Math.Abs(newRank[i] - prevRank[i]));  // calc how much ranks have changed
                    prevRank[i] = newRank[i]; //  copy newRank to prev to prepare for next iteration
                }
            }

            return newRank;
        }
    }
}