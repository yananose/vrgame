// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The game.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Vrgame
{
    using System;
    using System.Collections.Generic;
    using Omochaya.Common;
    using Omochaya.Debug;
    using UnityEngine;

    /// <summary>
    /// The game.
    /// </summary>
    public class Game : Part<Transform>
    {
        /// <summary>The monsters.</summary>
        [SerializeField]
        private List<GameObject> monsters = null;

        /// <summary>The monster no.</summary>
        private int monsterNo = 0;

        /// <summary>The scenario.</summary>
        private static Scenario scenario = null;

        /// <summary>Gets the is usable gyro.</summary>
        public static bool IsUsableGyro { get; set; }

        /// <summary>Gets the gyro.</summary>
        public static Vector3 Gyro { get { return Game.IsUsableGyro ? Input.gyro.rotationRate : Vector3.zero; } }

        /// <summary>The awake.</summary>
        private void Awake()
        {
            // 横持ち
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            monsterNo = Math.Max(monsterNo, 0);
            monsterNo = Math.Min(monsterNo, monsters.Count - 1);
            for (var i=0; i < monsters.Count ; i++)
            {
                monsters[i].SetActive(i == monsterNo);
            }
        }

        /// <summary>The on destroy.</summary>
        private void OnDestroy()
        {
            Resources.UnloadUnusedAssets();
        }

        /// <summary>The start.</summary>
        private void Start()
        {
            if (Game.scenario == null)
            {
                Game.scenario = new Scenario(Scenario());
            }
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            Game.scenario.Update();
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            yield return null;

            // ジャイロ有効？
            Input.gyro.enabled = true;
            Game.IsUsableGyro = Input.gyro.enabled;

            while (true)
            {
                // 画面タップで次
                if (Input.GetMouseButtonUp(0))
                {
                    monsters[monsterNo].SetActive(false);
                    monsterNo++;
                    monsterNo %= monsters.Count;
                    monsters[monsterNo].SetActive(true);
                }

                yield return null;
            }

            Resources.UnloadUnusedAssets();
            yield return null;

            DebugLog.Put("アプリ終了");
            Application.Quit();
        }
    }
}