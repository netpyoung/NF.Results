namespace NF.Results.Option
{
    public struct OptionErr<TErr> : IOption
    {
        public bool IsNone => !isSome;

        readonly internal TErr value;

        readonly bool isSome; // for prevent default false of IsNone;

        internal OptionErr(TErr value)
        {
            this.value = value;
            this.isSome = true;
        }

        internal OptionErr(TErr value, bool isSome)
        {
            this.value = value;
            this.isSome = isSome;
        }

        public override bool Equals(object obj)
        {
            if (obj is OptionErr<TErr>)
            {
                return base.Equals(obj);
            }
            if (obj is OptionNone && this.IsNone)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator OptionErr<TErr>(OptionNone none)
        {
            return new OptionErr<TErr>(default(TErr), false);
        }
    }
}
