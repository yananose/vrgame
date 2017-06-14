// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UiInWorld.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The ui in world.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Vrgame.Game.Camera
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

        /// <summary>The camera.</summary>
        [SerializeField]
        private new Camera camera = null;

        /// <summary>The aspect.</summary>
        private float aspect = 0f;

        /// <summary>The field of view.</summary>
        private float fieldOfView = 0f;

        /// <summary>The fit.</summary>
        public void Update()    // ToDo. もったいないので必要なときだけ実行するように。
        {
            var aspect = this.camera.rect.width * Screen.width / this.camera.rect.height / Screen.height;
            var fieldOfView = this.camera.fieldOfView;
            if (this.aspect != aspect || this.fieldOfView != fieldOfView)
            {
                this.aspect = aspect;
                this.fieldOfView = fieldOfView;

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

                var distance = this.transform.localPosition.z;
                var viewSize = distance * Mathf.Tan(fieldOfView * Mathf.PI / 360f) * 2f;
                var scale = Vector3.one;
                scale.x = scale.y = viewSize / view.y;
                this.transform.localScale = scale;
            }
        }
    }
}