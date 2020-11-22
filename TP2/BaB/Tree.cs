using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TP2.BaB
{
    public class Tree
    {
        public int Capacity { get; set; }

        public bool [,] Conflicts { get; set; }

        public int ConflictsCount { get; set; }

        public IList<Item> Items { get; set; }

        public Node Root { get; set; }


        public async Task Build()
        {
            //ordena pelos itens de maior valor e menor peso
            Items = Items.OrderByDescending(x => x.Profit/x.Weight).ToList();

            Densities = new double[Items.Count];

            Root = new Node();

            Insert(Root, 0);

            
        }

        public int total = 0;

        public double BestProfit = 0;
        private double BestWeight = 0;
        public Node Bigger;
        public double[] Densities;
        
        public int ExploredNodes { get; set; } 


        /// <summary>
        /// Insere um nó na árvore
        /// </summary>
        /// <param name="n">Nó a ser inserido</param>
        /// <param name="currentCapacity">Capacidade atual no nível do nó</param>
        /// <param name="currentProfit">Lucro atual no nível do nó</param>
        private void Insert(Node n, double currentCapacity = 0, double currentProfit = 0, int level = 0)
        {

            foreach (var item in Items)
            {
                if (!ConflictsWith(item, n))
                {
                    //se o item ainda cabe na bolsa e ele ainda não foi colocado, insira-o
                    if ((currentCapacity + item.Weight) <= Capacity)
                    {
                        var bound = Bound(item, currentCapacity, currentProfit, level);
                        if (bound < BestProfit) continue;

                        Node node = new Node();
                        node.Item = item;
                        node.Parent = n;
                        node.CurrentProfit = currentProfit + item.Profit;
                        node.Bound = bound;
                        ExploredNodes++;

                        //se o nó gerar uma solução, inclua-o na árvore
                        //n.Childs.Add(node);
                        Insert(node, currentCapacity + item.Weight, currentProfit + item.Profit, level + 1);

                        //n.Childs.Remove(node);

                    }
                    else
                    {
                        //encontra a folha com o maior lucro
                        if (currentProfit > BestProfit)
                        {
                            BestProfit = currentProfit;
                            BestWeight = currentCapacity;
                            Bigger = n;

                            return;
                        }

                        return;
                    }
                }
            }
            return;
        }


        public double Bound(Item i, double currentWeight, double currentProfit, int level)
        {
            //return i.Profit + (Capacity - currentWeight) * Items[level + 1].Profit / Items[level + 1].Weight;

            int j = level;
            double wt = i.Weight;
            double bound = i.Profit;

            while (j < Items.Count && wt + Items[j].Weight <= Capacity)
            {
                bound += Items[j].Profit;
                wt += Items[j].Weight;
                j++;
            }

            if (j < Capacity)
                bound += (Capacity - wt) * (Items[j].Profit / Items[j].Weight);


            return bound;
        }


        /// <summary>
        /// Determina se um item já existem em um ramo da árvore
        /// </summary>
        /// <param name="newItem">Item</param>
        /// <param name="currentNode">Nó atual</param>
        private bool Exists(Item newItem, Node currentNode)
        {
            Node aux = currentNode;
            while (aux.Item != null)
            {
                if (newItem.Id == aux.Item.Id)
                    return true;

                aux = aux.Parent;
            }

            return false;
        }

        /// <summary>
        /// Determina se um item já existem em um ramo da árvore
        /// </summary>
        /// <param name="newItem">Item</param>
        /// <param name="currentNode">Nó atual</param>
        private bool ConflictsWith(Item newItem, Node currentNode)
        {
            Node aux = currentNode;
            while (aux.Item != null)
            {
                if (Conflicts[newItem.Id, aux.Item.Id])
                    return true;

                aux = aux.Parent;
            }

            return false;            
        }


        /// <summary>
        /// Conta quantos nós existem na árvore
        /// </summary>
        /// <param name="n">Nó raiz</param>
        public int Count(Node n = null)
        {
            if (n == null)
                n = Root;

            if (n.Childs == null || n.Childs.Count == 0)
                return 1;

            int total = 1;
            foreach(var item in n.Childs)
            {
                total += Count(item);
            }

            return total;
        }

        public void Print(Node n)
        {
            string r = "";
            while (n != null && n.Item != null)
            {
                r += n.Item.Name + " ";
                n = n.Parent;
            }

            MessageBox.Show(r);
        }
    }
}
