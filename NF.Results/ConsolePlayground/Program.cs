using System;
using NF.Results;

namespace ConsolePlayground
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var hello = new Hello();
            var result = hello.GetResult(10);
            var opt = Option.Some(result);
            var trans = opt.Transpose();
            
            Console.WriteLine(opt);
            Console.WriteLine(trans);
            Console.WriteLine(trans.Transpose());
            Hello a = new Hello();
            var option = a.ToOption();
            Console.WriteLine(option);

            var ok = a.ToOk<Hello, int>();
            Console.WriteLine(ok.Ok());
        }
        
        public static void Main2(string[] args)
        {
            var hello = Option.Some(10);
            if (hello.IsSome)
            {
                var val = hello.Unwrap();
                Console.WriteLine(val);
            }

            Console.WriteLine(hello);
        }

        public static void Main1(string[] args)
        {
            var hello = new Hello();
            var result = hello.GetResult(10);
            if (result.IsOk)
            {
                var val = result.Unwrap();
                Console.WriteLine(val);
            }

            Console.WriteLine(result);
        }
    }

    public enum E_ERROR
    {
        A,
        B,
    }
    
    public class Hello
    {
        public Result<int, E_ERROR> GetResult(int a)
        {
            int next = a + 10;
            return Result.Ok<int, E_ERROR>(next);
        }
    }
}