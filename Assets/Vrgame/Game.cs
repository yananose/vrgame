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
    using Omochaya.Audio;
    using Omochaya.Common;
    using Omochaya.Debug;
    using Omochaya.Vr;
    using UnityEngine;

    /// <summary>
    /// The game.
    /// </summary>
    public class Game : Part<Transform>
    {
        /// <summary>The monsters.</summary>
        [SerializeField]
        private List<GameObject> monsters = null;

        /// <summary>The person.</summary>
        [SerializeField]
        private Person person = null;

        /// <summary>The knight.</summary>
        [SerializeField]
        private Knight knight = null;

        /// <summary>The monster no.</summary>
        private int monsterNo = 0;

        /// <summary>The scenario.</summary>
        private Scenario scenario = null;

        /// <summary>The voice command.</summary>
        private VoiceCommand voiceCommand = null;

        /// <summary>The awake.</summary>
        private void Awake()
        {
            // 横持ち
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;

            // スリープオフ
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // サンプルモデル
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
            this.scenario = new Scenario(Scenario());
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            this.scenario.Update();
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            // ボイスコマンド開始待ち
            this.voiceCommand = this.gameObject.AddComponent<VoiceCommand>();
            yield return this.voiceCommand.IsStart;

            // メインループ
            while (true)
            {
                yield return null;
                var skip = Input.GetMouseButtonUp(0);
                // 開始待ち
                this.person.IsEnable = false;
                while (!this.person.IsEnable && !skip)
                {
                    this.person.IsEnable = this.voiceCommand.IsHit(4) && this.person.IsStable;
                    //Debug.Log(""+ this.voiceCommand.IsHit(4));
                    yield return null;
                    skip = Input.GetMouseButtonUp(0);
                }

                // メイン
                while (!skip)
                {
                    if (this.voiceCommand.IsHit(4))
                    {
                        this.knight.Attack();
                    }
                    yield return null;
                    skip = Input.GetMouseButtonUp(0);
                }

                // 次の画面
                monsters[monsterNo].SetActive(false);
                monsterNo++;
                monsterNo %= monsters.Count;
                monsters[monsterNo].SetActive(true);
            }
        }
    }
}