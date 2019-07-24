namespace NF.Results.Option
{
    public sealed class OptionErr<TErr> : Option<TErr>
    {
        internal OptionErr(TErr value) : base(value, true)
        {
        }

        internal OptionErr() : base(default(TErr), false)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is OptionErr<TErr>)
            {
                return base.Equals(obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator OptionErr<TErr>(OptionNone none)
        {
            return new OptionErr<TErr>();
        }
    }
}
