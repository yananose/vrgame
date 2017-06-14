// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Res.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The res.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya
{
    using UnityEngine;

    /// <summary>The pause.</summary>
    public class Res : MonoBehaviour
    {
        /// <summary>The c.</summary>
        public static Res C = new Res();

        /// <summary>The vr ui.</summary>
        [SerializeField]
        private Material vrUi;

        /// <summary>The vr ui.</summary>
        public Material VrUi { get { return vrUi; } }
    }
}