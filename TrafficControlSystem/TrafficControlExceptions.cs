using System;

namespace TrafficControlSystem
{
    //Exception class for testing
    class IncorrectPosition : Exception
    {
        public IncorrectPosition() : base("Wrong Value for Position.") { }
    }
}
