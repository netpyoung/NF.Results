using System;
using System.Collections.Generic;

namespace NF.Results
{
    public static class Result
    {
        public static Result<TOk, TErr> Ok<TOk, TErr>(TOk ok)
        {
            return new Result<TOk, TErr>(ok, default(TErr), true);
        }

        public static Result<TOk, TErr> Err<TOk, TErr>(TErr err)
        {
            return new Result<TOk, TErr>(default(TOk), err, false);
        }

        public static Result<TOk, TOk> Ok<TOk>(TOk ok)
        {
            return new Result<TOk, TOk>(ok, ok, true);
        }

        public static Result<TErr, TErr> Err<TErr>(TErr err)
        {
            return new Result<TErr, TErr>(err, err, false);
        }
    }

    public struct Result<TOk, TErr> : IEquatable<Result<TOk, TErr>>, IComparable<Result<TOk, TErr>>, ICloneable
    {
        public bool IsOk { get; }
        public bool IsErr => !this.IsOk;

        public TOk Ok => this.Unwrap();
        public TErr Err => this.UnwrapErr();

        internal TOk _ok;

        internal TErr _err;

        internal Result(TOk ok, TErr err, bool isOk)
        {
            this._ok = ok;
            this.IsOk = isOk;
            this._err = err;
        }

        public bool Equals(Result<TOk, TErr> other)
        {
            if (!this.IsOk && !other.IsOk)
            {
                return EqualityComparer<TErr>.Default.Equals(this._err, other._err);
            }

            if (this.IsOk && other.IsOk)
            {
                return EqualityComparer<TOk>.Default.Equals(this._ok, other._ok);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is Result<TOk, TErr> ? this.Equals((Result<TOk, TErr>) obj) : false;
        }

        public static bool operator ==(Result<TOk, TErr> left, Result<TOk, TErr> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Result<TOk, TErr> left, Result<TOk, TErr> right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            if (this.IsOk)
            {
                if (this._ok == null)
                {
                    return 1;
                }

                return this._ok.GetHashCode();
            }

            if (this._err == null)
            {
                return 0;
            }

            return this._err.GetHashCode();
        }

        public int CompareTo(Result<TOk, TErr> other)
        {
            if (this.IsOk && !other.IsOk)
            {
                return 1;
            }

            if (!this.IsOk && other.IsOk)
            {
                return -1;
            }

            return this.IsOk
                ? Comparer<TOk>.Default.Compare(this._ok, other._ok)
                : Comparer<TErr>.Default.Compare(this._err, other._err);
        }

        public static bool operator <(Result<TOk, TErr> left, Result<TOk, TErr> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Result<TOk, TErr> left, Result<TOk, TErr> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Result<TOk, TErr> left, Result<TOk, TErr> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Result<TOk, TErr> left, Result<TOk, TErr> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public override string ToString()
        {
            if (this.IsOk)
            {
                if (this._ok == null)
                {
                    return "Ok(null)";
                }

                return $"Ok({this._ok})";
            }

            if (this._err == null)
            {
                return "Err(null)";
            }

            return $"Err({this._err})";
        }

        public object Clone()
        {
            return this.Cloned();
        }

        public Result<TOk, TErr> Cloned()
        {
            return this;
        }

        public bool Contains(TOk value)
        {
            if (!this.IsOk)
            {
                return false;
            }

            if (this._ok == null)
            {
                return value == null;
            }

            return this._ok.Equals(value);
        }

        public Result<TResult, TErr> Match<TResult>(Func<TOk, Result<TResult, TErr>> ok,
            Func<TErr, Result<TResult, TErr>> err)
        {
            if (ok == null)
            {
                throw new ArgumentNullException(nameof(ok));
            }

            if (err == null)
            {
                throw new ArgumentNullException(nameof(err));
            }

            return this.IsOk ? ok(this._ok) : err(this._err);
        }

        public Result<TResult, TErr> Map<TResult>(Func<TOk, TResult> f)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            return this.Match(
                ok => Result.Ok<TResult, TErr>(f(ok)),
                err => Result.Err<TResult, TErr>(err)
            );
        }

        public Result<TOk, TResult> MapErr<TResult>(Func<TErr, TResult> f)
        {
            if (f == null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            if (this.IsOk)
            {
                return Result.Ok<TOk, TResult>(this._ok);
            }

            return Result.Err<TOk, TResult>(f(this._err));
        }

        public IEnumerable<Option<TOk>> ToEnumerable()
        {
            if (this.IsOk)
            {
                yield return Option.Some(this._ok);
            }
            else
            {
                yield return Option.None<TOk>();
            }
        }

        public IEnumerator<Option<TOk>> GetEnumerator()
        {
            if (this.IsOk)
            {
                yield return Option.Some(this._ok);
            }
            else
            {
                yield return Option.None<TOk>();
            }
        }

        public Result<TResult, TErr> And<TResult>(Result<TResult, TErr> o)
        {
            if (this.IsOk)
            {
                return o;
            }

            return Result.Err<TResult, TErr>(this._err);
        }

        public Result<TResult, TErr> AndThen<TResult>(Func<TOk, Result<TResult, TErr>> f)
        {
            if (this.IsOk)
            {
                return f(this._ok);
            }

            return Result.Err<TResult, TErr>(this._err);
        }

        public Result<TOk, TErr> Or(Result<TOk, TErr> o)
        {
            if (this.IsErr)
            {
                return o;
            }

            return Result.Ok<TOk, TErr>(this._ok);
        }

        public Result<TErr, TErr> OrElse(Func<TErr, Result<TErr, TErr>> f)
        {
            if (this.IsErr)
            {
                return f(this._err);
            }

            return Result.Ok<TErr, TErr>((TErr) (object) this._ok);
        }

        public TOk Expect(string message)
        {
            if (this.IsOk)
            {
                return this._ok;
            }

            throw new UnExpectedException($"{message}: {this._err}");
        }

        public TErr ExpectErr(string message)
        {
            if (this.IsErr)
            {
                return this._err;
            }

            throw new UnExpectedException($"{message}: {this._ok}");
        }

        public TOk Unwrap()
        {
            if (this.IsOk)
            {
                return this._ok;
            }

            throw new UnwrapException(this.ToString());
        }

        public TOk UnwrapOr(TOk val)
        {
            if (this.IsOk)
            {
                return this._ok;
            }

            return val;
        }

        public TOk UnwrapOrDefault()
        {
            if (this.IsOk)
            {
                return this._ok;
            }

            return default(TOk);
        }

        public TOk UnwrapOrElse(Func<TErr, TOk> f)
        {
            if (this.IsOk)
            {
                return this._ok;
            }

            return f != null ? f(this._err) : default(TOk);
        }

        public TErr UnwrapErr()
        {
            if (this.IsOk)
            {
                throw new UnwrapException($"{nameof(this.UnwrapErr)}: {this}");
            }

            return this._err;
        }
    }
}
