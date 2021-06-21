using System;
using System.Collections.Generic;
using TrafficControlSystem;

namespace Objects
{
    // Abstract class Object
    abstract class Object
    {
        protected int id, x, y, z; // ID and cooridates 
        protected string name;
        public Object(int id, int x, int y, string name) // Constructor for the class
        {
            this.id = id;
            this.x = x;
            this.y = y;
            z = 0;
            this.name = name;
        }
        public int Get_Id() // Returning int ID of na object
        {
            return id;
        }
        public int GetX // Setting or getting up x cordinate of an object
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int GetY // Setting or getting up y cordinate of an object
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public int GetZ // Setting or getting up z cordinate of an object
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }
        public string Get_Name() // Returning string name of an object
        {
            return name;
        }
    }
    // Class Airport 
    class Airport : Object
    {
        private string country, city, post_code;
        // Constructor
        public Airport(int id, int x, int y, string name, string country, string city, string post_code) : base(id, x, y, name)
        {
            this.country = country;
            this.city = city;
            this.post_code = post_code;
        }
        // Returning string City name of the airport
        public string Get_City()
        {
            return city;
        }
        // Returning shorter version of the ToString methode [id, country, city]
        public string shortToString()
        {
            return $"Airport ID: {id} \nCountry: {country}\nCity: {city}";
        }
        // Returning longer version of the ToString methode [id, name, country, city, post code]
        public override string ToString()
        {
            return $"Airport ID: {id}\nName: {name}\nCountry: {country}\nCity: {city}\nPost code: {post_code}";
        }
    }
    // LandMap class
    class LandMap
    {
        private List<Airport> AirportList; // List of Airport objects
        public LandMap() // Constructor of an object
        {
            AirportList = new List<Airport>();
        }
        // Methode responsible for adding airport to the list of object [AirportList]
        public void AddAirport(Airport a)
        {
            AirportList.Add(a);
        }
        // Returning list of objects [AirportList]
        public List<Airport> Get_Airports()
        {
            return AirportList;
        }
    }
    // Aircraft class
    class Aircraft : Object
    {
        private string type; // Type of the aircraft
        // Coordinates to move [route_x,route_y] in x,y positions, Departure and arrival id of Airports [departure_airport,arrival_airport], speed of Aircraft
        private int route_x, route_y, departure_airport, arrival_airport, speed; 
        // Date of deprture and arrival
        private DateTime departure_date, arrival_date;
        // Constructor
        public Aircraft(int id, int departure_airport, int x, int y, int arrival_airport, int x_arrival, int y_arrival, DateTime time, string name, string type) : base(id, x, y, name)
        {
            // Setting up default altitute
            z = 8000;
            this.type = type; // Setting type
            route_x = x_arrival - x; // Calculate route of x
            route_y = y_arrival - y; // Calculate route of y
            //Setting up speed depending on the Aircraft type
            switch (type)
            {
                case "Plane": speed = 3; break;
                case "Cargo": speed = 2; break;
                case "Glider": speed = 1; break;
                case "Fighter": speed = 5; break;
            }
            // Setting departure time
            departure_date = time;
            // Calculating arrival date by method 
            CalculateDelay(route_x, route_y, time);
            this.departure_airport = departure_airport; // Setting departure Airport id
            this.arrival_airport = arrival_airport; // Setting arrival Airport id
        }
        // Methode responsible for changeing name of an Aircraft
        public void ChangeName(string name)
        {
            this.name = name;
        }
        public int Getroute_x // Methode responsible for setting up left x route of an Aircraft
        {
            get
            {
                return route_x;
            }
            set
            {
                route_x = value;
            }
        }
        public int Getroute_y // Methode responsible for setting up left y route of an Aircraft
        {
            get
            {
                return route_y;
            }
            set
            {
                route_y = value;
            }
        }
        public string Get_Type() // Returning type of an Aircraft
        {
            return type;
        }
        public void Flight_x() // Calculating new x position of the route cordinates
        {
            if (route_x < 0) // If of direction to move Left or Right
            {
                if(Math.Abs(route_x) < speed) // If of how much Aircrafte move
                {
                    x -= Math.Abs(route_x);
                    route_x += Math.Abs(route_x);
                }
                else
                {
                    route_x += speed;
                    x -= speed;
                }
            }
            else if (route_x > 0) // If of direction to move Left or Right
            {
                if(route_x < speed) // If of how much Aircrafte move
                {
                    x += Math.Abs(route_x);
                    route_x -= Math.Abs(route_x);
                }
                else
                {
                    route_x -= speed;
                    x += speed;
                }
            }
            // No route_x == 0 , route_x == 0 <==> Aricraft is at correct x positions
        }
        public void Flight_y() // Calculating new y position of the route cordinates
        {
            if (route_y < 0) // If of direction to move Upper or Down
            {
                if(Math.Abs(route_y) < speed) // If of how much Aircrafte move
                {
                    y -= Math.Abs(route_y);
                    route_y += Math.Abs(route_y);
                }
                else
                {
                    route_y += speed;
                    y -= speed;
                }
            }
            else if (route_y > 0) // If of direction to move Upper or Down
            {
                if(route_y < speed) // If of how much Aircrafte move
                {
                    y += Math.Abs(route_y);
                    route_y -= Math.Abs(route_y);
                }
                else
                {
                    route_y -= speed;
                    y += speed;
                }
            }
        }
        // Changing rute of thr flight and recalculating time ot the arrival Airport 
        public void ChangeRouteFlight(int Airport_arrival, int x_arrival, int y_arrival, DateTime time)
        {
            route_x = x_arrival - x;
            route_y = y_arrival - y;
            departure_date = time;
            CalculateDelay(route_x,route_y,time);
        }
        // Calculating time of travel between to places
        public void CalculateDelay(int delay_x,int delay_y,DateTime time)
        {
            // delay cannot be < 0, because it could back in time
            if (delay_x < 0) delay_x *= (-1); 
            if (delay_y < 0) delay_y *= (-1); 
            // Choosing possible square to move and left of distatance to move to calculate time , adding 1 to correct division by speed
            if (delay_x < delay_y) arrival_date = time.AddMinutes(((delay_x + (delay_y - delay_x)) / speed) + 1); // 
            else arrival_date = time.AddMinutes(((delay_y + (delay_x - delay_y)) / speed) + 1);
        }
        // Returning shorter version of the ToString methode [name, type, z]
        public string shortToString()
        {
            return $"Aircraft: {name}\nType of Aircraft: {type}\nHeight of Flight: {z} meters";
        }
        //returning full version of the ToString methode [id,name, type,departure airport, arrival airport,departure date, arrival date]
        public override string ToString()
        {
            return $"Flight ID: {id}, Aircraft: {name}\nType of Aircraft: {type}\nFlight from Airport ID: {departure_airport}, to Airport ID: {arrival_airport}\nDeparture Date: {departure_date.ToShortTimeString()} {departure_date.ToShortDateString()}\nArrival Date: {arrival_date.ToShortTimeString()} {arrival_date.ToShortDateString()}";
        }
    }
    // Radar class
    class Radar
    {
        private List<Aircraft> AircraftList; // List of Aircrafts
        // Constructor 
        public Radar()
        {
            AircraftList = new List<Aircraft>();
        }
        // Adding aircraftr to the list of Aircrafts [AircraftList]
        public void AddFlight(Aircraft f)
        {
            AircraftList.Add(f);
        }
        // Methode responsible for checking colision between all Aircrafts
        public bool CheckCollision()
        {
            foreach(Aircraft f1 in AircraftList)
            {
                foreach (Aircraft f2 in AircraftList)
                {
                    if ((f1.GetZ == f2.GetZ) && (f1.Getroute_y != 0 && f2.Getroute_y != 0) && (f1.Getroute_x!=0 && f2.Getroute_x!=0) && (f1.Get_Name() != f2.Get_Name()) && (f1.GetX + 5 >= f2.GetX && f1.GetX - 5 <= f2.GetX) && (f1.GetY + 5 >= f2.GetY && f1.GetY - 5 <= f2.GetY))
                    {
                        // Changing altitute of aircrafts that were in possible collision course
                        if (f1.GetZ >= 8000 && f1.GetZ < 11000)
                        {
                            f1.GetZ += 1000;
                            f2.GetZ -= 1000;
                        }
                        else
                        {
                            f1.GetZ -= 1000;
                            f2.GetZ += 1000;
                        }
                        // Returning value that collicin might happend
                        return true;
                    }
                }
            }
            // Collision had not been detected
            return false;
        }
        public List<Aircraft> GetAircrafts() //Returning list of Aircrafts [AircraftList]
        {
            return AircraftList;
        }
    }
}