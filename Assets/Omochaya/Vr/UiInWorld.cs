// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UiInWorld.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The ui in world.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Vr
{
    using UnityEngine;

    /// <summary>The ui in world.</summary>
    public class UiInWorld : MonoBehaviour
    {
        /// <summary>The size.</summary>
        [SerializeField]
        private float size = 640f;

        /// <summary>The canvas.</summary>
        [SerializeField]
        private RectTransform canvas = null;

        /// <summary>The eyes.</summary>
        [SerializeField]
        private Eyes eyes = null;

        /// <summary>The is scaling.</summary>
        [SerializeField]
        private bool isScaling = true;

        /// <summary>The eyes.</summary>
        public Eyes Eyes { get { return this.eyes; } }

        /// <summary>The setup.</summary>
        public void Setup(Eyes eyes, bool isScaling)
        {
            this.eyes = eyes;
            this.isScaling = isScaling;
        }

        /// <summary>The update.</summary>
        /// ※ 毎フレームは重いかも。
        private void Update()
        {
            var camera = this.eyes.Left;
            var aspect = camera.rect.width * Screen.width / camera.rect.height / Screen.height;
            var fieldOfView = camera.fieldOfView;
            var view = Vector2.one * this.size;
            if (aspect < 1f)
            {
                view.y /= aspect;
            }
            else
            {
                view.x *= aspect;
            }

            this.canvas.anchoredPosition = -view / 2f;
            this.canvas.sizeDelta = view;

            var position = this.transform.position;
            var d = position - this.eyes.transform.position;
            var distance = this.isScaling ? d.magnitude : 5f;
            var viewSize = distance * Mathf.Tan(fieldOfView * Mathf.PI / 360f) * 2;
            var scale = Vector3.one;
            scale.x = scale.y = viewSize / view.y;
            this.transform.localScale = scale;
            this.transform.LookAt(position + d, Vector3.up);
        }
    }
}