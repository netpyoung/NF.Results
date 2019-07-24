namespace NF.Results.Option
{
    public static class ExOption
    {
        public static Option<T> ToOption<T>(this T val)
        {
            return Option.Some(val);
        }
    }
}