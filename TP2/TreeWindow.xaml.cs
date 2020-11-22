using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TP2.BaB;
using TP2.Extensions;

namespace TP2
{
    /// <summary>
    /// Lógica interna para TreeWindow.xaml
    /// </summary>
    public partial class TreeWindow : Window
    {
        public TreeWindow(Node n)
        {
            InitializeComponent();

            Load(n);            
        }

        private async void Load(Node n)
        {
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                graph.Graph = n.ToGraphSharp();
            });
        }
    }
}
