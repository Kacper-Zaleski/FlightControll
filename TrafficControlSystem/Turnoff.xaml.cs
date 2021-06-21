using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TrafficControlSystem
{
    public partial class Turnoff : Window
    {
        public Turnoff()
        {
            InitializeComponent();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e) // No Button Mouse Enter, changing look
        {
            No.Background = Brushes.Green;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e) // No Button Mouse Leave, changing look
        {
            No.Background = Brushes.AliceBlue;
        }
        private void Button_MouseEnter_1(object sender, MouseEventArgs e) // Yes Button Mouse Enter, changing look
        {
            Yes.Background = Brushes.Red;
        }
        private void Button_MouseLeave_1(object sender, MouseEventArgs e) // Yes Button Mouse Leave, changing look
        {
            Yes.Background = Brushes.AliceBlue;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) => DialogResult = true; // Yes Button Result to MainWindow
        private void Button_Click(object sender, RoutedEventArgs e) => DialogResult = false; // No Button Result to MainWindow
    }
}
