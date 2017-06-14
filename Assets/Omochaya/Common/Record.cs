// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Record.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The record.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Common
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The record.
    /// </summary>
    public abstract class Record
    {
        /// <summary>The ascii s.</summary>
        private const char AsciiS = ' ';

        /// <summary>The ascii e.</summary>
        private const char AsciiE = '~';

        /// <summary>The ascii n.</summary>
        private const int AsciiN = Record.AsciiE - Record.AsciiS;

        /// <summary>The true.</summary>
        private const string True = "1";

        /// <summary>The false.</summary>
        private const string False = "0";

        /// <summary>The is exist.</summary>
        public bool IsExist { get; private set; }

        /// <summary>The language.</summary>
        public static class Language
        {
            /// <summary>The japanese.</summary>
            public const string Japanese = "ja";

            /// <summary>The english.</summary>
            public const string English = "en";
        }

        /// <summary>The from.</summary>
        public static string From(string prm)
        {
            return prm;
        }

        /// <summary>The to string.</summary>
        public static string ToString(string prm)
        {
            return prm;
        }

        /// <summary>The from.</summary>
        public static string From(int prm)
        {
            return prm.ToString();
        }

        /// <summary>The to int.</summary>
        public static int ToInt(string prm)
        {
            var ret = 0;
            int.TryParse(prm, out ret);
            return ret;
        }

        /// <summary>The from.</summary>
        public static string From(float prm)
        {
            return prm.ToString();
        }

        /// <summary>The to float.</summary>
        public static float ToFloat(string prm)
        {
            var ret = 0f;
            float.TryParse(prm, out ret);
            return ret;
        }

        /// <summary>The from.</summary>
        public static string From(bool prm)
        {
            return prm ? Record.True : Record.False;
        }

        /// <summary>The to bool.</summary>
        public static bool ToBool(string prm)
        {
            return prm == Record.True;
        }

        /// <summary>The from.</summary>
        public static string From<T>(T prm) where T : struct
        {
            var tmp = (int)Enum.ToObject(typeof(T), prm);
            return tmp.ToString();
        }

        /// <summary>The to.</summary>
        public static T To<T>(string prm)
        {
            return (T)Enum.ToObject(typeof(T), Record.ToInt(prm));
        }

        /// <summary>The archive.</summary>
        public static string Archive(params string[] prms)
        {
            var ret = string.Empty;
            foreach (var prm in prms)
            {
                var n = prm.Length;
                ret += (char)(Record.AsciiS + n % Record.AsciiN);
                n = n / Record.AsciiN;
                ret += (char)(Record.AsciiS + n % Record.AsciiN);
                n = n / Record.AsciiN;
                ret += (char)(Record.AsciiS + n % Record.AsciiN);
                ret += prm;
            }

            return ret;
        }

        /// <summary>The split.</summary>
        public static string[] Split(string data)
        {
            var empty = new string[0];
            if (data == null)
            {
                return empty;
            }

            var ret = new List<string>();
            var limit = data.Length;
            var ofs = 0;
            while (ofs < limit)
            {
                var n = 0;
                var keta = 1;
                if (limit < ofs + 3)
                {
                    return empty;
                }

                n += (data[ofs++] - Record.AsciiS) * keta;
                keta *= Record.AsciiN;
                n += (data[ofs++] - Record.AsciiS) * keta;
                keta *= Record.AsciiN;
                n += (data[ofs++] - Record.AsciiS) * keta;
                if (limit < ofs + n)
                {
                    return empty;
                }

                ret.Add(data.Substring(ofs, n));
                ofs += n;
            }

            return ret.ToArray();
        }

        /// <summary>The get key.</summary>
        public abstract string GetKey();

        /// <summary>The initialize.</summary>
        public abstract void Initialize();

        /// <summary>The deserialize.</summary>
        public abstract void Deserialize(string code);

        /// <summary>The serialize.</summary>
        public abstract string Serialize();

        /// <summary>The save.</summary>
        public void Save()
        {
            var code = this.Serialize();
            if (!string.IsNullOrEmpty(code))
            {
                PlayerPrefs.SetString(GetKey(), code);
                PlayerPrefs.Save();
                this.IsExist = true;
            }
            else
            {
                this.Delete();
            }
        }

        /// <summary>The load.</summary>
        public void Load()
        {
            var code = PlayerPrefs.GetString(GetKey());
            if (!string.IsNullOrEmpty(code))
            {
                this.Deserialize(code);
                this.IsExist = true;
            }
            else
            {
                this.Initialize();
                this.IsExist = false;
            }
        }

        /// <summary>The delete.</summary>
        public void Delete()
        {
            this.Initialize();
            PlayerPrefs.DeleteKey(GetKey());
            PlayerPrefs.Save();
            this.IsExist = false;
        }
    }
}