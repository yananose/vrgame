// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugPrefab.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The debug prefab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
namespace Omochaya.Debug
{
    using UnityEngine;

    /// <summary>The debug prefab.</summary>
    public class DebugPrefab : MonoBehaviour
    {
        /// いろいろ試してみたけど、スクリプトにデフォルトのリソースを設定するのはできないっぽい？

        /// <summary>The ins.</summary>
        private class Ins
        {
            /// <summary>Gets the debug prefab.</summary>
            public DebugPrefab DebugPrefab { get; private set; }

            /// <summary>The constructor.</summary>
            public Ins()
            {
                var go = new GameObject();
                go.name = "(debug prefab)";
                this.DebugPrefab = go.AddComponent<DebugPrefab>();
            }
        }

        /// <summary>The ins.</summary>
        private static Ins ins = new Ins();

        /// <summary>The monitor.</summary>
        [SerializeField]
        private GameObject monitor = null;

        /// <summary>The marker.</summary>
        [SerializeField]
        private GameObject marker = null;

        /// <summary>Gets the monitor.</summary>
        public static GameObject Monitor { get { return DebugPrefab.ins.DebugPrefab.monitor; } }

        /// <summary>Gets the marker.</summary>
        public static GameObject Marker { get { return DebugPrefab.ins.DebugPrefab.marker; } }
    }
}
#endif