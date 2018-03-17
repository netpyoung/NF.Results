namespace NF.Results
{
    public static class ExOption
    {
        public static Result<Option<TOk>, TErr> Transpose<TOk, TErr>(this Option<Result<TOk, TErr>> self)
        {
            if (!self.IsSome)
            {
                return Result.Ok<Option<TOk>, TErr>(Option.None<TOk>());
            }

            Result<TOk, TErr> result = self.Unwrap();
            if (result.IsOk)
            {
                return Result.Ok<Option<TOk>, TErr>(Option.Some(result.Unwrap()));
            }

            return Result.Err<Option<TOk>, TErr>(result.UnwrapErr());
        }

        public static Option<T> ToOption<T>(this T val)
        {
            return Option.Some(val);
        }
    }
}