// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Caribration.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The caribration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using Omochaya.Debug;
    using UnityEngine;

    /// <summary>The caribration.</summary>
    public class Caribration
    {
        /// <summary>The cand.</summary>
        private Vector3 cand = Vector3.zero;

        /// <summary>The score.</summary>
        private Score score = null;

        /// <summary>The sensitivity.</summary>
        private float sensitivity = 0f;

        /// <summary>The value.</summary>
        public Vector3 Value { get; private set; }

        /// <summary>The is pass.</summary>
        public bool IsPass { get { return this.score.IsPass; } }

        /// <summary>The constructor.</summary>
        public Caribration(float passLine, int time, float sensitivity)
        {
            this.score = new Score(passLine, time);
            this.sensitivity = Mathf.Max(0.001f, sensitivity);
        }

        /// <summary>The update.</summary>
        public void Update(Vector3 value)
        {
            this.cand = Vector3.LerpUnclamped(this.cand, value, this.sensitivity);
            var diff = Vector3.Distance(this.Value, value);
            var diffCand = Vector3.Distance(this.cand, value);
            if (this.score.Update(diff, diffCand))
            {
                this.Value = this.cand;
                DebugLog.Put("Calibration:" + this.Value.ToString("G3") + " <-" + value.ToString("G3"));
            }
        }
    }
}