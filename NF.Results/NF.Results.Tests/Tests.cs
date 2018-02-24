using System;
using Xunit;

namespace NF.Results.Tests
{
    public class A
    {
        
    }
    public class Tests
    {
        private Option<A> opt;
        
        [Fact]
        public void Test1()
        {
            this.opt = Option.Some<A>(null);
            
            Assert.True(true);
            Console.WriteLine("A");
            Assert.Equal(this.opt, Option.Some<A>(null));
            this.opt.Expect("hello");
            Option.None<A>().Expect("hello");

        }
    }
}