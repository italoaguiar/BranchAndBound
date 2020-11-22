using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2.BaB;

namespace TP2.Extensions
{
    public static class GraphConverter
    {
        public static BidirectionalGraph<object, IEdge<object>> ToGraphSharp(this Node node, BidirectionalGraph<object, IEdge<object>> graph = null)
        {
            if (graph == null)
                graph = new BidirectionalGraph<object, IEdge<object>>();

            graph.AddVertex(node);

            if (node.Childs != null)
                foreach (var no in node.Childs)
                {
                    ToGraphSharp(no, graph);
                    graph.AddEdge(new Edge<object>(node, no));
                }

            return graph;
        }
    }
}
