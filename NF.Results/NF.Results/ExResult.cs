using System;

namespace NF.Results
{
    public static class ExResult
    {
        public static Option<Result<TOk, TErr>> Transpose<TOk, TErr>(this Result<Option<TOk>, TErr> self)
        {
            if (!self.IsOk)
            {
                return Option.Some(Result.Err<TOk, TErr>(self.UnwrapErr()));
            }

            Option<TOk> opt = self.Unwrap();
            if (opt.IsSome)
            {
                return Option.Some(Result.Ok<TOk, TErr>(opt.Unwrap()));
            }

            return Option.None<Result<TOk, TErr>>();
        }

        public static Result<TOk, TErr> ToOk<TOk, TErr>(this TOk val)
        {
            return Result.Ok<TOk, TErr>(val);
        }

        public static Result<TOk, TErr> ToErr<TOk, TErr>(this TErr err)
        {
            return Result.Err<TOk, TErr>(err);
        }

        public static Option<TOk> Ok<TOk, TErr>(this Result<TOk, TErr> result)
        {
            return result.IsOk ? Option.Some(result._ok) : Option.None<TOk>();
        }

        public static Option<TErr> Err<TOk, TErr>(this Result<TOk, TErr> result)
        {
            return result.IsOk ? Option.None<TErr>() : Option.Some(result._err);
        }
        
        
        public static Result<TOk, string> ToOk<TOk>(this TOk val)
        {
            return Result.Ok<TOk, string>(val);
        }

        public static Result<TOk, string> ToErr<TOk>(this string err)
        {
            return Result.Err<TOk, string>(err);
        }
    }
}