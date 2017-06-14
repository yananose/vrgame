// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Part.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The part.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>The part.</summary>
    public class Part : MonoBehaviour
    {
        /// <summary>The enable.</summary>
        public bool Enable
        {
            get
            {
                return this.gameObject.activeSelf;
            }

            set
            {
                this.gameObject.SetActive(value);
            }
        }

        /// <summary>Gets the component.</summary>
        private Dictionary<Type, Part> components = new Dictionary<Type, Part>();

        protected T Get<T>() where T : Part
        {
            // 二度目以降は即返す（ここを早くしたい）
            var type = typeof(T);
            if (this.components.ContainsKey(type))
            {
                return this.components[type] as T;
            }

            // Hierarchy 上に配置していればそれを使う
            var ret = this.GetComponentInChildren<T>(true);
            if(ret == null)
            {
                // なければ GameObject ごと作成
                var child = new GameObject(type.Name, type);
                child.transform.SetParent(this.transform, false);
                ret = child.GetComponent<T>();
            }

            this.components.Add(type, ret);
            return ret;
        }

        //public System.Collections.IEnumerator Get<S>()
        //{
        //    var ret = this.GetComponent<S>();
        //    if (ret == null)
        //    {
        //        ret = this.GetComponentInChildren<S>(true);
        //    }

        //    while (true)
        //    {
        //        yield return ret;
        //    }
        //}
    }

    /// <summary>The part.</summary>
    public class Part<T0> : Part
    {
        /// <summary>The component.</summary>
        private T0 component0;

        /// <summary>Gets the component.</summary>
        protected T0 Component0
        {
            get
            {
                if (this.component0 == null)
                {
                    this.component0 = this.GetComponent<T0>();
                    if(this.component0 == null)
                    {
                        this.component0 = this.GetComponentInChildren<T0>(true);
                    }
                }

                return this.component0;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1> : Part<T0>
    {
        /// <summary>The component.</summary>
        private T1 component1;

        /// <summary>Gets the component.</summary>
        protected T1 Component1
        {
            get
            {
                if (this.component1 == null)
                {
                    this.component1 = this.GetComponent<T1>();
                    if (this.component1 == null)
                    {
                        this.component1 = this.GetComponentInChildren<T1>(true);
                    }
                }

                return this.component1;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2> : Part<T0, T1>
    {
        /// <summary>The component.</summary>
        private T2 component2;

        /// <summary>Gets the component.</summary>
        protected T2 Component2
        {
            get
            {
                if (this.component2 == null)
                {
                    this.component2 = this.GetComponent<T2>();
                    if (this.component2 == null)
                    {
                        this.component2 = this.GetComponentInChildren<T2>(true);
                    }
                }

                return this.component2;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3> : Part<T0, T1, T2>
    {
        /// <summary>The component.</summary>
        private T3 component3;

        /// <summary>Gets the component.</summary>
        protected T3 Component3
        {
            get
            {
                if (this.component3 == null)
                {
                    this.component3 = this.GetComponent<T3>();
                    if (this.component3 == null)
                    {
                        this.component3 = this.GetComponentInChildren<T3>(true);
                    }
                }

                return this.component3;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4> : Part<T0, T1, T2, T3>
    {
        /// <summary>The component.</summary>
        private T4 component4;

        /// <summary>Gets the component.</summary>
        protected T4 Component4
        {
            get
            {
                if (this.component4 == null)
                {
                    this.component4 = this.GetComponent<T4>();
                    if (this.component4 == null)
                    {
                        this.component4 = this.GetComponentInChildren<T4>(true);
                    }
                }

                return this.component4;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4, T5> : Part<T0, T1, T2, T3, T4>
    {
        /// <summary>The component.</summary>
        private T5 component5;

        /// <summary>Gets the component.</summary>
        protected T5 Component5
        {
            get
            {
                if (this.component5 == null)
                {
                    this.component5 = this.GetComponent<T5>();
                    if (this.component5 == null)
                    {
                        this.component5 = this.GetComponentInChildren<T5>(true);
                    }
                }

                return this.component5;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4, T5, T6> : Part<T0, T1, T2, T3, T4, T5>
    {
        /// <summary>The component.</summary>
        private T6 component6;

        /// <summary>Gets the component.</summary>
        protected T6 Component6
        {
            get
            {
                if (this.component6 == null)
                {
                    this.component6 = this.GetComponent<T6>();
                    if (this.component6 == null)
                    {
                        this.component6 = this.GetComponentInChildren<T6>(true);
                    }
                }

                return this.component6;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4, T5, T6, T7> : Part<T0, T1, T2, T3, T4, T5, T6>
    {
        /// <summary>The component.</summary>
        private T7 component7;

        /// <summary>Gets the component.</summary>
        protected T7 Component7
        {
            get
            {
                if (this.component7 == null)
                {
                    this.component7 = this.GetComponent<T7>();
                    if (this.component7 == null)
                    {
                        this.component7 = this.GetComponentInChildren<T7>(true);
                    }
                }

                return this.component7;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4, T5, T6, T7, T8> : Part<T0, T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>The component.</summary>
        private T8 component8;

        /// <summary>Gets the component.</summary>
        protected T8 Component8
        {
            get
            {
                if (this.component8 == null)
                {
                    this.component8 = this.GetComponent<T8>();
                    if (this.component8 == null)
                    {
                        this.component8 = this.GetComponentInChildren<T8>(true);
                    }
                }

                return this.component8;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : Part<T0, T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>The component.</summary>
        private T9 component9;

        /// <summary>Gets the component.</summary>
        protected T9 Component9
        {
            get
            {
                if (this.component9 == null)
                {
                    this.component9 = this.GetComponent<T9>();
                    if (this.component9 == null)
                    {
                        this.component9 = this.GetComponentInChildren<T9>(true);
                    }
                }

                return this.component9;
            }
        }
    }

    /// <summary>The part.</summary>
    public class Part<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Part<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>The component.</summary>
        private T10 component10;

        /// <summary>Gets the component.</summary>
        protected T10 Component10
        {
            get
            {
                if (this.component10 == null)
                {
                    this.component10 = this.GetComponent<T10>();
                    if (this.component10 == null)
                    {
                        this.component10 = this.GetComponentInChildren<T10>(true);
                    }
                }

                return this.component10;
            }
        }
    }
}