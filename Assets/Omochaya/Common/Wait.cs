// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Wait.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The wait.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>The wait.</summary>
    public class Wait
    {
        /// <summary>The second.</summary>
        private float second;

        /// <summary>The constructor.</summary>
        public Wait(float second)
        {
            this.second = second;
        }

        /// <summary>The update.</summary>
        public bool Update()
        {
            this.second -= Time.deltaTime;
            return 0f < this.second;
        }
    }
}