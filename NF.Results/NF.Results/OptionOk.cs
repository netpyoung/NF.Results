namespace NF.Results.Option
{
    public sealed class OptionOk<TOk> : Option<TOk>
    {
        internal OptionOk(TOk value) : base(value, true)
        {
        }

        internal OptionOk() : base(default(TOk), false)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is OptionOk<TOk>)
            {
                return base.Equals(obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator OptionOk<TOk>(OptionNone none)
        {
            return new OptionOk<TOk>();
        }
    }
}
