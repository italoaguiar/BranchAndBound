using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TP2.BaB;
using TP2.Extensions;
using TP2.Instrumentation;

namespace TP2
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ListInstances();
        }

        private void ListInstances()
        {
            var files = Directory.GetFiles("Instances").Where(x=> !x.Contains("750") && !x.Contains("850") && !x.Contains("950")).ToArray();
            cbInstances.ItemsSource = files;
            cbInstances.SelectedIndex = 0;

            Instances = new List<Instance>();
            for(int i = 0; i< files.Length; i++)
            {
                Instances.Add(new Instance(files[i]));
            }
            lvInstances.ItemsSource = Instances;
 
        }

        List<Instance> Instances { get; set; }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {

            foreach(var i in Instances)
            {
                await i.RunAsync();

                await Task.Delay(100);
            }


            //totalização da entrada
            var s1 = new LiveCharts.Wpf.LineSeries() { Title = "Total de Itens", Values = new LiveCharts.ChartValues<int>(Instances.Select(x=> x.Items)) };
            var s2 = new LiveCharts.Wpf.LineSeries() { Title = "Total de Conflitos", Values = new LiveCharts.ChartValues<int>(Instances.Select(x => x.Conflicts)) };
            var sc1 = new LiveCharts.SeriesCollection() { s1, s2 };
            ReportEntry r1 = new ReportEntry() { Name = "Itens x Conflitos", Labels = Instances.Select(x=> x.Name).ToArray(), SeriesCollection = sc1 };


            //tempo de leitura
            var s3 = new LiveCharts.Wpf.LineSeries() { Title = "Tempo de leitura (ms)", Values = new LiveCharts.ChartValues<double>(Instances.Select(x => x.LoadTime)) };
            var sc2 = new LiveCharts.SeriesCollection() { s3};
            ReportEntry r2 = new ReportEntry() { Name = "Tempo gasto para ler o arquivo de entrada", Labels = Instances.Select(x => x.Name).ToArray(), SeriesCollection = sc2 };

            //tempo gasto para encontrar uma solução
            var s4 = new LiveCharts.Wpf.LineSeries() { Title = "Tempo de gasto (ms)", Values = new LiveCharts.ChartValues<double>(Instances.Select(x => x.ExecutionTime)) };
            var sc3 = new LiveCharts.SeriesCollection() { s4 };
            ReportEntry r3 = new ReportEntry() { Name = "Tempo gasto para gerar uma solução", Labels = Instances.Select(x => x.Name).ToArray(), SeriesCollection = sc3 };

            //número de nós explorados
            var s5 = new LiveCharts.Wpf.LineSeries() { Title = "Nós explorados", Values = new LiveCharts.ChartValues<int>(Instances.Select(x => x.ExploredNodes)) };
            var sc4 = new LiveCharts.SeriesCollection() { s5 };
            ReportEntry r4 = new ReportEntry() { Name = "Quantidade de nós explorados para gerar uma solução", Labels = Instances.Select(x => x.Name).ToArray(), SeriesCollection = sc4 };

            Report r = new Report();
            r.Entries = new List<ReportEntry>() { r1, r2, r3, r4 };

            ReportWindow rw = new ReportWindow(r);
            rw.Show();


            //var f = cbInstances.SelectedValue as string;

            //await Task.Run( async () =>
            //{
            //    var tree = FileReader.ReadFile(f);
            //    tree.Build();
            //    var total = tree.Count();
            //    //tree.PruneConflicts(tree.Root);
            //    var total2 = tree.Count();
            //    var k = tree;

            //    //await App.Current.Dispatcher.InvokeAsync(() =>
            //    //{
            //    //    graph.Graph = tree.Root.ToGraphSharp();
            //    //});

            //});
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Instance i = (sender as Button)?.Tag as Instance;

            if(i != null)
            {
                if(i.Nodes > 100)
                {
                    var r = MessageBox.Show("A ávore é muito grande, pode ser inviável plotá-la. Deseja prosseguir?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if(r == MessageBoxResult.Yes)
                    {
                        new TreeWindow(i.Tree.Root).Show();
                    }
                    return;
                }

                TreeWindow w = new TreeWindow(i.Tree.Root);
                w.Show();
            }
        }
    }
}
