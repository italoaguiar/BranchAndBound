using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2.BaB
{
    public class FileReader
    {
        /// <summary>
        /// Processa o arquivo de entrada e retorna a estrutura da árvore
        /// </summary>
        /// <param name="filePath">Caminho para o arquivo</param>
        public static Tree ReadFile(string filePath)
        {
            Tree t = new Tree();

            using(StreamReader sr = new StreamReader(filePath))
            {
                var c = int.Parse(sr.ReadLine().Trim());
                var n = int.Parse(sr.ReadLine().Trim());

                List<Item> itens = new List<Item>();


                for(int i = 0; i< n; i++)
                {
                    var profit = sr.ReadLine();
                    itens.Add(new Item { Profit = double.Parse(profit), Id = i, Name = $"Item-{i + 1}" });
                }

                for (int i = 0; i < n; i++)
                {
                    var weight = sr.ReadLine();
                    itens[i].Weight = double.Parse(weight);
                }

                var m = int.Parse(sr.ReadLine());

                bool[,] conflicts = new bool[n, n];
                for(int i = 0; i< n; i++)
                {
                    conflicts[i, i] = true;
                }

                for(int i = 0; i< m; i++)
                {
                    var indexes = sr.ReadLine().Trim().Split(' ').Select(x=> int.Parse(x.Trim())).ToArray();

                    conflicts[indexes[0], indexes[1]] = true;
                    conflicts[indexes[1], indexes[0]] = true;                    
                }

                t.Capacity = c;
                t.Items = itens;
                t.Conflicts = conflicts;
                t.ConflictsCount = m;

                return t;
            }
        }
    }
}
