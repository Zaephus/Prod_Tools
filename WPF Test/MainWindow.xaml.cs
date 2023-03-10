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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Prod_Tools {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            Start();
        }

        private string path = "C:/Users/marcc/Documents/HKU/J2.3/KERN_PROD_TOOLS/Prod_Tools/sprites/Coin v1.png";

        private void Start() {
            baseImage.Source = new BitmapImage(new Uri(path));
            hexImage.Source = new BitmapImage(new Uri(path));
        }
        
        public void Update() {
            if((Keyboard.GetKeyStates(Key.Enter) & KeyStates.Down) > 0) {
                ButtonAddName_Click(null, null);
            }
        }

        private void LoadImage(object _sender, RoutedEventArgs _e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +  
                                   "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +  
                                   "Portable Network Graphic (*.png)|*.png"; 
            
            if(openFileDialog.ShowDialog() == true) {
                baseImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                hexImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void ButtonAddName_Click(object _sender, RoutedEventArgs _e) {
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !lstNames.Items.Contains(txtName.Text)) {
                lstNames.Items.Add(txtName.Text);
                txtName.Clear();
            }
        }

        private void HexagonLoaded(object _sender, RoutedEventArgs _e) {
            Path hexagon = _sender as Path;
            CreateDataPath(hexagon.Width);
        }

        private void CreateDataPath(double _radius) {

            _radius -= hexPath.StrokeThickness;
            _radius *= 0.5f;

            Vector2[] points = {
                new Vector2(0, _radius),
                new Vector2((MathF.Sqrt(3) * _radius) / 2, _radius / 2),
                new Vector2((MathF.Sqrt(3) * _radius) / 2, -_radius / 2),
                new Vector2(0, -_radius),
                new Vector2(-(MathF.Sqrt(3) * _radius) / 2, -_radius / 2),
                new Vector2(-(MathF.Sqrt(3) * _radius) / 2, _radius / 2)
            };

            for(int i = 0; i < points.Length; i++) {
                points[i] += new Vector2(_radius, _radius);
            }

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(points[0].x, points[0].y);

            for(int i = 1; i < points.Length; i++) {
                LineSegment segment = new LineSegment();
                segment.Point = new Point(points[i].x + 0.5 * hexPath.StrokeThickness, points[i].y + 0.5 * hexPath.StrokeThickness);
                pathFigure.Segments.Add(segment);
            }

            pathFigure.IsClosed = true;

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            hexPath.Data = pathGeometry;

        }

    }

}