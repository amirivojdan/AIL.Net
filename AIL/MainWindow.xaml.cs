using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using AIL.Clustering;
using AIL.LineSimplification;
using AIL.Shared;

namespace AIL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<IClusterableLocation> _mapLocations;
        private KMeansClustering _kMeansClustering;
        private AglomerativeClustering _aglomerativeClustering;
        public MainWindow()
        {
            InitializeComponent();
            _mapLocations = new List<IClusterableLocation>();
            _kMeansClustering = new KMeansClustering(1000);
        }

        private void Button_KMeansCalc_Click(object sender, RoutedEventArgs e)
        {
            _kMeansClustering.NumberOfClusters = Convert.ToInt32(TextBox_NumberOfClusters.Text);
            var result = _kMeansClustering.Learn(_mapLocations);
            Visualize(result);
        }

        private void Visualize(List<IClusterableLocation> locations)
        {
            var colors = new List<Color>
            {
                Colors.Red,
                Colors.Yellow,
                Colors.DeepPink,
                Colors.Green,
                Colors.MediumBlue,
                Colors.OrangeRed
            };

            Canvas_Main.Children.Clear();
            foreach (var location in locations)
            {
                DrawPoint(location, colors[location.ClusterId]);
            }

        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(Canvas_Main);
            var mapLocation = new MapLocation
            {
                X = p.X,
                Y = p.Y,
                Weight = 1,
            };
            DrawPoint(mapLocation, Colors.Green);
            _mapLocations.Add(mapLocation);
            Label_NumberOfPoints.Content = _mapLocations.Count.ToString();

        }

        private void Canvas_Main_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(Canvas_Main);
            Label_X.Content = "X : " + p.X;
            Label_Y.Content = "Y : " + p.Y;
        }

        private void DrawPoint(ILocatable point, Color color, int multiplier = 1)
        {
            var brush = new SolidColorBrush(color);
            var ellipse = new Ellipse { Width = 6 * multiplier, Height = 6 * multiplier, Stroke = brush, Fill = brush };
            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);
            Canvas_Main.Children.Add(ellipse);
        }

        private void DrawRect(IWindow window)
        {
            var rec = new Rectangle()
            {
                Width = window.Width,
                Height = window.Height,

                Stroke = Brushes.Green,
                StrokeThickness = 1,
            };


            Canvas.SetLeft(rec, window.Center.X - window.Width / 2);
            Canvas.SetTop(rec, window.Center.Y - window.Height / 2);
            Canvas_Main.Children.Add(rec);
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            _mapLocations.Clear();
            Canvas_Main.Children.Clear();
        }

        private void Button_LineSimplification_Click(object sender, RoutedEventArgs e)
        {
            var simplifier = new RamerDouglasPeucker(5);
            _mapLocations = simplifier.DouglasPeuckerReduction(_mapLocations);
            Canvas_Main.Children.Clear();
            foreach (var clusterableLocation in _mapLocations)
            {
                DrawPoint(clusterableLocation, Colors.Green);
            }
        }

        public void DrawLocations(List<IClusterableLocation> locations)
        {
            Canvas_Main.Children.Clear();
            foreach (var loc in locations)
            {
                DrawPoint(loc, Colors.Green, loc.Weight);
            }
        }

        private async void Button_AglomerativeClustering_Click(object sender, RoutedEventArgs e)
        {
            var aglomerative = new AglomerativeClustering();
            var layers = aglomerative.Learn(_mapLocations);
            DrawLocations(layers.LastOrDefault());

        }

        private void Button_DrawContainingWindow_Click(object sender, RoutedEventArgs e)
        {
            var rect = Helper.GetContainingWindow(_mapLocations);
            DrawRect(rect);
        }

        private void Button_MeanShift_Click(object sender, RoutedEventArgs e)
        {
            var meanShift = new MeanShiftClustering();
            var clusters = meanShift.Learn(_mapLocations, 3);
            foreach (var cluster in clusters)
            {
                DrawRect(cluster);
            }

        }
    }
}
