using Kasay.PropertyChanged;
using System;

namespace AssemblyToProcess
{
    public class Foo : Notifiable
    {
        [Notify] public String Property { get; set; }
    }
}
