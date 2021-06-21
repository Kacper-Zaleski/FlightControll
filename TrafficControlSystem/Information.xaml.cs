using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TrafficControlSystem
{
    public partial class Information : Window
    {
        public Information()
        {
            InitializeComponent();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e) // OK Button Mouse Enter, changing look
        {
            ButtonOK.Foreground = Brushes.Gold;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e) // OK Button Mouse Leave, changing look
        {
            ButtonOK.Foreground = Brushes.White;
        }
        private void MouseEnter_object(object sender, MouseEventArgs e) // Objects Mouse Enter, changing Text for information
        {
            Border border = sender as Border;
            TextBlock_Object.Text = border.Name;
        }
        private void Button_Click(object sender, RoutedEventArgs e) // OK Button Mouse Click, close the window
        {
            Close();
        }
    }
}
