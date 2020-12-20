using Kasay.PropertyChanged;
using System;

namespace AssemblyToProcess
{
    public class Foo : Notifiable
    {
        [Notify] public String Property { get; set; }
    }

    public class Doo : Foo
    {
        [Notify] public String Property2 { get; set; }
    }
}
