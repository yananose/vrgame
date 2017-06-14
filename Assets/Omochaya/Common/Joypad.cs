// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Joypad.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The joypad.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// The joypad.
    /// </summary>
    public class Joypad
    {
        /// <summary>The current.</summary>
        public static Joypad Current { get; private set; }

        /// <summary>The shot.</summary>
        private KeyCode shot = KeyCode.None;

        /// <summary>The shot.</summary>
        private KeyCode pull = KeyCode.None;

        /// <summary>The shot.</summary>
        private KeyCode menu = KeyCode.None;

        /// <summary>The touch position.</summary>
        private Vector2 position;

        /// <summary>The mouse position.</summary>
        private Vector2 mousePosition;

        /// <summary>The constructor.</summary>
        static Joypad()
        {
            Joypad.Current = new Joypad();
        }

        /// <summary>The setpu.</summary>
        public void Setup(KeyCode shot, KeyCode pull, KeyCode menu)
        {
            this.shot = shot;
            this.pull = pull;
            this.menu = menu;
            EventSystem.current.sendNavigationEvents = false;
        }

        /// <summary>The update.</summary>
        public void Update()
        {
            var mousePosition = (Vector2)Input.mousePosition;
            var mouse = this.mousePosition != mousePosition;
            if (mouse)
            {
                this.mousePosition = mousePosition;
            }

            if (0 < Input.touchCount)
            {
                // タッチ位置優先
                this.position = Input.touches[0].position;
            }
            else if (mouse)
            {
                // マウスが動いていたらそっちをみる
                this.position = mousePosition;
            }
        }

        /// <summary>Gets the x.</summary>
        public float X
        {
            get
            {
                switch (this.pull)
                {
                    case KeyCode.UpArrow: return -Input.GetAxis("Horizontal");
                    case KeyCode.DownArrow: return Input.GetAxis("Horizontal");
                    case KeyCode.LeftArrow: return -Input.GetAxis("Vertical");
                    case KeyCode.RightArrow: return Input.GetAxis("Vertical");
                    default: return 0f;
                }
            }
        }

        /// <summary>Gets the y.</summary>
        public float Y
        {
            get
            {
                switch (this.pull)
                {
                    case KeyCode.UpArrow: return -Input.GetAxis("Vertical");
                    case KeyCode.DownArrow: return Input.GetAxis("Vertical");
                    case KeyCode.LeftArrow: return Input.GetAxis("Horizontal");
                    case KeyCode.RightArrow: return -Input.GetAxis("Horizontal");
                    default: return 0f;
                }
            }
        }

        /// <summary>Gets the direction.</summary>
        public Vector2 Direction
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }
        }

        /// <summary>Gets the is touching.</summary>
        public bool IsTouching { get { return Input.GetMouseButton(0) || Input.GetKey(this.shot) || 0 < Input.touchCount; } }

        /// <summary>Gets the touch position.</summary>
        public Vector2 touchPosition { get { return this.position; } }

        /// <summary>Gets the on shot.</summary>
        public bool onShot { get { return Input.GetKeyUp(this.shot); } }

        /// <summary>Gets the on menu.</summary>
        public bool onMenu { get { return Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(this.menu); } }

        /// <summary>Gets the is usable.</summary>
#if UNITY_EDITOR
        public bool IsUsable { get; set; }
#else
        public bool IsUsable { get { return 0 < Input.GetJoystickNames().Length; } }
#endif

        /// <summary>Gets the is enable.</summary>
        public bool IsEnabled { get { return this.IsUsable && this.shot != KeyCode.None && this.pull != KeyCode.None && this.menu != KeyCode.None; } }
    }
}