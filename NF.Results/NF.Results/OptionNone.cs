namespace NF.Results.Option
{
    public sealed class OptionNone : Option
    {
        public override bool IsNone => true;

        internal OptionNone()
        {
        }

        public override bool Equals(object obj)
        {
            return (obj is OptionNone);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
