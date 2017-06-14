// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tween.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The tween.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>The tween.</summary>
    public class Tween
    {
        /// <summary>The action.</summary>
        private Action<float> action = null;

        /// <summary>The to.</summary>
        private float to = 0f;

        /// <summary>The callback.</summary>
        private Func<bool> callback = null;

        /// <summary>The callback.</summary>
        private List<Action<float>> callbacks = new List<Action<float>>();

        /// <summary>The callback no.</summary>
        private int callbackNo = 0;

        /// <summary>The add.</summary>
        private float add = 1f;

        /// <summary>The progress.</summary>
        private float progress = 0f;

        /// <summary>The time.</summary>
        private float time = 0f;

        /// <summary>The is callback.</summary>
        private bool isCallback = false;

        /// <summary>The is busy.</summary>
        public bool IsBusy { get { return this.callback != null; } }

        /// <summary>The is pause.</summary>
        public bool IsPause { get; set; }

        /// <summary>The is reverse.</summary>
        public bool IsReverse { get { return this.add < 0f; } }

        /// <summary>The ease.</summary>
        public enum Ease
        {
            Liner,
            In,
            Out,
            InOut,
            OutIn,
        }

        /// <summary>The constructor.</summary>
        public Tween()
        {
        }

        /// <summary>The effect quad.</summary>
        public static float EaseQuad(float rate) { return rate * rate; }

        /// <summary>The effect sine.</summary>
        public static float EaseSine(float rate) { return 1f - Mathf.Cos(rate * Mathf.PI / 2f); }

        /// <summary>The start.</summary>
        public void Start(float length, Action<float> action = null, Func<bool> callback = null, Tween.Ease easeType = Tween.Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            this.Exit();

            var reverse = false;
            // 逆再生？
            if (length < 0f)
            {
                reverse = true;
                length = -length;
            }

            this.callback = callback ?? Tween.Callback;
            this.Next(length, action, easeType, easeLevel, ease);
            this.callbacks[0](0f);
            if (reverse)
            {
                this.Reverse();
            }
            else if(action != null)
            {
                this.action(0);
            }
        }

        /// <summary>The start.</summary>
        public void Start(float length, float from, float to, Action<float> action = null, Func<bool> callback = null, Tween.Ease easeType = Tween.Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            this.Exit();

            // 逆再生？
            var reverse = false;
            if (length < 0f)
            {
                reverse = true;
                length = -length;
            }

            this.callback = callback ?? Tween.Callback;
            this.to = from;
            this.Next(length, to, action, easeType, easeLevel, ease);
            this.callbacks[0](0f);
            if (reverse)
            {
                this.Reverse();
            }
            else if (action != null)
            {
                this.action(0);
            }
        }

        /// <summary>The start.</summary>
        public void Start(float length, float from, Action<float> action = null)
        {
            this.Start(length, from, from, action);
        }

        /// <summary>The next.</summary>
        public void Next(float length, Action<float> action = null, Tween.Ease easeType = Tween.Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            action = Tween.MakeAction(action, easeType, easeLevel, ease);
            var self = this;
            this.callbacks.Add(over => self.ExecuteNext(over, length, action));
        }

        /// <summary>The next.</summary>
        public void Next(float length, float to, Action<float> action = null, Tween.Ease easeType = Tween.Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            action = this.LerpAction(action, to);
            action = Tween.MakeAction(action, easeType, easeLevel, ease);
            var self = this;
            this.callbacks.Add(over => self.ExecuteNext(over, length, action));
        }

        /// <summary>The complete.</summary>
        public bool Complete()
        {
            this.IsPause = false;
            this.time = float.NegativeInfinity;
            return !this.Calc();
        }

        /// <summary>The reverse.</summary>
        public void Reverse()
        {
            this.add = -this.add;
        }

        /// <summary>The exit.</summary>
        public void Exit()
        {
            if (this.isCallback)
            {
                this.Clear();
            }
            else
            {
                var callback = this.callback;
                if (callback != null)
                {
                    this.isCallback = true;
                    if (callback())
                    {
                        this.Clear();
                    }

                    this.isCallback = false;
                }
            }
        }

        /// <summary>The calc.</summary>
        public bool Calc()
        {
            // 未登録
            if (this.callback == null)
            {
                return false;
            }

            // 一時停止中
            var time = Time.realtimeSinceStartup;
            if (this.IsPause)
            {
                this.time = time;
                return true;
            }

            // 初期化
            if (this.time == 0f)
            {
                // 逆再生
                if (this.add < 0f)
                {
                    this.add = -this.add;
                    var callback = this.callback;
                    this.callback = () => { return false; };
                    this.Complete();
                    this.callback = callback;
                    this.add = -this.add;
                }

                this.time = time;
                return true;
            }

            // 時間をすすめる
            var progress = this.progress + this.add * (time - this.time);
            this.time = time;

            // 変化中
            if (0f < progress && progress < 1f)
            {
                this.progress = progress;
                if (this.action != null)
                {
                    this.action(progress);
                }

                return true;
            }

            // 終了
            if (0f < progress)
            {
                this.progress = 1f;
                progress -= 1f;
            }
            else
            {
                this.progress = 0f;
                progress = -progress;
            }

            this.ExecuteCallback(progress * this.add, this.action);
            return this.callback != null;
        }

        /// <summary>The callback.</summary>
        private static bool Callback() { return true; }

        /// <summary>The make action.</summary>
        private static Action<float> MakeAction(Action<float> action, Ease easeType = Ease.Liner, int easeLevel = 1, Func<float, float> ease = null)
        {
            if(action == null)
            {
                return null;
            }

            if (easeType == Ease.Liner || easeLevel <= 0)
            {
                return action;
            }

            if (ease == null)
            {
                ease = Tween.EaseQuad;
            }

            if (1 < easeLevel)
            {
                var core = ease;
                ease = rate => Tween.EaseLevel(rate, core, easeLevel);
            }

            switch (easeType)
            {
                case Ease.Out:
                    return rate => action(Tween.EaseOut(rate, ease));
                case Ease.InOut:
                    return rate => action(Tween.EaseInOut(rate, ease));
                case Ease.OutIn:
                    return rate => action(Tween.EaseOutIn(rate, ease));
                default:
                    return rate => action(ease(rate));
            }
        }

        /// <summary>The ease level.</summary>
        private static float EaseLevel(float rate, Func<float, float> ease, int level)
        {
            for (var i = 0; i < level; i++)
            {
                rate = ease(rate);
            }

            return rate;
        }

        /// <summary>The ease out.</summary>
        private static float EaseOut(float rate, Func<float, float> ease) { return 1f - ease(1f - rate); }

        /// <summary>The ease in out.</summary>
        private static float EaseInOut(float rate, Func<float, float> ease)
        {
            rate *= 2f;
            if (1f < rate)
            {
                rate -= 1f;
                rate = Tween.EaseOut(rate, ease);
                rate += 1f;
            }
            else
            {
                rate = ease(rate);
            }

            rate /= 2f;
            return rate;
        }

        /// <summary>The ease out in.</summary>
        private static float EaseOutIn(float rate, Func<float, float> ease)
        {
            rate *= 2f;
            if (1f < rate)
            {
                rate -= 1f;
                rate = ease(rate);
                rate += 1f;
            }
            else
            {
                rate = Tween.EaseOut(rate, ease);
            }

            rate /= 2f;
            return rate;
        }

        /// <summary>The lerp action.</summary>
        private Action<float> LerpAction(Action<float> action, float to)
        {
            var from = this.to;
            this.to = to;
            if (action != null)
            {
                return rate => action(Mathf.LerpUnclamped(from, to, rate));
            }
            else
            {
                return null;
            }
        }

        /// <summary>The execute next.</summary>
        private void ExecuteNext(float over, float length, Action<float> action)
        {
            if (-length < over && over < length)
            {
                this.progress = over / length;
                this.action = action;
                this.add = 1 / length;
                if (over < 0)
                {
                    this.progress += 1f;
                    this.add = -this.add;
                }
            }
            else
            {
                over -= 0 < over ? length : -length;
                this.ExecuteCallback(over, action);
            }
        }

        /// <summary>The execute callback.</summary>
        private void ExecuteCallback(float over, Action<float> action)
        {
            if (0 < this.add)
            {
                if(action != null)
                {
                    action(1f);
                }

                var no = this.callbackNo;
                if (++no < this.callbacks.Count)
                {
                    this.callbackNo = no;
                    this.callbacks[no](over);
                    return;
                }
            }
            else
            {
                if (action != null)
                {
                    action(0f);
                }

                var no = this.callbackNo;
                if (0 <= --no)
                {
                    this.callbackNo = no;
                    this.callbacks[no](over);
                    return;
                }
            }

            this.Exit();
        }

        /// <summary>The clear.</summary>
        private void Clear()
        {
            this.action = null;
            this.callback = null;
            this.callbacks.Clear();
            this.callbackNo = 0;
            this.IsPause = false;
            this.time = 0f;
        }
    }
}