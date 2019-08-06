namespace NF.Results.Option
{
    public struct OptionOk<TOk> : IOption
    {
        public bool IsNone => false;

        readonly internal TOk value;

        internal OptionOk(TOk value)
        {
            this.value = value;
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
