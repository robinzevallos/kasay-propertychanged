using Kasay.PropertyChanged;
using System;

namespace SmokeTest
{
    public class Person : Notifiable
    {
        [Notify] public String Status { get; set; }

        public Int32 Age { get; set; }
    }
}
