// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Knight.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The knight.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Vrgame
{
    using Omochaya.Common;
    using UnityEngine;

    /// <summary>
    /// The game.
    /// </summary>
    public class Knight : Part<Animator>
    {
        /// <summary>The monster no.</summary>
        private int monsterNo = 0;

        /// <summary>Gets the component.</summary>
        public Animator Animator { get { return this.Component0; } }

        /// <summary>The freeze.</summary>
        private int freeze = 0;

        /// <summary>The attack.</summary>
        public bool Attack()
        {
            bool ret = this.freeze < Time.frameCount;
            if(ret)
            {
                this.freeze = Time.frameCount + 25;
                this.Animator.CrossFade("WK_heavy_infantry_08_attack_B", 0f);
            }

            return ret;
        }
    }
}