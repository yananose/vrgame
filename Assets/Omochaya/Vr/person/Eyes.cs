// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Eyes.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The eyes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Vr.person
{
    using UnityEngine;
    using Omochaya.Common;

    /// <summary>The pause.</summary>
    public class Eyes : Part<Transform>
    {
        /// <summary>The left.</summary>
        [SerializeField]
        private Camera left;

        /// <summary>The right.</summary>
        [SerializeField]
        private Camera right;

        /// <summary>Gets the left.</summary>
        public Camera Left { get { return left; } }

        /// <summary>Gets the right.</summary>
        public Camera Right { get { return right; } }

        /// <summary>The awake.</summary>
        private void Awake()
        {
        }

        /// <summary>The start.</summary>
        private void Start()
        {
        }

        /// <summary>The update.</summary>
        private void Update()
        {
        }
    }
}