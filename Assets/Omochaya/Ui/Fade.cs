// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fade.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Ui
{
    using System;
    using Omochaya.Common;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>The fade.</summary>
    public class Fade : Part<Image>
    {
        /// <summary>Gets the component.</summary>
        public Image Image { get { return this.Component0; } }

        /// <summary>The tween.</summary>
        private Tween tween = null;

        /// <summary>The is out.</summary>
        public bool IsOut { get { return this.tween != null && this.tween.IsReverse; } }

        /// <summary>The is pause.</summary>
        public bool IsPause
        {
            get
            {
                return this.tween.IsPause;
            }

            set
            {
                this.tween.IsPause = value;
            }
        }

        /// <summary>The in color.</summary>
        public Color InColor { get; set; }

        /// <summary>The out color.</summary>
        public Color OutColor { get; set; }

        /// <summary>The is busy.</summary>
        public bool IsBusy() { return this.tween.IsBusy; }

        /// <summary>The complete.</summary>
        public bool Complete()
        {
            var ret = this.tween.Complete();
            if (!this.IsBusy() && this.Image.color.a == 0f)
            {
                this.Enable = false;
            }

            return ret;
        }

        /// <summary>The reverse.</summary>
        public void Reverse() { this.tween.Reverse(); }

        /// <summary>The in.</summary>
        public void In(float time = 0f, Func<bool> callback = null, Tween.Ease easeType = Tween.Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            this.Initialize();
            if (0 < time)
            {
                this.SetColor(0);
                this.Enable = true;
                this.tween.Start(time, this.SetColor, callback, easeType, easeLevel, ease);
            }
            else
            {
                this.tween.Exit();
                this.SetColor(1);
                this.Enable = false;
                if (this.tween.IsReverse)
                {
                    this.tween.Reverse();
                }
            }
        }

        /// <summary>The out.</summary>
        public void Out(float time = 0f, Func<bool> callback = null, Tween.Ease easeType = Tween.Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            this.Initialize();
            if (0f < time)
            {
                this.SetColor(1);
                this.Enable = true;
                this.tween.Start(-time, this.SetColor, callback, easeType, easeLevel, ease);
            }
            else
            {
                this.tween.Exit();
                this.SetColor(0);
                this.Enable = true;
                if (!this.tween.IsReverse)
                {
                    this.tween.Reverse();
                }
            }
        }

        /// <summary>The initialize.</summary>
        protected void Initialize()
        {
            if(this.tween == null)
            {
                this.tween =  new Tween();
                var color = this.Image.color;
                var a = color.a;
                color.a = 0f;
                this.InColor = color;
                color.a = 1f;
                this.OutColor = color;
                if (0 < a)
                {
                    this.Image.color = this.OutColor;
                }
                else
                {
                    this.Image.color = this.InColor;
                }
            }
        }

        /// <summary>The update.</summary>
        protected void Update()
        {
            if (!this.tween.Calc() && !this.IsBusy() && this.Image.color.a == 0f)
            {
                this.Enable = false;
            }
        }

        /// <summary>The update.</summary>
        private void SetColor(float rate)
        {
            this.Image.color = Color.Lerp(this.OutColor, this.InColor, rate);
        }
    }
}