// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlickArea.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The flick area.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    /// <summary>The flick area.</summary>
    public class FlickArea : Part, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        /// <summary>The interval.</summary>
        [SerializeField]
        private float interval = 0.1f;

        /// <summary>The distance.</summary>
        [SerializeField]
        private float distance = 100f;

        /// <summary>The rigor.</summary>
        [SerializeField]
        private float rigor = 0.7f;

        /// <summary>The start position.</summary>
        private Vector2 startPosition = Vector2.zero;

        /// <summary>The drag position.</summary>
        private Vector2 dragPosition = Vector2.zero;

        /// <summary>The time limit.</summary>
        private float timeLimit = 0f;

        /// <summary>The is release.</summary>
        private bool isRelease = false;

        /// <summary>Gets or sets the direction.</summary>
        public Vector2 Direction { get; private set; }

        /// <summary>Gets or sets the is click.</summary>
        public bool IsClick { get; private set; }

        /// <summary>The is hit.</summary>
        public bool IsHit(Vector2 direction)
        {
            return this.distance * this.rigor <= Vector2.Dot(this.Direction, direction.normalized);
        }

        /// <summary>The on begin drag.</summary>
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.timeLimit = Time.realtimeSinceStartup + this.interval;
            this.startPosition = this.dragPosition = eventData.position;
        }

        /// <summary>The on drag.</summary>
        public void OnDrag(PointerEventData eventData)
        {
            this.dragPosition = eventData.position;
        }

        /// <summary>The on end drag.</summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            this.isRelease = true;
        }

        /// <summary>The on pointer click.</summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            this.isRelease = true;
        }

        /// <summary>The initialize.</summary>
        public void Initialize()
        {
            this.isRelease = false;
            this.timeLimit = 0f;
            this.Direction =
            this.startPosition =
            this.dragPosition = Vector2.zero;
            this.IsClick = false;
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            // 伝達用パラメータリセット
            this.IsClick = false;
            this.Direction = Vector2.zero;

            // クリック検知
            if (this.isRelease)
            {
                this.isRelease = false;
                this.IsClick = 0 <= this.timeLimit;
                this.timeLimit = 0f;
            }
            else if(0f < this.timeLimit)
            {
                // フリック検知
                var time = Time.realtimeSinceStartup;
                if (time < this.timeLimit)
                {
                    var move = this.dragPosition - this.startPosition;
                    if (this.distance < move.magnitude)
                    {
                        this.Direction = move;
                        this.timeLimit = -1f;
                    }
                }
                else
                {
                    this.timeLimit = -1f;
                }
            }
        }
    }
}