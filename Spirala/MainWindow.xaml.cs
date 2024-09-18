using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Spirala
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _startLength;
        public int _gap;
        public bool _isRecursive;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates the outer layer of the spiral and calls CreateSpiralPart to create the next layer.
        /// </summary>
        public void CreateSpiralRecursive()
        {
            //The length that current (smallest) line has.
            int currentLength = _startLength;
            //Offset from the sides of the screen.
            Point offset = new(0, 0);

            //Creates the outer layer.
            CreateLine(new(0, 0), new(currentLength, 0));
            CreateLine(new(currentLength, 0), new(currentLength, currentLength));
            CreateLine(new(currentLength, currentLength), new(0, currentLength));
            CreateLine(new(0, currentLength), new(0, 0 + _gap));
            offset.Y += _gap;
            currentLength -= _gap;

            CreateSpiralPart(currentLength, offset);
        }

        /// <summary>
        /// Creates a layer of the spiral and calls itself to create the next level of the spiral.
        /// </summary>
        /// <param name="currentLength"></param>
        /// <param name="offset"></param>
        public void CreateSpiralPart(int currentLength, Point offset)
        {
            if (!(currentLength > 0 && offset.X * 2 <= _startLength && offset.Y * 2 <= _startLength))
                return;

            CreateLine(new(offset.X, offset.Y), new(currentLength, offset.Y));
            CreateLine(new(currentLength, offset.Y), new(currentLength, currentLength));
            CreateLine(new(currentLength, currentLength), new(offset.Y, currentLength));
            CreateLine(new(offset.X, currentLength + _gap), new(offset.X, offset.Y));
            offset.Y += _gap;
            offset.X += _gap;
            currentLength -= _gap;

            CreateSpiralPart(currentLength, offset);
        }

        /// <summary>
        /// Creates a line on the canvas.
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="endLine"></param>
        public void CreateLine(Point startLine, Point endLine)
        {
            Line line = new();
            line.Stroke = Brushes.Black;
            line.X1 = startLine.X;
            line.Y1 = startLine.Y;
            line.X2 = endLine.X;
            line.Y2 = endLine.Y;
            Field.Children.Add(line);
        }

        /// <summary>
        /// Creates a spiral withouth using recursion.
        /// </summary>
        public void CreateSpiralNonRecursive()
        {
            //The length that current (smallest) line has.
            int currentLength = _startLength;
            //Offset from the sides of the screen.
            Point offset = new(0, 0);

            //Creates the outer layer.
            CreateLine(new(0,0),new(currentLength, 0));
            CreateLine(new(currentLength, 0), new(currentLength, currentLength));
            CreateLine(new(currentLength, currentLength), new(0, currentLength));
            CreateLine(new(0, currentLength), new(0,0+_gap));
            offset.Y += _gap;
            currentLength -= _gap;

            //Creates all of the inner layers.
            while (currentLength > 0 && offset.X*2 <= _startLength && offset.Y * 2 <= _startLength)
            {
                CreateLine(new(offset.X, offset.Y), new(currentLength, offset.Y));
                CreateLine(new(currentLength, offset.Y), new(currentLength, currentLength));
                CreateLine(new(currentLength, currentLength), new(offset.Y, currentLength));
                CreateLine(new(offset.X, currentLength + _gap), new(offset.X, offset.Y));
                offset.Y += _gap;
                offset.X += _gap;
                currentLength -= _gap;
            }
        }

        /// <summary>
        /// Gets input from WPF elements and start render process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Render(object sender, RoutedEventArgs e)
        {
            //Input error checking and setting values.
            if(!int.TryParse(LengthInput.Text, out _startLength))
            {
                MessageBox.Show("Incorrect length format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(GapInput.Text, out _gap))
            {
                MessageBox.Show("Incorrect gap format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _isRecursive = RecursionInput.IsChecked ?? false;

            //Number size error checking.
            if (_gap <= 0)
            {
                MessageBox.Show("Gap is too small.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_startLength <= 0)
            {
                MessageBox.Show("Length is too small.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_gap >= _startLength)
            {
                MessageBox.Show("Gap is too big compared to length.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Menus.Visibility = Visibility.Collapsed;
            if (_isRecursive)
                CreateSpiralRecursive();
            else
                CreateSpiralNonRecursive();
        }
    }
}