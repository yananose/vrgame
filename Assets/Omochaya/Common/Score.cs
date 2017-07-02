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

    /// <summary>The score.</summary>
    public class Score
    {
        /// <summary>The now.</summary>
        private int now = 0;

        /// <summary>The pass line.</summary>
        private float passLine = 0f;

        /// <summary>The time.</summary>
        private int time = 0;

        /// <summary>The is pass.</summary>
        public bool IsPass { get; private set; }

        /// <summary>The constructor.</summary>
        public Score(float passLine, int time)
        {
            this.now = 0;
            this.passLine = passLine;
            this.time = Math.Max(1, time);
            this.IsPass = false;
        }

        /// <summary>The update.</summary>
        public bool Update(float diff, float diffCand)
        {
            var ret = false;
            if (diff < this.passLine)
            {
                this.now++;
            }
            else if (diffCand < this.passLine)
            {
                this.now--;
            }

            if (this.now < 0)
            {
                this.now = this.time;
                this.IsPass = false;
                ret = true;
            }
            else
            {
                this.now = Math.Min(this.now, this.time * 3);
                if (this.time * 2 < this.now)
                {
                    this.IsPass = true;
                }
            }

            return ret;
        }
    }
}