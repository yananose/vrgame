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
    using UnityEngine;
    using Omochaya.Common;
    using Omochaya.Vr.person;

    /// <summary>The pause.</summary>
    public class Person : Part<Transform>
    {
        /// <summary>The neck.</summary>
        [SerializeField]
        private Transform neck;

        /// <summary>The eyes.</summary>
        [SerializeField]
        private Eyes eyes;

        /// <summary>Gets the neck.</summary>
        public Transform Neck { get { return neck; } }

        /// <summary>Gets the eyes.</summary>
        public Eyes Eyes { get { return eyes; } }

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
            // 現在の向きを取得
            var angles = neck.transform.localEulerAngles;

            // ジャイロで回転
            var rotation = Input.gyro.rotationRate;
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
        }
    }
}