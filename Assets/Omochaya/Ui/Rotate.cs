// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Rotate.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The rotate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Ui
{
    using System;
    using System.Collections.Generic;
    using Omochaya.Common;
    using UnityEngine;

    /// <summary>The miss.</summary>
    public class Rotate : Part
    {
        /// <summary>The sp.</summary>
        [SerializeField]
        private Transform sp = null;

        /// <summary>The arrow.</summary>
        [SerializeField]
        private GameObject arrow = null;

        /// <summary>The tween.</summary>
        private Tween tween = new Tween();

        /// <summary>The scenario.</summary>
        private Scenario scenario = new Scenario();

        /// <summary>The on.</summary>
        public void On()
        {
            this.Enable = true;
            this.scenario.Set(this.Scenario());
        }

        /// <summary>The on.</summary>
        public void Off()
        {
            this.Enable = false;
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            this.scenario.Update();
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            while (true)
            {
                this.tween.Start(1f);
                yield return this.tween.Calc;
                this.arrow.SetActive(true);
                this.arrow.transform.localScale = Vector3.one;
                this.tween.Start(1f);
                yield return this.tween.Calc;
                this.arrow.SetActive(false);
                this.tween.Start(1f, 0f, 90f, this.Animation, null, Tween.Ease.InOut, 2);
                yield return this.tween.Calc;

                this.tween.Start(1f);
                yield return this.tween.Calc;
                this.arrow.SetActive(true);
                this.arrow.transform.localScale = new Vector3(-1f, 1f, 1f);
                this.tween.Start(1f);
                yield return this.tween.Calc;
                this.arrow.SetActive(false);
                this.tween.Start(-1f, 0f, 90f, this.Animation, null, Tween.Ease.InOut, 2);
                yield return this.tween.Calc;
            }
        }

        /// <summary>The animation.</summary>
        private void Animation(float prm)
        {
            this.sp.localEulerAngles = new Vector3(0f, 0f, prm);
        }
    }
}