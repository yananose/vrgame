﻿// --------------------------------------------------------------------------------------------------------------------
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
    using Omochaya.Common;
    using Omochaya.Debug;

    /// <summary>The pause.</summary>
    public class Person : Part<Transform>
    {
        /// <summary>The neck.</summary>
        [SerializeField]
        private Transform neck = null;

        /// <summary>The scenario.</summary>
        private Scenario scenario = null;

        /// <summary>The gyro caribration.</summary>
        private Caribration gyroCaribration;

        /// <summary>The gravity caribration.</summary>
        private Caribration gravityCaribration;

        /// <summary>The gravity caribration z.</summary>
        private float gravityCaribrationZ = 0f;

        /// <summary>The gravity caribration x.</summary>
        private float gravityCaribrationX = 0f;

        /// <summary>Gets the is stable.</summary>
        public bool IsStable { get; private set; }

        /// <summary>Gets or sets the is enable.</summary>
        public bool IsEnable { get; set; }

        /// <summary>Gets or sets the angles.</summary>
        public Vector3 Angles { get; set; }

        /// <summary>Gets the neck.</summary>
        public Transform Neck { get { return this.neck; } }

        /// <summary>Gets or sets the is lead.</summary>
        public bool IsLead { get; set; }

        /// <summary>Gets or sets the rate.</summary>
        public Vector3 Rate { get; set; }

        /// <summary>Gets the caribration score.</summary>
        public float CaribrationScore
        {
            get
            {
                if (this.gyroCaribration == null || this.gravityCaribration == null)
                {
                    return 0f;
                }

                return Math.Min(this.gyroCaribration.Score, this.gravityCaribration.Score);
            }
        }

        /// <summary>The awake.</summary>
        private void Awake()
        {
            this.IsLead = true;
            this.Angles = Vector3.zero;
            this.Rate = Vector3.one;
        }

        /// <summary>The start.</summary>
        private void Start()
        {
            // delta time 更新
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            this.SetDeltaTime(1f / Application.targetFrameRate);
            this.scenario = new Scenario(Scenario());
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            Input.gyro.enabled = true;
            var bodyAngles = this.transform.localEulerAngles;
            var neckAngles = neck.transform.localEulerAngles;
            neckAngles.x = this.Angles.x;
            bodyAngles.y = this.Angles.y;
            neckAngles.z = this.Angles.z;
            this.transform.localEulerAngles = bodyAngles;
            neck.transform.localEulerAngles = neckAngles;
        }

        /// <summary>The fixed update.</summary>
        private void FixedUpdate()
        {
            if (Input.gyro.enabled)
            {
                this.scenario.Update();
            }
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            while (true)
            {
                yield return null;

                // 初期化
                this.IsStable = false;
                this.IsEnable = false;
                this.gyroCaribration = new Caribration(0.0115f, 0.6f, 2f, 1f / 4f);
                this.gravityCaribration = new Caribration(0.008f, 0.6f, 0f, 1f / 4f);
                this.Angles = Vector3.zero;

                // 開始待ち
                while (!this.IsEnable)
                {
                    yield return null;
                    this.gyroCaribration.Update(Input.gyro.rotationRate);
                    this.gravityCaribration.Update(Input.gyro.gravity.normalized);
                    var old = this.IsStable;
                    this.IsStable = this.gyroCaribration.IsPass && this.gravityCaribration.IsPass;
                    if (!old && this.IsStable)
                    {
                        this.gyroCaribration.SetMax();
                        this.gravityCaribration.SetMax();
                    }
                }

                {
                    var g = this.gravityCaribration.Value;
                    var xy = Mathf.Sqrt(g.x * g.x + g.y * g.y);
                    this.gravityCaribrationZ = Mathf.Atan2(-g.x, -g.y);
                    this.gravityCaribrationX = Mathf.Atan2(-g.z, xy);
                }

                // メイン
                yield return this.UpdateNeck;
            }
        }

        /// <summary>The update neck.</summary>
        private bool UpdateNeck()
        {
            if (this.IsLead)
            {
                // ジャイロで回転
                var angles = this.Angles;
                var gyro = Input.gyro.rotationRate - this.gyroCaribration.Value;
                var rad = angles.z * Mathf.Deg2Rad;
                var sin = Mathf.Sin(rad);
                var cos = Mathf.Cos(rad);
                gyro.x *= this.Rate.x;
                gyro.y *= this.Rate.y;
                gyro.z *= this.Rate.z;
                angles.x -= gyro.x * cos - gyro.y * sin;
                angles.y -= gyro.y * cos + gyro.x * sin;
                angles.z += gyro.z;

                // 重力で回転
                var g = Input.gyro.gravity;
                if (g.y < -0.1f)
                {
                    var xy = Mathf.Sqrt(g.x * g.x + g.y * g.y);
                    var gz = Mathf.Atan2(-g.x, -g.y) - this.gravityCaribrationZ;
                    var gx = Mathf.Atan2(-g.z, xy) - this.gravityCaribrationX;
                    angles.z = Mathf.LerpAngle(angles.z, gz * Mathf.Rad2Deg, 0.05f);
                    angles.x = Mathf.LerpAngle(angles.x, gx * Mathf.Rad2Deg, 0.05f);
                }

                angles.x = Math.Min(angles.x, 75);
                angles.x = Math.Max(angles.x, -89);
                this.Angles = angles;
            }

            // 反映
            return this.IsEnable;
        }

        /// <summary>The set delta time.</summary>
        private void SetDeltaTime(float deltaTime)
        {
            Time.fixedDeltaTime
                = Input.gyro.updateInterval
                = deltaTime;
            Debug.Log("SetDeltaTime:" + deltaTime);
        }
    }
}