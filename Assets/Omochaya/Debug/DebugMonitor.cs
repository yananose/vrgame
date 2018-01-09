// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugMonitor.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The debug monitor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Debug
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using Omochaya.Common;

    /// <summary>
    /// The debug monitor.
    /// </summary>
    public class DebugMonitor : Part<Text>
    {
        /// <summary>Gets the text.</summary>
        public Text Text { get { return this.Component0; } }

        /// <summary>Gets the component.</summary>
        public Func<string> Func { private get; set; }

        /// <summary>The on.</summary>
        public static DebugMonitor On(Func<string> func, GameObject prefab)
        {
            if (prefab)
            {
                var ret = GameObject.Instantiate(prefab).GetComponent<DebugMonitor>();
                ret.Func = func;
                return ret;
            }
            else
            {
                return null;
            }
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            if (this.Func != null)
            {
                this.Text.text = Func();
            }
        }
    }
}