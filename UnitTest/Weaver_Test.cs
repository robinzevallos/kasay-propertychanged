using Fody;
using System;
using System.ComponentModel;
using Xunit;

namespace UnitTest
{
    public class Weaver_Test
    {
        static readonly TestResult testResult;

        static Weaver_Test()
        {
            var weavingTask = new ModuleWeaver();
            testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");
        }

        [Fact]
        public void ValidateOnPropertyChangedInjected()
        {
            var type = testResult.Assembly.GetType("AssemblyToProcess.Foo");
            var foo = Activator.CreateInstance(type) as dynamic;

            dynamic expectedProperty = null;

            (foo as INotifyPropertyChanged).PropertyChanged += (s, e) =>
            {
                expectedProperty = foo.Property;
            };

            foo.Property = "Robin";

            var actualProperty = foo.Property;

            Assert.Equal(expectedProperty, actualProperty);
        }
    }
}
