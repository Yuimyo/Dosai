using Xunit.Abstractions;

namespace Dosai.Test
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;
        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void AreBoolEvalsExecute()
        {
            Func<bool, bool> c1 = (value) =>
            {
                output.WriteLine("c1 was executed!");
                return value;
            };
            Func<bool, bool> c2 = (value) =>
            {
                output.WriteLine("c2 was executed!");
                return value;
            };
            Assert.True(c1(true) || c2(true));
            Assert.True(c1(true) || c2(false));
            Assert.True(c1(false) || c2(true));
            Assert.False(c1(false) || c2(false));
        }

        [Fact]
        public void Nullable_VS_NonNullable()
        {
            Assert.True(default(int?) == null);
            #pragma warning disable CS0472 // この型の値が 'null' に等しくなることはないので、式の結果は常に同じです
            Assert.False(default(int) == null);
            #pragma warning restore CS0472 // この型の値が 'null' に等しくなることはないので、式の結果は常に同じです
        }

        [Fact]
        public void testTypeof()
        {
            
        }
    }
}