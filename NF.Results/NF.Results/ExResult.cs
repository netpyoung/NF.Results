namespace NF.Results.Result
{
    using NF.Results.Option;
    using System;

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

            return Option<Result<TOk, TErr>>.None;
        }

        public static Result<TOk, TErr> ToOk<TOk, TErr>(this TOk val)
        {
            return Result.Ok<TOk, TErr>(val);
        }

        public static Result<TOk, TErr> ToErr<TOk, TErr>(this TErr err)
        {
            return Result.Err<TOk, TErr>(err);
        }

        public static OptionOk<TOk> Ok<TOk, TErr>(this Result<TOk, TErr> result)
        {
            return result.IsOk ? Result.Ok(result._ok) : Option.None;
        }

        public static OptionErr<TErr> Err<TOk, TErr>(this Result<TOk, TErr> result)
        {
            return result.IsOk ? Option.None : Result.Err(result._err);
        }
        
        public static OptionOk<TOk> ToOk<TOk>(this TOk ok)
        {
            return new OptionOk<TOk>(ok);
        }

        public static OptionErr<TErr> ToErr<TErr>(this TErr error)
        {
            return new OptionErr<TErr>(error);
        }

        public static Result<Option<TOk>, TErr> Transpose<TOk, TErr>(this Option<Result<TOk, TErr>> self)
        {
            if (!self.IsSome)
            {
                return Result.Ok<Option<TOk>, TErr>(Option<TOk>.None);
            }

            Result<TOk, TErr> result = self.Unwrap();
            if (result.IsOk)
            {
                return Result.Ok<Option<TOk>, TErr>(Option.Some(result.Unwrap()));
            }

            return Result.Err<Option<TOk>, TErr>(result.UnwrapErr());
        }

        public static Result<T, TErr> OkOr<T, TErr>(this Option<T> option, TErr err)
        {
            if (option.IsSome)
            {
                return Result.Ok<T, TErr>(option.Unwrap());
            }

            return Result.Err<T, TErr>(err);
        }

        public static Result<T, TErr> OkOrElse<T, TErr>(this Option<T> option, Func<TErr> errFunc)
        {
            if (option.IsSome)
            {
                return Result.Ok<T, TErr>(option.Unwrap());
            }

            return Result.Err<T, TErr>(errFunc());
        }

    }
}