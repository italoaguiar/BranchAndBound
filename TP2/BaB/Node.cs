using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2.BaB
{
    public class Node
    {
        public Node()
        {
            Childs = new List<Node>();
        }

        public Item Item { get; set; }

        public Node Parent { get; set; }

        public double CurrentProfit { get; set; }

        public double Bound { get; set; }

        public IList<Node> Childs { get; set; }

        public override string ToString()
        {
            return Item?.Name + $"({CurrentProfit})";
        }
    }
}
