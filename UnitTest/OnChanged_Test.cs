using Kasay.PropertyChanged;
using SmokeTest;
using Xunit;

namespace UnitTest
{
    public class OnChanged_Test
    {
        [Fact]
        public void ValidateOnChanged()
        {
            var person = new Person();

            void callBack()
            {
                person.Age = 18;
            }

            person.OnChanged(_ => _.Status, callBack);

            Assert.NotEqual(18, person.Age);

            person.Status = "I'm majority age";

            Assert.Equal(18, person.Age);
        }

        [Fact]
        public void ValidateOnChangedInherent()
        {
            var person = new Student();

            void callBack()
            {
                person.ClassRoom = 104;
            }

            person.OnChanged(_ => _.Course, callBack);

            Assert.NotEqual(104, person.ClassRoom);

            person.Course = "Math";

            Assert.Equal(104, person.ClassRoom);
        }
    }
}
