// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Operation.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>The operation.</summary>
    public class Operation
    {
        /// <summary>The items.</summary>
        private List<Item> items = new List<Item>();

        /// <summary>The item.</summary>
        private Item item;

        /// <summary>The add.</summary>
        public void Add(Action on)
        {
            this.items.Add(new Item(on));
        }

        /// <summary>The add.</summary>
        public void Add(Action begin, Action on, Action end)
        {
            this.items.Add(new Item(begin, on, end));
        }

        /// <summary>The add.</summary>
        public void Add(Func<bool> func, Action on)
        {
            this.items.Add(new Item(func, on));
        }

        /// <summary>The add.</summary>
        public void Add(Func<bool> func, Action begin, Action on, Action end)
        {
            this.items.Add(new Item(func, begin, on, end));
        }

        /// <summary>The clear.</summary>
        public void Clear()
        {
            this.item = null;
            this.items.Clear();
        }

        /// <summary>The update.</summary>
        public void Update(bool on)
        {
            if (this.item != null && !this.item.Update(on))
            {
                this.item = null;
            }

            if (this.item == null && on)
            {
                foreach (var item in this.items)
                {
                    if (item.Select())
                    {
                        this.item = item;
                        break;
                    }
                }
            }
        }

        /// <summary>The update.</summary>
        public void Execute()
        {
            if (this.item != null)
            {
                this.item.Execute();
            }
        }

        /// <summary>The item.</summary>
        private class Item
        {

            /// <summary>The func.</summary>
            private Func<bool> func;

            /// <summary>The begin.</summary>
            private Action begin;

            /// <summary>The on.</summary>
            private Action on;

            /// <summary>The end.</summary>
            private Action end;

            /// <summary>The active.</summary>
            private Action active;

            /// <summary>The constructor.</summary>
            internal Item(Action on)
            {
                this.begin = this.on = on;
            }

            /// <summary>The constructor.</summary>
            internal Item(Action begin, Action on, Action end)
            {
                this.begin = begin;
                this.on = on;
                this.end = end;
            }

            /// <summary>The constructor.</summary>
            internal Item(Func<bool> func, Action on)
            {
                this.func = func;
                this.begin = this.on = on;
            }

            /// <summary>The constructor.</summary>
            internal Item(Func<bool> func, Action begin, Action on, Action end)
            {
                this.func = func;
                this.begin = begin;
                this.on = on;
                this.end = end;
            }

            /// <summary>The update.</summary>
            internal bool Update(bool on)
            {
                if (!on)
                {
                    if(this.active == this.on)
                    {
                        this.active = this.end;
                    }
                    else if (this.active == this.begin)
                    {
                        // まだ実行してない
                        this.active = null;
                    }
                }

                return this.active != null;
            }

            /// <summary>The select.</summary>
            internal bool Select()
            {
                this.active = null;
                if (this.func == null || this.func())
                {
                    this.active = this.begin;
                }

                return this.active != null;
            }

            /// <summary>The execute.</summary>
            internal void Execute()
            {
                if (this.active != null)
                {
                    this.active();
                    if (this.active == this.end)
                    {
                        this.active = null;
                    }
                    else if (this.active != this.on)
                    {
                        this.active = this.on;
                    }
                }
            }
        }
    }
}