// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The person.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Vr
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Omochaya.Common;
    using Omochaya.Vr.person;

    /// <summary>The pause.</summary>
    public class Person : Part<Transform>
    {
        /// <summary>The neck.</summary>
        [SerializeField]
        private Transform neck = null;

        /// <summary>The eyes.</summary>
        [SerializeField]
        private Eyes eyes = null;

        /// <summary>The caribration node.</summary>
        [SerializeField]
        private GameObject caribrationNode = null;

        /// <summary>The start node.</summary>
        [SerializeField]
        private GameObject startNode = null;

        /// <summary>The scenario.</summary>
        private Scenario scenario = null;

        /// <summary>The caribration.</summary>
        private Vector3 caribration = Vector3.zero;

        /// <summary>The caribration cand.</summary>
        private Vector3 caribrationCand = Vector3.zero;

        /// <summary>The caribration score.</summary>
        private int caribrationScore = 0;

        /// <summary>Gets the is usable gyro.</summary>
        public bool IsUsableGyro { get; private set; }

        /// <summary>Gets the is stable.</summary>
        public bool IsStable { get; private set; }

        /// <summary>Gets the is enable.</summary>
        public bool IsEnable { get; set; }

        /// <summary>Gets the neck.</summary>
        public Transform Neck { get { return neck; } }

        /// <summary>Gets the eyes.</summary>
        public Eyes Eyes { get { return eyes; } }

        /// <summary>The start.</summary>
        private void Start()
        {
            this.caribrationNode.SetActive(true);
            this.startNode.SetActive(false);
            this.scenario = new Scenario(Scenario());
        }

        /// <summary>The fixed update.</summary>
        private void FixedUpdate()
        {
            if (Input.gyro.enabled)
            {
                this.scenario.Update();
            }
            else
            {
                Input.gyro.enabled = true;
            }
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            while (true)
            {
                // 初期化
                this.IsStable = false;
                this.IsEnable = false;
                this.caribration = this.caribrationCand = Vector3.zero;
                this.caribrationScore = 2;
                this.neck.transform.localEulerAngles = Vector3.zero;

                // 開始待ち
                while (!this.IsEnable)
                {
                    var flag = this.Calibration();
                    this.IsStable = flag;
                    this.caribrationNode.SetActive(!flag);
                    this.startNode.SetActive(flag);
                    yield return null;
                }

                this.caribrationNode.SetActive(false);
                this.startNode.SetActive(false);

                // メイン
                yield return this.UpdateNeck;
            }
        }

        /// <summary>The calibration.</summary>
        private bool Calibration()
        {
            var gyro = Input.gyro.rotationRate;
            this.caribrationCand = Vector3.LerpUnclamped(this.caribrationCand, gyro, 1 / 30f);
            var diff = Vector3.Distance(this.caribration, gyro);
            var diffCand = Vector3.Distance(this.caribrationCand, gyro);
            if (diff < 0.015f)
            {
                this.caribrationScore++;
            }
            else if (0.05f < diff)
            {
                this.caribrationScore--;
            }
            if (diffCand < 0.015f)
            {
                this.caribrationScore--;
                if (this.caribrationScore < 0)
                {
                    this.caribrationScore = 8;
                    this.caribration = this.caribrationCand;
                }
            }
            return 0 < this.caribrationScore;
        }

        /// <summary>The update neck.</summary>
        private bool UpdateNeck()
        {
            // 現在の向きを取得
            var angles = neck.transform.localEulerAngles;

            // ジャイロで回転
            var rotation = Input.gyro.rotationRate - this.caribration;
            angles.x -= rotation.x * 2;
            angles.y -= rotation.y * 2;
            var x = angles.x;
            while (180 < x)
            {
                x -= 360;
            }
            angles.x = Mathf.Max(x, -80);
            angles.x = Mathf.Min(x, +80);

            // 反映
            neck.transform.localEulerAngles = angles;

            return this.IsEnable;
        }
    }
}