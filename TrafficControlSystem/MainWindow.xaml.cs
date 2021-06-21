using Objects;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TrafficControlSystem
{
    public partial class MainWindow : Window
    {
        private LandMap M = new LandMap(); // Land map
        private Airport a1, a2; // Containers for selected Airports
        private Radar R = new Radar(); // Radar
        private List<Border> ListOfPlaneBorders = new List<Border>(); // List of Borders => Aircrafts (Graphic)
        private Aircraft a; // Containers for selected Aircraft
        private DateTime time; // Conteiner for time
        private int free_index, HowMuch; // Free index for new objects, Intiger of generate Aircrafts left (method by time)
        private DispatcherTimer timer; // Timer for generator of Aircrafts
        public MainWindow()
        {
            InitializeComponent();
            
            time = DateTime.Now; // Collectin current time

            // Adding to list of Airport airports 
            M.AddAirport(new Airport(0, 482, 312, "Kennedy International Airport", "USA", "New_York_City", "Queens, Nowy Jork 11430"));
            M.AddAirport(new Airport(1, 837, 266, "London-City", "England", "London", "Hartmann Rd, London E16 2PX"));
            M.AddAirport(new Airport(2, 948, 273, "Chopina", "Poland", "Warsaw", "Żwirki i Wigury"));
            M.AddAirport(new Airport(3, 1659, 373, "Narita International Airport", "Japan", "Tokyo", "1-1 Furugome"));
            M.AddAirport(new Airport(4, 220, 481, "Mexico City International Airport", "Mexico", "Mexicocity", "Av. Capitán Carlos León"));
            M.AddAirport(new Airport(5, 534, 772, "Gubernatora Andrégo Franca Montoro'ego", "Brazil", "Sao_Paolo", "Rod. Hélio Smidt"));
            M.AddAirport(new Airport(6, 133, 378, "Los Angeles International Airport", "USA", "Los_Angeles", "1 World Way"));
            M.AddAirport(new Airport(7, 1567, 411, "Shanghai Pudong International Airport", "China", "Shanghai", "Pudong New Area, 900 Qihang Road"));
            M.AddAirport(new Airport(8, 1305, 525, "Mumbai Airport", "India", "Mombai", "T1, Navpada, Area, Vile Parle"));
            M.AddAirport(new Airport(9, 1591, 366, "Seul-Inczon Airport", "South Korea", "Seul", "272 Gonghang-ro, Jung-gu"));
            M.AddAirport(new Airport(10, 856, 574, "Lagos Airport", "Nigeria", "Lagos", "Int'l Airport Rd"));

            free_index = 11;
        }
        private void Window_Loaded_Timer(object sender, RoutedEventArgs e) //Timer methode
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_tick;
            timer.Start();
        }
        private void Timer_tick(object sender, EventArgs e) //Timer methode tick
        {
            time = time.AddMinutes(1); // Adding time
            TextBlock_Time.Text = $"{time.ToShortTimeString()} {time.ToShortDateString()}"; // Updateing time

            foreach (Aircraft aircraft in R.GetAircrafts()) // Compare Borders and Aircrafts to move them
            {
                foreach (Border border in ListOfPlaneBorders)
                {
                    if (border.Visibility == Visibility.Hidden) // Skipping for hidden elements
                    {
                        continue;
                    }
                    if (border.Name.Equals(aircraft.Get_Name()))
                    {
                        if (aircraft.Getroute_x == 0 && aircraft.Getroute_y == 0) // Hide aircrafts that have achieved the objective
                        {
                            border.Visibility = Visibility.Hidden;
                        }
                        aircraft.Flight_x(); // Calclute x 
                        aircraft.Flight_y(); // Calclute y 
                        border.Margin = new Thickness(aircraft.GetX, aircraft.GetY, 0, 0); // Setting new positions of Aircraft (Graphic) 
                    }
                }
            }
            if (R.CheckCollision()) // Checking Collisions and comunicate them
            {
                TextBlock_Info.Text = "A Collision Was Avoided.";
            }
        }
        // Checking if mouse is in border of the airport + changing look
        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) 
        {
            var border = sender as Border;
            border.Background = Brushes.Green;

            foreach (Airport airport in M.Get_Airports())
            {
                // Checking name equals
                if (border.Name.Equals(airport.Get_City()))
                {
                    TextBlock_Info.Text = $"{airport}"; // Displaying airport text
                }
            }
        }
        // Checking if mouse left the border of the airport + changing look
        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var border = sender as Border;
            border.Background = Brushes.Yellow;
            TextBlock_Info.Text = "No Information. Please Select Object.";
        }
        // Mouse Click on a city and puts in the container
        private void City_Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string border_name = button.Name;
            border_name = border_name.Remove(border_name.Length - 7);
            object border_find = Canvas_main.FindName(border_name);
            Border border = border_find as Border;
            if (a1 != null && a2 != null)
            {
                TextBlock_Info.Text = "You can choose only two airports at one time!\nRight Mouse Click removes check mark.";
            }
            if (a1 == null)
            {
                A1_case(button, border); // Putting Departure City
            }
            else if (a2 == null)
            {
                A2_case(button, border); // Putting Arrival City
            }
        }
        // Methode that puts Departure City to container
        private void A1_case(Button button, Border border)
        {
            foreach (Airport airport in M.Get_Airports())
            {
                if (button.Name.Equals(airport.Get_City() + "_Button") && airport != a2)
                {
                    a1 = airport;
                    TextBlock_Info.Text = "Departure Airport has been choosed!";
                    border.BorderBrush = Brushes.Orange;
                    ChoosenAirportOne.Text = a1.shortToString();
                }
            }
        }
        // Methode that puts Arrival City to container
        private void A2_case(Button button, Border border)
        {
            foreach (Airport airport in M.Get_Airports())
            {
                if (button.Name.Equals(airport.Get_City() + "_Button") && airport != a1)
                {
                    a2 = airport;
                    TextBlock_Info.Text = "Arrival Airport has been choosed!";
                    border.BorderBrush = Brushes.Orange;
                    ChoosenAirportTwo.Text = a2.shortToString();
                }
            }
        }
        // Mouse Right Click on a city and remove from continer
        private void City_Button_Right_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var button = sender as Button;
            string border_name = button.Name;
            border_name = border_name.Remove(border_name.Length - 7);
            object border_find = Canvas_main.FindName(border_name);
            Border border = border_find as Border;
            if ((a1 == null || a2 == null) || (a1 != null || a2 == null) || (a1 == null || a2 != null))
            {
                TextBlock_Info.Text = "No check mark You can not remove Airport!\nLeft Mouse Button checks mark.";
            }
            if (a1 != null && button.Name.Equals(a1.Get_City() + "_Button"))
            {
                a1 = null;
                TextBlock_Info.Text = "Removed Departure Airport!";
                border.BorderBrush = Brushes.Red;
                ChoosenAirportOne.Text = "";
            }
            if (a2 != null && button.Name.Equals(a2.Get_City() + "_Button"))
            { 
                a2 = null;
                TextBlock_Info.Text = "Removed Arrival Airport!";
                border.BorderBrush = Brushes.Red;
                ChoosenAirportTwo.Text = "";
            }
        }
        private void AddPlane_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Enter on Add Plane Button + changing look
        {
            AddPlaneImage.Source = new BitmapImage(new Uri("/img/plusgreen.png", UriKind.Relative));
            TextBlock_Info.Text = "Press Button To Create New Flight.";
        }
        private void AddPlane_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Leave from the Add Plane Button + changing look
        {
            AddPlaneImage.Source = new BitmapImage(new Uri("/img/plus.png", UriKind.Relative));
            TextBlock_Info.Text = "No Information. Please Select Object.";
        }
        private void AddPlane_Click(object sender, RoutedEventArgs e) // Mouse Click on Add Plane Button + changing look
        {
            if (a1 == null || a2 == null)
            {
                TextBlock_Info.Text = "Please select Airports before pressing New Flight Button\nDeparture Airport and Arrival Airport are necessary.";
            }
            else
            {
                AddFlight window = new AddFlight(); // Creating window of Add Plane
                window.Owner = this;
                if ((bool)window.ShowDialog())
                {
                    if (window.Aircraft_Type.Text == "") window.Aircraft_Type.Text = "Plane";
                    R.AddFlight(new Aircraft(free_index, a1.Get_Id(), a1.GetX + 5, a1.GetY + 5, a2.Get_Id(), a2.GetX + 5, a2.GetY + 5, time, window.Aircraft_Name.Text, window.Aircraft_Type.Text));
                    string name = window.Aircraft_Name.Text;
                    for (int i = 0; i < R.GetAircrafts().Count - 1; i++)
                    {
                        if (name.Equals(R.GetAircrafts()[i].Get_Name()))
                        {
                            name += free_index;
                            R.GetAircrafts()[R.GetAircrafts().Count - 1].ChangeName(name);
                        }
                    }
                    CreateBorderOfAircraft(a1.GetX + 5, a1.GetY + 5, name, window.Aircraft_Type.Text);
                    TextBlock_Info.Text = "Added New Flight, " + R.GetAircrafts()[free_index - M.Get_Airports().Count];
                    free_index++;
                }
                else
                {
                    TextBlock_Info.Text = "Adding a flight was canceled.";
                }
            }
        }
        private void CreateBorderOfAircraft(int x, int y, string name, string type) // Creating Border (Graphic) of Aircrafts
        {
            Border border_airplane = new Border();
            switch (type)
            {
                case "Plane": border_airplane.Background = Brushes.White; break;
                case "Cargo": border_airplane.Background = Brushes.Gold; break;
                case "Glider": border_airplane.Background = Brushes.Blue; break;
                case "Fighter": border_airplane.Background = Brushes.Black; break;
            }
            border_airplane.BorderBrush = Brushes.Red;
            border_airplane.BorderThickness = new Thickness(3, 3, 3, 3);
            border_airplane.CornerRadius = new CornerRadius(100);
            border_airplane.Height = 15;
            border_airplane.Width = 15;
            border_airplane.Margin = new Thickness(x, y, 0, 0);
            border_airplane.Name = name;
            border_airplane.MouseEnter += Border_Plane_MouseEnter;
            border_airplane.MouseLeave += Border_Plane_MouseLeave;
            border_airplane.MouseLeftButtonDown += Border_Plane_Click;
            border_airplane.MouseRightButtonDown += Border_Plane_Right_Click;
            TrafficControlSystem.Children.Add(border_airplane);
            ListOfPlaneBorders.Add(border_airplane);
        }
        private void RandomGenerate_Thick(object sender, EventArgs e) // Thick of Random Generate of Aircrafts 
        {
            Random random = new Random();
            int number_one, number_two, number_three;
            number_one = random.Next(0, M.Get_Airports().Count);
            do
            {
                number_two = random.Next(0, M.Get_Airports().Count);
            }
            while (number_two == number_one);
            string type = "", name = "Automatic_" + free_index;
            number_three = random.Next(0, 4);
            switch (number_three % 4)
            {
                case 0: type = "Plane"; break;
                case 1: type = "Cargo"; break;
                case 2: type = "Glider"; break;
                case 3: type = "Fighter"; break;
                default: type = "Plane"; break;
            }
            R.AddFlight(new Aircraft(free_index, M.Get_Airports()[number_one].Get_Id(), M.Get_Airports()[number_one].GetX + 5, M.Get_Airports()[number_one].GetY + 5, M.Get_Airports()[number_two].Get_Id(), M.Get_Airports()[number_two].GetX + 5, M.Get_Airports()[number_two].GetY + 5, time, name, type));
            CreateBorderOfAircraft(M.Get_Airports()[number_one].GetX + 5, M.Get_Airports()[number_one].GetY + 5, name, type);
            free_index++;
            HowMuch--; // Decresing Left Aircrafts
            if (HowMuch == 0)
            {
                timer.Stop();
            }
        }
        private void RandomGenerate() // Timer of Random Generate, Delay 5 seconds
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += new EventHandler(RandomGenerate_Thick);
            timer.Start();
        }
        private void Border_Plane_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) // Mouse enter at aircraft, changing look
        {
            Border border = sender as Border;
            foreach (Aircraft aircraft in R.GetAircrafts())
            {
                if (aircraft.Get_Name() == border.Name)
                {
                    border.Background = Brushes.Green;
                    TextBlock_Info.Text = $"{aircraft}";
                }
            }
        }
        private void Border_Plane_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Left from aircraft, changing look
        {
            Border border = sender as Border;
            foreach (Aircraft aircraft in R.GetAircrafts())
            {
                if (aircraft.Get_Name() == border.Name)
                {
                    switch (aircraft.Get_Type())
                    {
                        case "Plane": border.Background = Brushes.White; break;
                        case "Cargo": border.Background = Brushes.Gold; break;
                        case "Glider": border.Background = Brushes.Blue; break;
                        case "Fighter": border.Background = Brushes.Black; break;
                    }
                    TextBlock_Info.Text = "";
                }
            }
        }
        private void Border_Plane_Click(object sender, RoutedEventArgs e) // Mouse Click at aircraft, Putting aircraft to the container
        {
            var border = sender as Border;
            if (a == null)
            {
                foreach (Aircraft aircraft in R.GetAircrafts())
                {
                    if (border.Name.Equals(aircraft.Get_Name()))
                    {
                        border.BorderBrush = Brushes.Orange;
                        a = aircraft;
                        ChoosenAircraft.Text = a.shortToString();
                    }
                }
            }
            else
            {
                TextBlock_Info.Text = "Aircraft is already choosen.\nRight Mouse Click removes check mark.";
            }
        }
        // Mouse Right Click at aircraft, Removeing aircraft from the container
        private void Border_Plane_Right_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) 
        {
            if (a != null)
            {
                var border = sender as Border;
                foreach (Aircraft aircraft in R.GetAircrafts())
                {
                    if (border.Name.Equals(aircraft.Get_Name()))
                    {
                        border.BorderBrush = Brushes.Red;
                        a = null;
                        ChoosenAircraft.Text = "";
                    }
                }
            }
            else
            {
                TextBlock_Info.Text = "Can not remove Aircraft, No Aircraft has been choosen";
            }
        }
        private void Route_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Enter at Route Button, changing look
        {
            RouteFlightImage.Source = new BitmapImage(new Uri("/img/confirmrouteorange.png", UriKind.Relative));
            TextBlock_Info.Text = "Press Button To Change Flight Direction.";
        }
        private void Route_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Leave from Route Button, changing look
        {
            RouteFlightImage.Source = new BitmapImage(new Uri("/img/confirmroute.png", UriKind.Relative));
            TextBlock_Info.Text = "No Information. Please Select Object.";
        }
        private void Route_Click(object sender, RoutedEventArgs e) // Mouse Click at rute Button
        {
            if (a != null && a2 != null)
            {
                a.ChangeRouteFlight(a2.Get_Id(), a2.GetX + 5, a2.GetY + 5, time); // Changing Route to arrival airport
                TextBlock_Info.Text = "Change Flight direction to: " + a2;
            }
            else
            {
                TextBlock_Info.Text = "Please select Aircraft and Arrival Airport,\nto change route Flight.";
            }
        }
        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Enter at Buttons, changing look
        {
            Button button = sender as Button;
            button.Background = Brushes.LightGray;
        }
        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Leave from Buttons, changing look
        {
            Button button = sender as Button;
            button.Background = Brushes.Gray;
        }
        private void Random_Click(object sender, RoutedEventArgs e) // Mouse Click at Random Button
        {
            RandomGenerate window = new RandomGenerate();
            window.Owner = this;
            if ((bool)window.ShowDialog()) // Displaying Window of Generator
            {
                try
                {
                    int amount = Int32.Parse(window.GenerateAmount.Text);
                    HowMuch = amount;
                    RandomGenerate();
                }
                catch (FormatException fe)
                {
                    string messageBoxText = "Only numbers are allowed.\nPlease do not write text.";
                    string caption = "Incorrect Number";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result;
                    result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                    TextBlock_Info.Text = "Only numbers are allowed. Please do not write text.\nPlease try again.";
                }
            }
            else
            {
                TextBlock_Info.Text = "Generator has been canceled.";
            }
        }
        private void InformationButton_Click(object sender, RoutedEventArgs e) // Mouse Click at Information Button
        {
            Information window = new Information();
            window.Show(); // Displaying window of Information
        }
        private void TurnOff_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Enter at Turn off Button, changing look
        {
            TurnOffImage.Source = new BitmapImage(new Uri("/img/turnoffred.png", UriKind.Relative));
            TextBlock_Info.Text = "Turn Off Program.";
        }
        private void TurnOff_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) // Mouse Leave at Turn off Button, changing look
        {
            TurnOffImage.Source = new BitmapImage(new Uri("/img/turnoff.png", UriKind.Relative));
            TextBlock_Info.Text = "No Information. Please Select Object.";
        }
        private void TurnOff_Click(object sender, RoutedEventArgs e) // Mouse Click at Turn off Button
        {
            Turnoff window = new Turnoff();
            if ((bool)window.ShowDialog()) // Displaying window of Turn off
            {
                Close();
            }
        }
    }
}