// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AudioPlayer.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The audio player.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Audio
{
    using System.Collections.Generic;
    using Omochaya.Common;
    using Omochaya.Debug;
    using UnityEngine;

    /// <summary>The audio player.</summary>
    public class AudioPlayer : MonoBehaviour
    {
        /// <summary>The sources.</summary>
        private List<AudioSource> sources = new List<AudioSource>();

        /// <summary>The fadeouts.</summary>
        private List<Tween> fadeouts = new List<Tween>();

        /// <summary>The current.</summary>
        public static AudioPlayer Ins { get; set; }

        /// <summary>The volume bgm.</summary>
        public static float VolumeBgm { get; set; }

        /// <summary>The volume bgm get.</summary>
        public static float VolumeBgmGet { get { return AudioPlayer.VolumeBgm * 0.4f; } }

        /// <summary>The volume se.</summary>
        public static float VolumeSe { get; set; }

        /// <summary>The volume voice.</summary>
        public static float VolumeVoice { get; set; }

        /// <summary>The is mute bgm.</summary>
        public static bool IsMuteBgm { get; set; }

        /// <summary>The is mute se.</summary>
        public static bool IsMuteSe { get; set; }

        /// <summary>The is mute voice.</summary>
        public static bool IsMuteVoice { get; set; }

        /// <summary>The bgm.</summary>
        public AudioSource Bgm { get; private set; }

        /// <summary>The voice.</summary>
        public AudioSource Voice { get; private set; }

        /// <summary>The last time.</summary>
        public float LastTime { get; private set; }


        /// <summary>The play bgm.</summary>
        static AudioPlayer()
        {
            AudioPlayer.VolumeBgm = 1f;
            AudioPlayer.VolumeSe = 1f;
            AudioPlayer.VolumeVoice = 1f;
        }

        /// <summary>The play bgm.</summary>
        public AudioSource PlayBgm(AudioClip clip)
        {
            if (this.Bgm && this.Bgm.isPlaying)
            {
                this.StopBgm(0.2f);
            }

            this.Bgm = this.play(clip, AudioPlayer.IsMuteBgm ? 0f : AudioPlayer.VolumeBgmGet, true);
            return this.Bgm;
        }

        /// <summary>The stop bgm.</summary>
        public void StopBgm(float time=0f, AudioSource source = null)
        {
            source = source ?? this.Bgm;
            if (this.Bgm == source)
            {
                this.Bgm = null;
            }

            if (source)
            {
                this.stop(source, time);
            }
        }

        /// <summary>The play se.</summary>
        public AudioSource PlaySe(AudioClip clip, float volume = 1f, float delay = 0f)
        {
            var source = this.play(clip, AudioPlayer.IsMuteSe ? 0f : volume * AudioPlayer.VolumeSe, false, delay);
            source.ignoreListenerPause = true;
            return source;
        }

        /// <summary>The stop se.</summary>
        public void StopSe(AudioSource source, float time=0f)
        {
            this.stop(source, time);
        }

        /// <summary>The play voice.</summary>
        public AudioSource PlayVoice(AudioClip clip, float volume = 1f, float delay = 0f)
        {
            if (!this.Voice || !this.Voice.isPlaying)
            {
                this.Voice = this.play(clip, AudioPlayer.IsMuteVoice ? 0f : volume * AudioPlayer.VolumeVoice, false, delay);
            }

            return this.Voice;
        }

        /// <summary>The stop voice.</summary>
        public void StopVoice(float time = 0f, AudioSource source = null)
        {
            source = source ?? this.Voice;
            if (this.Voice == source)
            {
                this.Voice = null;
            }

            if (source)
            {
                this.stop(source, time);
            }
        }

        /// <summary>The play.</summary>
        private AudioSource play(AudioClip clip, float volume = 1f, bool loop = false, float delay = 0f)
        {
            AudioSource ret = null;
            foreach(var source in this.sources)
            {
                if (!source.isPlaying)
                {
                    ret = source;
                }
            }
            
            if(!ret)
            {
                ret = this.gameObject.AddComponent<AudioSource>();
                this.sources.Add(ret);
            }
            else if (this.Voice == ret)
            {
                this.Voice = null;
            }
            else if (this.Bgm == ret)
            {
                this.Bgm = null;
            }

            ret.clip = clip;
            ret.volume = volume;
            ret.loop = loop;
            ret.minDistance = 1f;
            ret.maxDistance = 100f;
            ret.spatialBlend = 1f;
            if (0f < delay)
            {
                ret.PlayDelayed(delay);
            }
            else
            {
                ret.Play();
            }

            this.LastTime = Time.realtimeSinceStartup;
            DebugLog.Put(clip.name);
            return ret;
        }

        /// <summary>The stop.</summary>
        private void stop(AudioSource source, float time = 0f)
        {
            if (source.isPlaying)
            {
                if (time <= 0f)
                {
                    source.Stop();
                }
                else
                {
                    var fadeout = new Tween();
                    fadeout.Start(
                        time,
                        source.volume,
                        0,
                        rate =>
                        {
                            if (source && source.isPlaying)
                            {
                                source.volume = rate;
                            }
                            else
                            {
                                source = null;
                            }
                        },
                        () =>
                        {
                            if (source)
                            {
                                source.Stop();
                            }

                            return true;
                        });
                    this.fadeouts.Add(fadeout);
                }
            }
        }

        /// <summary>The update.</summary>
        protected void Update()
        {
            for (var i = this.fadeouts.Count - 1; 0 <= i; i--)
            {
                var fadeout = this.fadeouts[i];
                if (!fadeout.Calc())
                {
                    this.fadeouts.RemoveAt(i);
                }
            }
        }
    }
}