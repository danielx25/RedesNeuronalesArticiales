using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RedesNeuronalesArtificiales.RNA
{
    class NumeroRandom : RandomNumberGenerator
    {
        private static RandomNumberGenerator r;

        public NumeroRandom()
        {
            r = RandomNumberGenerator.Create();
        }

        public override void GetBytes(byte[] buffer)
        {
            r.GetBytes(buffer);
        }

        public double NextDouble()
        {
            byte[] b = new byte[4];
            r.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }

        public double NextDouble(double min, double max)
        {
            byte[] b = new byte[4];
            r.GetBytes(b);
            double number = (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
            return min + (number * (max - min));
        }

        public int Next(int minValue, int maxValue)
        {
            //return (int)Math.Round(NextDouble() * (maxValue – minValue – 1)) + minValue;
            return (int)Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue;
        }


        ///<summary>
        /// Returns a nonnegative random number.
        ///</summary>
        public int Next()
        {
            return Next(0, Int32.MaxValue);
        }

        ///<summary>
        /// Returns a nonnegative random number less than the specified maximum
        ///</summary>
        ///<param name=”maxValue”>The inclusive upper bound of the random number returned. maxValue must be greater than or equal 0</param>
        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }
    }
}
