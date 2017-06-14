// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Information.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Ui
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>The information.</summary>
    public class Information : Popup
    {
        /// <summary>The text.</summary>
        [SerializeField]
        private Text text = null;

        /// <summary>The yes.</summary>
        [SerializeField]
        private Button yes = null;

        /// <summary>The no.</summary>
        [SerializeField]
        private Button no = null;

        /// <summary>The ok.</summary>
        [SerializeField]
        private Button ok = null;

        /// <summary>The note.</summary>
        [SerializeField]
        private RectTransform note = null;

        /// <summary>The agree.</summary>
        private Action agree = null;

        /// <summary>The is yes.</summary>
        private bool isYes = false;

        /// <summary>The is ok end.</summary>
        public static bool IsOkEnd { get; set; }

        /// <summary>The is ok.</summary>
        public bool IsOk { get; set; }

        /// <summary>The ok utton.</summary>
        public Button OkButton { get { return this.ok; } }

        /// <summary>The open.</summary>
        public void Open(string message, Action agree = null)
        {
            base.Open();
            this.text.text = message;
            if (agree != null)
            {
                this.agree = agree;
                this.yes.gameObject.SetActive(true);
                this.no.gameObject.SetActive(true);
                this.ok.gameObject.SetActive(false);
            }
            else
            {
                this.agree = null;
                this.yes.gameObject.SetActive(false);
                this.no.gameObject.SetActive(false);
                this.ok.gameObject.SetActive(true);
            }

            this.isYes = false;
            this.IsOk = false;
            Information.IsOkEnd = false;
            Popup.EnableButton(this.ok, true);

            var note = this.note.sizeDelta;
            note.y = -100;
            this.note.sizeDelta = note;
        }

        /// <summary>The open no button.</summary>
        public void OpenNoButton(string message)
        {
            base.Open();
            this.text.text = message;
            this.isYes = false;
            this.IsOk = false;
            Information.IsOkEnd = false;
            this.yes.gameObject.SetActive(false);
            this.no.gameObject.SetActive(false);
            this.ok.gameObject.SetActive(false);

            var note = this.note.sizeDelta;
            note.y = -60;
            this.note.sizeDelta = note;
        }

        /// <summary>The yes.</summary>
        public void Yes()
        {
            base.Close();
            this.isYes = true;
        }

        /// <summary>The ok.</summary>
        public void Ok()
        {
            base.Close();
            this.IsOk = true;
            Information.IsOkEnd = true;
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            if (this.Animation())
            {
                // 同意した時の処理は閉じるアニメが終了してから。
                if (!this.Enable)
                {
                    this.IsOk = false;
                    if (this.isYes)
                    {
                        var agree = this.agree;
                        this.agree = null;
                        this.isYes = false;
                        if (agree != null)
                        {
                            agree();
                        }
                    }
                }

                return;
            }
        }
    }
}