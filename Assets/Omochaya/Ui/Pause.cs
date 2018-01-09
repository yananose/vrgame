// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pause.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The pause.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Ui
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using Omochaya.Common;

    /// <summary>The pause.</summary>
    public class Pause : Part<Text>
    {
        /// <summary>Gets the text.</summary>
        private Text Text { get { return this.Component0; } }

        /// <summary>The callback.</summary>
        private Action callback = null;

        /// <summary>The on.</summary>
        public void On(Action callback)
        {
            this.callback = callback;
            this.Enable = true;
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            var time = (int)(Time.realtimeSinceStartup * 1000);
            this.Text.gameObject.SetActive((time & 0x300) != 0);
            if (Joypad.Ins.IsTouching)
            {
                if (this.callback != null)
                {
                    this.callback();
                }

                this.Enable = false;
            }
        }
    }
}