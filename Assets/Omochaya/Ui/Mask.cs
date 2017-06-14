// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mask.cs" company="yoshikazu yananose">
//   (c) 2016 machi no omochaya-san.
// </copyright>
// <summary>
//   The mask.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Omochaya.Ui
{
    using Omochaya.Common;

    /// <summary>The mask.</summary>
    public class Mask : Part
    {
        /// <summary>The lock.</summary>
        private int count = 0;

        /// <summary>The on.</summary>
        public void On()
        {
            this.Enable = true;
            this.count++;
        }

        /// <summary>The off.</summary>
        public void Off()
        {
            this.count--;
        }

        /// <summary>The reset.</summary>
        public void Reset()
        {
            this.count = 0;
        }

        /// <summary>The update.</summary>
        private void Update()
        {
            if (this.count <= 0)
            {
                this.count = 0;
                this.Enable = false;
            }
        }
    }
}