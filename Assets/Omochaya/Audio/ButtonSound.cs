// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ButtonSound.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The button sound.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Audio
{
    using System;
    using Omochaya.Common;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>The button sound.</summary>
    public class ButtonSound : Part<Button>
    {
        /// <summary>Gets the component.</summary>
        private Button button { get { return this.Component0; } }

        /// <summary>The audio clip.</summary>
        [SerializeField]
        private AudioClip audioClip = null;

        /// <summary>The is voice.</summary>
        [SerializeField]
        private bool isVoice = false;

        /// <summary>The audio clip.</summary>
        public AudioClip AudioClip { get { return this.audioClip; } set { this.audioClip = value; } }

        /// <summary>The start.</summary>
        private void Start()
        {
            var onClick = this.button.onClick;
            onClick.RemoveListener(this.Play);
            onClick.AddListener(this.Play);
        }

        /// <summary>The play.</summary>
        private void Play()
        {
            if (this.isVoice)
            {
                AudioPlayer.Ins.PlayVoice(this.audioClip);
            }
            else
            {
                AudioPlayer.Ins.PlaySe(this.audioClip, 0.5f);
            }
        }
    }
}