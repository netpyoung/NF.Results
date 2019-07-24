namespace NF.Results.Option
{
    using System;
    using System.Collections.Generic;
    using NF.Results.Exceptions;
    using ArgumentNullException = Exceptions.ArgumentNullException;

    public abstract class Option
    {
        public abstract bool IsNone { get; }

        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value, true);
        }
        public static readonly OptionNone None = new OptionNone();
    }

    public class Option<T> : Option, IEquatable<Option<T>>, IComparable<Option<T>>, ICloneable
    {
        private T _value;

        internal Option(T value, bool isSome)
        {
            this._value = value;
            this.IsSome = isSome;
        }

        public bool IsSome { get; private set; }

        public override bool IsNone => !this.IsSome;

        public T Value => this.Unwrap();

        public T Expect(string message)
        {
            if (this.IsSome)
            {
                return this._value;
            }

            throw new UnExpectedException(message);
        }

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (some == null)
            {
                throw new ArgumentNullException(nameof(some));
            }

            if (none == null)
            {
                throw new ArgumentNullException(nameof(none));
            }

            return this.IsSome ? some(this._value) : none();
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> f)
        {
            if (f == null)
            {
                return Option<TResult>.None;
            }

            return this.Match(
                value => Option.Some(f(value)),
                () => Option<TResult>.None
            );
        }

        public TResult MapOr<TResult>(TResult defaultValue, Func<T, TResult> f)
        {
            if (f == null)
            {
                return default(TResult);
            }

            return this.Match(
                f,
                () => defaultValue
            );
        }

        public TResult MapOrElse<TResult>(Func<TResult> defaultFunc, Func<T, TResult> f)
        {
            if (f == null)
            {
                return default(TResult);
            }

            return this.Match(
                f,
                defaultFunc
            );
        }

        public IEnumerable<Option<T>> ToEnumerable()
        {
            if (this.IsSome)
            {
                yield return Some(this._value);
            }
            else
            {
                yield return None;
            }
        }

        public IEnumerator<Option<T>> GetEnumerator()
        {
            if (this.IsSome)
            {
                yield return Some(this._value);
            }
            else
            {
                yield return None;
            }
        }

        public Option<T> And(Option<T> o)
        {
            if (this.IsSome && o.IsSome)
            {
                return o;
            }

            return None;
        }

        public Option<T> AndThen(Func<Option<T>> f)
        {
            if (!this.IsSome)
            {
                return None;
            }

            Option<T> optb = f();
            if (!optb.IsSome)
            {
                return None;
            }

            return optb;
        }

        public Option<T> Filter(Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                return None;
            }

            if (!this.IsSome)
            {
                return None;
            }

            if (!predicate(this._value))
            {
                return None;
            }

            return this;
        }

        public Option<T> Or(T alternative)
        {
            if (this.IsSome)
            {
                return this;
            }

            return Option.Some(alternative);
        }

        public Option<T> OrElse(Func<T> f)
        {
            if (f == null)
            {
                return None;
            }

            if (this.IsSome)
            {
                return this;
            }

            return Option.Some(f());
        }

        public T GetOrInsert(T v)
        {
            if (this.IsSome)
            {
                return this._value;
            }

            this._value = v;
            return v;
        }

        public T GetOrInsert(Func<T> f)
        {
            if (this.IsSome)
            {
                return this._value;
            }

            T v = f == null ? default(T) : f();
            this._value = v;
            return v;
        }

        public Option<T> Take()
        {
            if (!this.IsSome)
            {
                return None;
            }

            T v = this._value;
            this._value = default(T);
            this.IsSome = false;
            return Option.Some(v);
        }

        public object Clone()
        {
            return this.Cloned();
        }

        public Option<T> Cloned()
        {
            return this;
        }

        private IEnumerable<T> InToEnumerable()
        {
            if (!this.IsSome)
            {
                yield break;
            }

            yield return this._value;
        }

        private IEnumerator<T> IntoEnumerator()
        {
            if (!this.IsSome)
            {
                yield break;
            }

            yield return this._value;
        }

        public bool Equals(Option<T> o)
        {
            if (!this.IsSome && !o.IsSome)
            {
                return true;
            }

            if (this.IsSome && o.IsSome)
            {
                return EqualityComparer<T>.Default.Equals(this._value, o._value);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Option<T> && this.Equals((Option<T>) obj);
        }

        public override int GetHashCode()
        {
            if (!this.IsSome)
            {
                return 0;
            }

            if (this._value == null)
            {
                return 1;
            }

            return this._value.GetHashCode();
        }

        public int CompareTo(Option<T> o)
        {
            if (this.IsSome && !o.IsSome)
            {
                return 1;
            }

            if (!this.IsSome && o.IsSome)
            {
                return -1;
            }

            return Comparer<T>.Default.Compare(this._value, o._value);
        }

        #region operator Option<T>
        public static bool operator ==(Option<T> left, Option<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Option<T> left, Option<T> right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Option<T> left, Option<T> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Option<T> left, Option<T> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Option<T> left, Option<T> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Option<T> left, Option<T> right)
        {
            return left.CompareTo(right) >= 0;
        }
        #endregion operator Option<T>

        public override string ToString()
        {
            if (!this.IsSome)
            {
                return "None";
            }

            if (this._value == null)
            {
                return "Some(null)";
            }

            return $"Some({this._value})";
        }

        public T Unwrap()
        {
            if (this.IsSome)
            {
                return this._value;
            }

            throw new UnwrapException(this.ToString());
        }

        public T UnwrapOrDefault()
        {
            if (this.IsSome)
            {
                return this._value;
            }

            return default(T);
        }

        public T UnwrapOrElse(T val)
        {
            if (this.IsSome)
            {
                return this._value;
            }

            return val;
        }

        public T UnwrapOrElse(Func<T> f)
        {
            if (this.IsSome)
            {
                return this._value;
            }

            return f != null ? f() : default(T);
        }

        public static Option<T> Some(T value)
        {
            return new Option<T>(value, true);
        }

        public new static readonly Option<T> None = new Option<T>(default(T), false);

        public static implicit operator Option<T>(OptionNone none)
        {
            return None;
        }
    }
}
