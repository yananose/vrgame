// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bits.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The bits.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;

    /// <summary>The bits.</summary>
    public class Bits<T> where T : struct
    {
        /// <summary>The bits.</summary>
        private long bits = 0;

        /// <summary>The clear.</summary>
        public void Clear()
        {
            this.bits = 0;
        }

        /// <summary>The set.</summary>
        public void Set(params T[] es)
        {
            foreach (var e in es)
            {
                this.bits |= EnumToBit(e);
            }
        }

        /// <summary>The copy.</summary>
        public void Copy(Bits<T> b)
        {
            this.bits = b.bits;
        }

        /// <summary>The unset.</summary>
        public void Unset(params T[] es)
        {
            foreach (var e in es)
            {
                this.bits &= ~EnumToBit(e);
            }
        }

        /// <summary>The Contains.</summary>
        public bool Contains(T e)
        {
            return (this.bits & EnumToBit(e)) != 0;
        }

        /// <summary>The or.</summary>
        public bool Or(params T[] es)
        {
            foreach (var e in es)
            {
                if (this.Contains(e))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>The and.</summary>
        public bool And(params T[] es)
        {
            foreach (var e in es)
            {
                if (!this.Contains(e))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>The enum to bit.</summary>
        static private long EnumToBit(T e)
        {
            return 1 << Convert.ToInt32(e);
        }

        /// <summary>The override operator.</summary>
        public static bool operator ==(Bits<T> a, Bits<T> b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.bits == b.bits;
        }

        /// <summary>The override operator.</summary>
        public static bool operator !=(Bits<T> a, Bits<T> b)
        {
            return !(a == b);
        }

        /// <summary>The override operator.</summary>
        public override bool Equals(System.Object o)
        {
            if (o == null)
            {
                return false;
            }

            Bits<T> b = o as Bits<T>;
            if ((System.Object)b == null)
            {
                return false;
            }

            return this.bits == b.bits;
        }

        /// <summary>The override operator.</summary>
        public bool Equals(Bits<T> b)
        {
            if ((object)b == null)
            {
                return false;
            }

            return this.bits == b.bits;
        }

        /// <summary>The override operator.</summary>
        public override int GetHashCode()
        {
            return (int)(this.bits ^ (this.bits >> 32));
        }
    }
}