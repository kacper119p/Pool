using Logic;

namespace LogicTests
{
    public class ExampleTests
    {
        [Test]
        public void Test()
        {
            Assert.IsTrue(ExampleClass.ReturnTrue());
            Assert.IsFalse(ExampleClass.ReturnFalse());
        }
    }
}