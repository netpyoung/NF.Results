namespace NF.Results.Option
{
    public struct OptionNone : IOption
    {
        public bool IsNone => true;

        public override bool Equals(object obj)
        {
            if (obj is OptionNone)
            {
                return true;
            }

            if (obj is IOption o)
            {
                return o.IsNone;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
