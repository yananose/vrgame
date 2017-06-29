// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VoiceCommand.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The voice command.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Audio
{
    using System;
    using System.Collections.Generic;
    using Omochaya.Common;
    using UnityEngine;

    /// <summary>The voice command.</summary>
    public class VoiceCommand : MonoBehaviour
    {
        /// <summary>The frequency.</summary>
        private const int Frequency = 8192;

        /// <summary>The volume time.</summary>
        private const float VolumeTime = 0.05f;

        /// <summary>The audio source.</summary>
        private AudioSource audioSource = null;

        /// <summary>The scenario.</summary>
        private Scenario scenario = null;

        /// <summary>The is start.</summary>
        private bool isStart = false;

        /// <summary>The over count.</summary>
        private int overCount = 0;

        /// <summary>The volume.</summary>
        public float Volume { get; private set; }

        /// <summary>The is hit.</summary>
        public bool IsHit(int overCount)
        {
            return overCount < this.overCount;
        }

        /// <summary>The is start.</summary>
        public bool IsStart() { return this.isStart; }

        /// <summary>The start.</summary>
        private void Start()
        {
            this.scenario = new Scenario(Scenario());
            this.audioSource = this.gameObject.AddComponent<AudioSource>();
            this.audioSource.clip = Microphone.Start(null, true, 1, Frequency);
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            this.scenario.Update();
        }

        /// <summary>The scenario.</summary>
        private IEnumerator<Func<bool>> Scenario()
        {
            // マイクの準備完了を待つ
            while (Microphone.GetPosition(null) <= 0)
            {
                yield return null;
            }

            this.isStart = true;
            while (true)
            {
                // 毎フレームサンプリングして直近の音量取得
                var sum = 0f;
                var srcSize = (int)(this.audioSource.clip.frequency * this.audioSource.clip.channels * this.audioSource.clip.length);
                var dstSize = (int)(srcSize * VolumeTime);
                var end = Microphone.GetPosition(null);
                var start = end - dstSize;
                if (start < 0)
                {
                    // バッファの先頭から終端まで
                    var buf = new float[end];
                    this.audioSource.clip.GetData(buf, 0);
                    foreach (var data in buf)
                    {
                        sum += 0 < data ? data : -data;
                    }
                    // バッファの後端に余ってるぶん
                    buf = new float[-start];
                    this.audioSource.clip.GetData(buf, srcSize + start);
                    foreach (var data in buf)
                    {
                        sum += 0 < data ? data : -data;
                    }
                }
                else
                {
                    // 範囲が取れた
                    var buf = new float[dstSize];
                    this.audioSource.clip.GetData(buf, start);
                    foreach (var data in buf)
                    {
                        sum += 0 < data ? data : -data;
                    }
                }
                this.Volume = sum / dstSize;

                // 音量が連続で閾値を超えた回数をカウント
                if (0.01f < this.Volume)
                {
                    this.overCount++;
                }
                else
                {
                    this.overCount = 0;
                }

                yield return null;
            }
        }
    }
}