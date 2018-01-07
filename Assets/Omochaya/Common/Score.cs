// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Score.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The score.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using UnityEngine;

    /// <summary>The score.</summary>
    public class Score
    {
        /// <summary>The pass line.</summary>
        private float passLine = 0f;

        /// <summary>The now.</summary>
        private float now = 0f;

        /// <summary>The min.</summary>
        private float min = 0f;

        /// <summary>The bad.</summary>
        private float bad = 0f;

        /// <summary>The hit.</summary>
        private float hit = 0f;

        /// <summary>The max.</summary>
        private float max = 0f;

        /// <summary>The is pass.</summary>
        public bool IsPass { get; private set; }

        /// <summary>Gets the now.</summary>
        public float Now { get { return this.now; } }

        /// <summary>The set max.</summary>
        public void SetMax() { this.now = this.max; }

        /// <summary>Gets the point.</summary>
        public float Point
        {
            get
            {
                if (this.IsPass)
                {
                    var a = this.bad;
                    var b = this.now - a;
                    var c = this.max - a;
                    var d = b / c;
                    d = Math.Max(d, 0f);
                    d = Math.Min(d, 1f);
                    return d;
                }
                else
                {
                    var a = this.min;
                    var b = this.now - a;
                    var c = this.hit - a;
                    var d = b / c;
                    d = Math.Max(d, 0f);
                    d = Math.Min(d, 1f);
                    return d;
                }
            }
        }

        /// <summary>The constructor.</summary>
        public Score(float passLine, float need, float stable)
        {
            this.passLine = passLine;
            this.IsPass = false;
            this.now = 0f;
            need = Math.Max(0.01f, need);
            this.bad = need / 2f;
            this.hit = need;
            this.max = Math.Max(need + 0.01f, stable);
        }

        /// <summary>The update.</summary>
        public bool Update(float diff, float diffCand)
        {
            float delta = Time.deltaTime;
            if (diff < this.passLine)
            {
                this.now += delta;
            }
            else if (diffCand < this.passLine)
            {
                this.now -= delta;
            }

            if (this.IsPass)
            {
                if (this.max < this.now)
                {
                    this.now = this.max;
                }
                else if (this.now < this.bad)
                {
                    this.IsPass = false;
                    this.now = this.min;
                }
            }
            else
            {
                if (this.now < this.min)
                {
                    this.now = this.min;
                }
                else if (this.hit < this.now)
                {
                    this.IsPass = true;
                    this.now = this.max;
                }
            }

            return this.now < this.bad;
        }
    }
}