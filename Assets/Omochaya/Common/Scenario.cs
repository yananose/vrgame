// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scenario.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The scenario.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>The scenario.</summary>
    public class Scenario
    {
        /// <summary>The current.</summary>
        private IEnumerator<Func<bool>> current = null;

        /// <summary>The stop.</summary>
        private Func<bool> stop = null;

        /// <summary>The constructor.</summary>
        public Scenario() { }

        /// <summary>The constructor.</summary>
        public Scenario(IEnumerator<Func<bool>> current)
        {
            this.Set(current);
        }

        /// <summary>The set.</summary>
        public void Set(IEnumerator<Func<bool>> current)
        {
            this.current = current;
            this.stop = null;
        }

        /// <summary>The update.</summary>
        public bool Update()
        {
            if (this.current != null)
            {
                if (this.stop == null || !this.stop())
                {
                    if (this.current.MoveNext())
                    {
                        this.stop = this.current.Current;
                    }
                    else
                    {
                        this.current = null;
                    }
                }
            }

            return this.current != null;
        }
    }
}