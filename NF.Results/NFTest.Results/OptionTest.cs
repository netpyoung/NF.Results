#pragma warning disable xUnit2000

namespace NFTest.Results
{
    using System;
    using Xunit;
    using NF.Results.Option;
    using NF.Results.Result;
    using NF.Results.Exceptions;

    public class OptionTest
    {
        public class A
        {
        }

        public enum E_ERROR
        {
            A,
            B
        }

        [Fact]
        public void Test1()
        {
            Option<A> opt = Option.Some<A>(null);
            Assert.Equal(opt, Option.Some<A>(null));

            Assert.Null(opt.Expect("hello"));
            Exception ex = Assert.Throws<UnExpectedException>(() =>
                Option<A>.None.Expect("hello")
            );
            Assert.Equal("hello", ex.Message);
        }

        [Fact]
        public void TestToOption()
        {
            Option<int> opt1 = 1.ToOption();
            Assert.Equal(opt1.Value, 1);

            Option<int> opt2 = 1;
            Assert.Equal(opt2.Value, 1);

            Option<int> opt3 = Option.None;
            Assert.Equal(opt3.IsNone, true);
        }

        [Fact]
        public void TestOptionOkErr()
        {
            Assert.Equal(10.ToOk(), Option.Some(10));
            Assert.Equal(10.ToErr(), Option.Some(10));
        }

        [Fact]
        public void TestOption()
        {
            Option<int> some = Option.Some(10);
            Assert.True(some.IsSome);
            Assert.Equal(some.Unwrap(), 10);
            Assert.Equal(some.Value, 10);
            Option<int> none = Option<int>.None;
            Assert.True(none.IsNone);
            Assert.Throws<UnwrapException>(() =>
                none.Value
            );
        }
    }
}