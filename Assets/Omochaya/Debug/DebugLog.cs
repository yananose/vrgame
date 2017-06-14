// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugLog.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The debug log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if DEVELOPMENT_BUILD || UNITY_EDITOR
#define DEBUG_LOG_ENABLE
#endif
namespace Omochaya.Debug
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The debug log.
    /// </summary>
    public static class DebugLog
    {
#if DEBUG_LOG_ENABLE
        /// <summary>The log.</summary>
        public static Action<object> Put = Debug.Log;

        /// <summary>The log warning.</summary>
        public static Action<object> Warning = Debug.LogWarning;

        /// <summary>The log error.</summary>
        public static Action<object> Error = Debug.LogError;

        /// <summary>The pause.</summary>
        public static void Pause()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
#else
        /// <summary>The log.</summary>
        /// <param name="msg">The msg.</param>
        [System.Diagnostics.Conditional("DEBUG_LOG_ENABLE")]
        public static void Put(object msg) { }

        /// <summary>The log warning.</summary>
        /// <param name="msg">The msg.</param>
        [System.Diagnostics.Conditional("DEBUG_LOG_ENABLE")]
        public static void Warning(object msg) { }

        /// <summary>The log error.</summary>
        /// <param name="msg">The msg.</param>
        [System.Diagnostics.Conditional("DEBUG_LOG_ENABLE")]
        public static void Error(object msg) { }

        /// <summary>The pause.</summary>
        [System.Diagnostics.Conditional("DEBUG_LOG_ENABLE")]
        public static void Pause() { }
#endif
    }
}