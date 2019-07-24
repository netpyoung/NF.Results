#pragma warning disable xUnit2000

namespace NFTest.Results
{
    using Xunit;
    using NF.Results.Option;
    using NF.Results.Result;

    public class ResultTest
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
                return a.ToOk();
            }
        }

        [Fact]
        public void Test2()
        {
            Result<int, E_ERROR> result = Result.Ok<int, E_ERROR>(10);
            Assert.Equal(result.Ok(), Option.Some(10));
            Assert.Equal(result.Ok, 10);

            Assert.Equal(result.Err(), Option<E_ERROR>.None);
        }

        [Fact]
        public void TestToOk()
        {
            Result<int, string> result = 1.ToOk();
            OptionOk<int> ok = result.Ok();
            Assert.Equal(ok, Result.Ok(1));
            Assert.Equal(ok, Option.Some(1));
        }

        [Fact]
        public void TestToErr()
        {
            Result<string, int> result = 1.ToErr();
            OptionErr<int> err = result.Err();
            Assert.Equal(err, Result.Err(1));
            Assert.Equal(err, Option.Some(1));
        }

        [Fact]
        public void TestTranspose()
        {
            Hello hello = new Hello();
            Result<int, E_ERROR> result = hello.GetResult(10);
            Option<Result<int, E_ERROR>> opt = Option.Some(result);
            Result<Option<int>, E_ERROR> trans = opt.Transpose();

            Assert.Equal(opt, Option.Some(Result.Ok<int, E_ERROR>(10)));
            Assert.Equal(trans, Result.Ok(Option.Some(10)));
            Assert.Equal(trans.Transpose(), Option<Result<int, E_ERROR>>.Some(Result.Ok(10)));
        }

        [Fact]
        public void TestResult()
        {
            Result<int, string> ok = 10.ToOk();
            Assert.True(ok.IsOk);
            Assert.Equal(ok.Unwrap(), 10);
            Assert.Equal(ok.Ok, 10);

            Result<int, string> err = "wtf".ToErr();
            Assert.True(err.IsErr);
            Assert.Equal(err.UnwrapErr(), "wtf");
            Assert.Equal(err.Err, "wtf");
        }
    }
}