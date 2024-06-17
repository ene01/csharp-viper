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
using Viper.Game.Controls;

namespace Viper.Game.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button _clearTestingSpace = new()
        {
            Height = 25,
            Margin = new Thickness(5, 5, 5, 5),
            Content = "ViperButton",
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public MainWindow()
        {
            InitializeComponent();

            TestingSpacesButtons.Children.Add(_clearTestingSpace);

            _clearTestingSpace.Click += _clearTestingSpace_Click;
        }

        private void DisposeLastTest()
        {
            TestingSpace.Children.Clear();
            TestingInteractions.Children.Clear();
        }

        private void _clearTestingSpace_Click(object sender, RoutedEventArgs e)
        {
            DisposeLastTest();

            ViperButton vButton = new()
            {
                Content = "Hello world!",
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                s
            };

            TestingSpace.Children.Add(vButton);
        }
    }
}