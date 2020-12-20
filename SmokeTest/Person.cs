using Kasay.PropertyChanged;
using System;

namespace SmokeTest
{
    public class Person : Notifiable
    {
        [Notify] public String Status { get; set; }

        public Int32 Age { get; set; }
    }

    public class Student : Person
    {
        [Notify] public String Course { get; set; }

        public Int32 ClassRoom { get; set; }
    }
}
