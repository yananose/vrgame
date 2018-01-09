// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Initializer.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The initializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using UnityEngine;

    /// <summary>The initializer.</summary>
    public class Initializer : MonoBehaviour
    {
        /// <summary>The staying object.</summary>
        private static GameObject stayingObject = null;

        /// <summary>The initial prefab.</summary>
        [SerializeField]
        private GameObject initialPrefab = null;

        /// <summary>The awake.</summary>
        private void Awake()
        {
            if (Initializer.stayingObject == null)
            {
                Initializer.stayingObject = GameObject.Instantiate(this.initialPrefab);
                GameObject.DontDestroyOnLoad(Initializer.stayingObject);
            }
        }
    }
}