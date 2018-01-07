// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Misc.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The misc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System.Collections;
    using Omochaya.Debug;
    using UnityEngine;

    /// <summary>The misc.</summary>
    public static class Misc
    {
        /// <summary>The closest approach.</summary>
        public static float ClosestApproach(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1)
        {
            var ad = a1 - a0;
            var bd = b1 - b0;
            var ab = b0 - a0;
            var dd = ad - bd;
            var ret = Vector3.Dot(ab, dd) / Vector3.Dot(dd, dd);
            return ret;
        }

        /// <summary>The one time effect.</summary>
        public class OneTimeEffect : Part<ParticleSystem>
        {
            /// <summary>The start.</summary>
            private void Start()
            {
                StartCoroutine(WaitEnd());
            }

            /// <summary>The wait end.</summary>
            private IEnumerator WaitEnd()
            {
                yield return new WaitWhile(() => { return !Component0.isStopped; });
                GameObject.Destroy(this.gameObject);
            }
        }

        /// <summary>The instanciate.</summary>
        public static GameObject Instantiate(GameObject prefab, Transform parent)
        {
            var gameObject = GameObject.Instantiate(prefab);
            gameObject.transform.SetParent(parent, false);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localEulerAngles = Vector3.zero;
            return gameObject;
        }

        /// <summary>The play effect.</summary>
        public static GameObject PlayEffect(GameObject prefab, Transform parent, Vector3 position)
        {
            var gameObject = Misc.Instantiate(prefab, parent);
            gameObject.transform.position = position;
            gameObject.AddComponent<OneTimeEffect>();
            return gameObject;
        }

        /// <summary>The deceleration.</summary>
        public static Vector3 Deceleration(Vector3 speed, float dec)
        {
            var ret = speed - speed.normalized * dec;
            return 0f < Vector3.Dot(ret, speed) ? ret : Vector3.zero;
        }

        /// <summary>The random.</summary>
        public static int Random(int max)
        {
            return (int)(UnityEngine.Random.value * 0xFFFFFF) % max;
        }
    }
}