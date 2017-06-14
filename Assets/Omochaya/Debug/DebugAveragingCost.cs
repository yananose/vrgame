// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugAveragingCost.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The debug averaging cost.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
namespace Omochaya.Debug
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The debug averaging cost.
    /// </summary>
    public class DebugAveragingCost
    {
        /// <summary>The value.</summary>
        public float Value { get; private set; }

        /// <summary>The interval.</summary>
        private int interval = 20;

        /// <summary>The buf.</summary>
        private float buf = 0f;

        /// <summary>The count.</summary>
        private int count = 0;

        /// <summary>The cost.</summary>
        public class Cost : IDisposable
        {
            /// <summary>The averaging.</summary>
            private DebugAveragingCost averaging;

            /// <summary>The raw.</summary>
            private float time = 0f;

            /// <summary>The constructor.</summary>
            internal Cost(DebugAveragingCost averaging)
            {
                this.averaging = averaging;
                this.time = Time.realtimeSinceStartup;
            }

            /// <summary>The dispose.</summary>
            public void Dispose()
            {
                if (this.averaging.interval <= ++this.averaging.count)
                {
                    this.averaging.Value = this.averaging.buf * 1000f / this.averaging.count;
                    this.averaging.buf = 0f;
                    this.averaging.count = 0;
                }
                else
                {
                    this.averaging.buf += Time.realtimeSinceStartup - this.time;
                }
            }
        }

        /// <summary>The constructor.</summary>
        public DebugAveragingCost(int interval = 20) { this.interval = interval; }

        /// <summary>The measure.</summary>
        public Cost Measure { get { return new Cost(this); } }

        /// <summary>The to string.</summary>
        public override string ToString() { return this.Value.ToString("f").PadLeft(6) + " ms"; }
    }
}
#endif
