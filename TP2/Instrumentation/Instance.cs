using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2.BaB;

namespace TP2.Instrumentation
{
    /// <summary>
    /// Representa uma instância experimental
    /// </summary>
    public class Instance : INotifyPropertyChanged
    {
        public Instance(string filePath)
        {
            _filePath = filePath;
            Name = System.IO.Path.GetFileName(filePath);
        }

        private string _filePath;
        private string _name;
        private Tree _tree;
        private double _loadTime;
        private double _executionTime;
        private int _items;
        private int _conflicts;
        private int _nodes;
        private double _profit;
        private int _exploredNodes;

        /// <summary>
        /// Nome da instância
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Árvore alvo do experimento
        /// </summary>
        public Tree Tree
        {
            get => _tree;
            set
            {
                _tree = value;
                OnPropertyChanged(nameof(Tree));
            }
        }

        /// <summary>
        /// Tempo de carregamento da árvore
        /// </summary>
        public double LoadTime
        {
            get => _loadTime;
            set
            {
                _loadTime = value;
                OnPropertyChanged(nameof(LoadTime));
            }
        }

        /// <summary>
        /// Tempo de execução da construção da árvore
        /// </summary>
        public double ExecutionTime
        {
            get => _executionTime;
            set
            {
                _executionTime = value;
                OnPropertyChanged(nameof(ExecutionTime));
            }
        }

        /// <summary>
        /// Número de itens
        /// </summary>
        public int Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        /// <summary>
        /// Número de conflitos
        /// </summary>
        public int Conflicts
        {
            get => _conflicts;
            set
            {
                _conflicts = value;
                OnPropertyChanged(nameof(Conflicts));
            }
        }

        /// <summary>
        /// Número de nós resultates na árvore
        /// </summary>
        public int Nodes
        {
            get => _nodes;
            set
            {
                _nodes = value;
                OnPropertyChanged(nameof(Nodes));
            }
        }

        public double Profit
        {
            get => _profit;
            set
            {
                _profit = value;
                OnPropertyChanged(nameof(Profit));
            }
        }

        public int ExploredNodes
        {
            get => _exploredNodes;
            set
            {
                _exploredNodes = value;
                OnPropertyChanged(nameof(ExploredNodes));
            }
        }

        /// <summary>
        /// Faz o carregamento dos dados iniciais do arquivo
        /// </summary>
        private async Task LoadInstanceAsync()
        {
            await Task.Run(() =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var tree = FileReader.ReadFile(_filePath);

                watch.Stop();

                App.Current.Dispatcher.Invoke(() =>
                {
                    Tree = tree;
                    LoadTime = watch.ElapsedMilliseconds;
                    Items = tree.Items.Count;
                    Conflicts = tree.ConflictsCount;
                });
            });
        }

        /// <summary>
        /// Gera a árvore baseado no arquivo de entrada
        /// </summary>
        private async Task LoadTreeAsync()
        {
            await Task.Run(async () =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                await Tree.Build();

                watch.Stop();

                App.Current.Dispatcher.Invoke(() =>
                {
                    ExecutionTime = watch.ElapsedMilliseconds;
                    Nodes = Tree.Count(Tree.Root);
                    Profit = Tree.BestProfit;
                    ExploredNodes = Tree.ExploredNodes;
                });
            });
        }

        public async Task RunAsync()
        {
            await LoadInstanceAsync();
            await LoadTreeAsync();
        }





        /// <summary>
        /// Notifica a interface gráfica uma alteração de valor de alguma propriedade
        /// </summary>
        /// <param name="propertyName">Nome da propriedade</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
