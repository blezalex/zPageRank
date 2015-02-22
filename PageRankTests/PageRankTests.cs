using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PageRank;

namespace PageRankTests
{
    [TestClass]
    public class PageRankTests
    {
        [TestMethod]
        public void TestWikiRank()
        {
            var graph = new List<Node>();

            // 11 nodes total
            graph.Add(new Node { IncomingLinks = new List<int> { 3 } }); // A
            graph.Add(new Node { IncomingLinks = new List<int> { 3, 6, 7, 8 ,4, 5, 2 }, OutgoingLinksCount = 1 }); // B
            graph.Add(new Node { IncomingLinks = new List<int> { 1 }, OutgoingLinksCount = 1 }); //C
            graph.Add(new Node { IncomingLinks = new List<int> { 4 }, OutgoingLinksCount = 2 }); // D
            graph.Add(new Node { IncomingLinks = new List<int> { 6,7,8,9,10,5 }, OutgoingLinksCount = 3 }); // E
            graph.Add(new Node { IncomingLinks = new List<int> { 4 }, OutgoingLinksCount = 2 }); // F
            
            graph.Add(new Node { OutgoingLinksCount = 2 });
            graph.Add(new Node { OutgoingLinksCount = 2 }); 
            graph.Add(new Node { OutgoingLinksCount = 2 });

            graph.Add(new Node { OutgoingLinksCount = 1 });
            graph.Add(new Node { OutgoingLinksCount = 1 });

            var ranks = RankGraph.Calculate(graph, 0.85, 0.0001);

            AssertExt.AreClose(0.033, ranks[0], 0.0005);
            AssertExt.AreClose(0.384, ranks[1], 0.0005);
            AssertExt.AreClose(0.343, ranks[2], 0.0005);
            AssertExt.AreClose(0.039, ranks[3], 0.0005);
            AssertExt.AreClose(0.081, ranks[4], 0.0005);
            AssertExt.AreClose(0.039, ranks[5], 0.0005);
            AssertExt.AreClose(0.016, ranks[6], 0.0005);
            AssertExt.AreClose(0.016, ranks[7], 0.0005);
            AssertExt.AreClose(0.016, ranks[8], 0.0005);
            AssertExt.AreClose(0.016, ranks[9], 0.0005);
            AssertExt.AreClose(0.016, ranks[10], 0.0005);
        }
    }

    public static class AssertExt
    {
        public static void AreClose(double expected, double actual, double tolerance)
        {
            Assert.IsTrue(Math.Abs(expected - actual) < tolerance);
        }
    }
}
