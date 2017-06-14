// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Popup.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The popup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Ui
{
    using System.Collections.Generic;
    using Omochaya.Audio;
    using Omochaya.Common;
    using Omochaya.Debug;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    /// <summary>The popup.</summary>
    public class Popup : Part
    {
        /// <summary>The disabled text color.</summary>
        public static readonly Color DisabledTextColor = new Color(0.85f, 0.85f, 0.85f);

        /// <summary>The enabled text color.</summary>
        public static readonly Color EnabledTextColor = new Color(0.2f, 0.2f, 0.2f);

        /// <summary>The current.</summary>
        private static Popup Current = null;

        /// <summary>The window.</summary>
        [SerializeField]
        protected Transform window = null;

        /// <summary>The se open.</summary>
        [SerializeField]
        protected AudioClip seOpen = null;

        /// <summary>The se cancel.</summary>
        [SerializeField]
        protected AudioClip seCancel = null;

        /// <summary>The se select.</summary>
        [SerializeField]
        protected AudioClip seSelect = null;

        /// <summary>The request se open.</summary>
        private bool requestSeOpen = false;

        /// <summary>The selectables.</summary>
        private List<Selectable> selectables = null;

        /// <summary>The selected.</summary>
        private Selectable selected = null;

        /// <summary>The inputed.</summary>
        private bool inputed = false;

        /// <summary>The previous.</summary>
        private Popup previous = null;

        /// <summary>The first.</summary>
        private Selectable first = null;

        /// <summary>The back.</summary>
        protected Image back = null;

        /// <summary>The tween.</summary>
        protected Tween tween = new Tween();

        /// <summary>Gets the is open any.</summary>
        public static bool IsOpenAny { get { return Popup.Current != null; } }

        /// <summary>Gets the is open.</summary>
        public bool IsOpen { get { return this.Enable && !this.tween.IsReverse; } }

        /// <summary>Gets the is close.</summary>
        public bool IsClose { get { return !this.IsOpen; } }

        /// <summary>The enable button.</summary>
        public static void EnableButton(Button button, bool isEnable)
        {
            if (button != null && button.interactable != isEnable)
            {
                button.interactable = isEnable;
                var color = isEnable ? Popup.EnabledTextColor : Popup.DisabledTextColor;
                foreach (var text in button.GetComponentsInChildren<Text>())
                {
                    text.color = color;
                }
            }
        }

        /// <summary>The open.</summary>
        public void Open()
        {
            if (this.IsClose)
            {
                if (this.tween.IsBusy)
                {
                    this.tween.Reverse();
                }
                else
                {
                    this.Initialize();
                    this.Enable = true;
                    this.StartTween(false);
                }

                this.requestSeOpen = true;
            }
        }

        /// <summary>The open.</summary>
        public void Close()
        {
            this.requestSeOpen = false;
            if (this.IsOpen)
            {
                if (this.tween.IsBusy)
                {
                    this.tween.Reverse();
                }
                else
                {
                    this.StartTween(true);
                }
            }
        }

        /// <summary>The start tween.</summary>
        protected void StartTween(bool isClose)
        {
            var time = 0.15f;
            var easeType = Tween.Ease.Out;
            if (isClose)
            {
                time = -time;
                easeType = Tween.Ease.In;
            }

            this.tween.Start(time, this.ChangeTween, null, easeType, 2);
//            Game.Ui.Mask.Get.On();
        }

        /// <summary>The start tween.</summary>
        protected bool Animation()
        {
            if (this.requestSeOpen)
            {
                this.requestSeOpen = false;
                this.ChangeCurrent(true);
                if (AudioPlayer.Current != null && this.seOpen != null && (AudioPlayer.Current.LastTime + 0.05f) < Time.realtimeSinceStartup)
                {
                    AudioPlayer.Current.PlaySe(this.seOpen, 0.5f);
                }
            }

            var ret = false;
            if (this.tween.IsBusy)
            {
                if (this.tween.Calc())
                {
                    ret = true;
                }
                else
                {
//                    Game.Ui.Mask.Get.Off();
                    if (this.tween.IsReverse)
                    {
                        ret = true;
                        this.Enable = false;
                        this.ChangeCurrent(false);
                    }
                }
            }

            // 操作
            if (!ret && Joypad.Current.IsEnabled)
            {
                this.InputKey(Joypad.Current.Direction, Joypad.Current.onShot, Joypad.Current.onMenu);
            }

            return ret;
        }

        /// <summary>The setup selectable.</summary>
        protected void SetupSelectable()
        {
            this.first = null;
            this.selectables = new List<Selectable>();
            var selectedPosition = new Vector2(2048f, -2048f);
            var back = this.back == null ? null : this.back.gameObject;
            foreach (var item in this.GetComponentsInChildren<Selectable>(false))
            {
                if (item.gameObject != back)
                {
                    this.selectables.Add(item);
                    var rectTransform = item.transform as RectTransform;
                    var itemPosition = (Vector2)rectTransform.position;
                    if (this.first == null ||
                        selectedPosition.y < itemPosition.y ||
                        (itemPosition.y == selectedPosition.y && itemPosition.x < selectedPosition.x))
                    {
                        this.first = item;
                        selectedPosition = itemPosition;
                    }
                }
            }

            this.selected = this.first;
            if (this.selected != null && Joypad.Current.IsEnabled)
            {
                EventSystem.current.SetSelectedGameObject(this.selected.gameObject);
            }
        }

        /// <summary>The input key.</summary>
        protected void InputKey(Vector2 direction, bool isDecide, bool isCancel)
        {
            if (this.selected == null || Popup.Current != this)
            {
                return;
            }

            var cand = this.selected;
            var limitDistance = 256f;
            var maxScore = 1f / limitDistance;
            var position = RectTransformUtility.WorldToScreenPoint(Camera.current, this.selected.transform.position);
            foreach (var item in this.selectables)
            {
                if (item != this.selected)
                {
                    var delta = RectTransformUtility.WorldToScreenPoint(Camera.current, item.transform.position) - position;
                    var score = Vector2.Dot(delta, direction) / delta.sqrMagnitude;
                    if (maxScore < score)
                    {
                        maxScore = score;
                        cand = item;
                    }
                }
            }

            if (this.selected != cand)
            {
                if (!this.inputed)
                {
                    this.inputed = true;
                    this.selected = cand;
                    EventSystem.current.SetSelectedGameObject(this.selected.gameObject);
                    if (AudioPlayer.Current != null && this.seSelect != null)
                    {
                        AudioPlayer.Current.PlaySe(this.seSelect, 0.5f);
                    }

                    DebugLog.Put(cand.name + "(" + maxScore + ")");
                }
            }
            else
            {
                this.inputed = false;
                var slider = cand as Slider;
                if (slider != null)
                {
                    var add = 0f;
                    switch (slider.direction)
                    {
                        case Slider.Direction.LeftToRight:
                            add = direction.x;
                            break;
                        case Slider.Direction.TopToBottom:
                            add = direction.y;
                            break;
                        case Slider.Direction.RightToLeft:
                            add = -direction.x;
                            break;
                        case Slider.Direction.BottomToTop:
                            add = -direction.y;
                            break;
                    }

                    slider.normalizedValue += add * Time.deltaTime / 2.0f;
                }
            }

            if (this.selected != null)
            {
                if (isDecide)
                {
                    var button = this.selected.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke();
                    }
                }
            }

            if (isCancel)
            {
                var back = this.back.GetComponent<Button>();
                if (back != null)
                {
                    back.onClick.Invoke();
                }
            }
        }

        /// <summary>The change current.</summary>
        private void ChangeCurrent(bool isActive)
        {
            if (isActive)
            {
                var selected = this.selected;
                this.SetupSelectable();
                this.selected = this.selectables.Contains(selected) ? selected : this.first;
                this.previous = Popup.Current;
                Popup.Current = this;
                if (this.selected != null && Joypad.Current.IsEnabled)
                {
                    EventSystem.current.SetSelectedGameObject(this.selected.gameObject);
                }
            }
            else if (Popup.Current == this)
            {
                Popup.Current = this.previous;
                this.previous = null;
                if (Popup.Current != null && Popup.Current.selected != null && Joypad.Current.IsEnabled)
                {
                    EventSystem.current.SetSelectedGameObject(Popup.Current.selected.gameObject);
                }
            }
        }

        /// <summary>The initialize.</summary>
        private void Initialize()
        {
            if (this.back == null)
            {
                var gameObject = new GameObject("back", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
                var transform = gameObject.transform as RectTransform;
                transform.SetParent(this.transform, false);
                transform.SetAsFirstSibling();
                transform.anchorMin = -Vector2.one;
                transform.anchorMax = Vector2.one * 2f;
                transform.anchoredPosition = Vector2.zero;
                transform.sizeDelta = Vector2.zero;
                transform.pivot = Vector2.one * 0.5f;
                this.back = gameObject.GetComponent<Image>();
                this.back.color = Color.black;
                this.back.material = null;// Game.MaterialVrUi;
                var button = gameObject.GetComponent<Button>();
                button.transition = Selectable.Transition.None;
                button.onClick.AddListener(this.Close);
                if (AudioPlayer.Current != null && this.seCancel != null)
                {
                    button.onClick.AddListener(this.PlaySeCancel);
                }
            }
        }

        /// <summary>The change tween.</summary>
        private void ChangeTween(float rate)
        {
            // スケール
            var scale = this.window.localScale;
            scale.x = scale.y = rate;
            this.window.localScale = scale;

            // 背景の暗さ
            var color = this.back.color;
            color.a = rate * 0.4f;
            this.back.color = color;
        }

        /// <summary>The play se cancel.</summary>
        private void PlaySeCancel()
        {
            AudioPlayer.Current.PlaySe(this.seCancel, 0.5f);
        }

        /// <summary>The start.</summary>
        private void Start()
        {
            this.SetupSelectable();
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            // アニメーション
            if (this.Animation())
            {
                return;
            }
        }
    }
}