using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TrafficControlSystem
{
    public partial class AddFlight :  Window
    {
        public AddFlight()
        {
            InitializeComponent();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e) // Cancel Button Mouse Enter, changing look
        {
            Cancel.Background = Brushes.Red;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e) // Cancel Button Mouse Leave, changing look
        {
            Cancel.Background = Brushes.AliceBlue;
        }
        private void Button_MouseEnter_1(object sender, MouseEventArgs e) // Confirm Button Mouse Enter, changing look
        {
            Confirm.Background = Brushes.Green;
        }
        private void Button_MouseLeave_1(object sender, MouseEventArgs e) // Confirm Button Mouse Leave, changing look
        {
            Confirm.Background = Brushes.AliceBlue;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) => DialogResult = true; // Confirm Button Result to MainWindow
        private void Button_Click(object sender, RoutedEventArgs e) => DialogResult = false; // Cancel Button Result to MainWindow
    }
}
