using System;
using Xunit;

namespace NF.Results.Tests
{
    public class A
    {
    }

    public enum E_ERROR
    {
        A,
        B
    }

    public class Hello
    {
        public Result<int, E_ERROR> GetResult(int a)
        {
            return Result.Ok<int, E_ERROR>(a);
        }
    }

    public class Tests
    {
        [Fact]
        public void Test1()
        {
            Option<A> opt = Option.Some<A>(null);
            Assert.Equal(opt, Option.Some<A>(null));

            Assert.Equal(opt.Expect("hello"), null);
            Exception ex = Assert.Throws<UnExpectedException>(() => Option.None<A>().Expect("hello"));
            Assert.Equal("hello", ex.Message);
        }

        [Fact]
        public void Test2()
        {
            Result<int, E_ERROR> result = Result.Ok<int, E_ERROR>(10);
            Assert.Equal(result.Ok(), Option.Some(10));
            Assert.Equal(result.Ok, 10);


            Assert.Equal(result.Err(), Option.None<E_ERROR>());
        }

        [Fact]
        public void TestToOk()
        {
            Result<int, string> ok = 1.ToOk();
            Assert.Equal(ok.Ok(), Option.Some(1));
        }

        [Fact]
        public void TestToOption()
        {
            Option<int> option = 1.ToOption();
            Assert.Equal(option.Value, 1);
        }

        [Fact]
        public void TestTranspose()
        {
            Hello hello = new Hello();
            Result<int, E_ERROR> result = hello.GetResult(10);
            Option<Result<int, E_ERROR>> opt = Option.Some(result);
            Result<Option<int>, E_ERROR> trans = opt.Transpose();

            Assert.Equal(opt, Option.Some(Result.Ok<int, E_ERROR>(10)));
            Assert.Equal(trans, Result.Ok<Option<int>, E_ERROR>(Option.Some(10)));
            Assert.Equal(trans.Transpose(), Option.Some(Result.Ok<int, E_ERROR>(10)));
        }

        [Fact]
        public void TestOption()
        {
            Option<int> some = Option.Some(10);
            Assert.True(some.IsSome);
            Assert.Equal(some.Unwrap(), 10);
            Assert.Equal(some.Value, 10);
            Option<int> none = Option.None<int>();
            Assert.True(none.IsNone);
            Assert.Throws<UnwrapException>(() => none.Value);
        }


        [Fact]
        public void TestResult()
        {
            Result<int, string> ok = 10.ToOk();
            Assert.True(ok.IsOk);
            Assert.Equal(ok.Unwrap(), 10);
            Assert.Equal(ok.Ok, 10);

            var err = "wtf".ToErr<int>();
            Assert.True(err.IsErr);
            Assert.Equal(err.UnwrapErr(), "wtf");
            Assert.Equal(err.Err, "wtf");
        }
    }
}