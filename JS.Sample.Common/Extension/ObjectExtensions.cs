using System;
using System.Collections.Generic;
using System.Text;

namespace JS.Sample.Common.Extension
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// Converts the object to Long.
        /// </summary>
        /// <param name="input">Object value</param>
        /// <returns>
        /// Retruns long value
        /// </returns>
        public static long ToLong(this object input)
        {

            long output = 0;

            if ((input == null) || (input == DBNull.Value))
                return output;
            try
            {
                if (string.Empty != input.ToString())
                    output = Convert.ToInt64(input);
            }
            catch { return output; }

            return output;
        }

        /// <summary>
        /// Converts the object to Double.
        /// </summary>
        /// <param name="input">Object value</param>
        /// <returns>
        /// Retruns double value
        /// </returns>
        public static double ToDouble(this object input)
        {
            double output = 0;

            if ((input == null) || (input == DBNull.Value))
                return output;
            try
            {
                if (string.Empty != input.ToString())
                    output = Convert.ToDouble(input);
            }
            catch { return output; }

            return output;
        }

        /// <summary>
        /// Converts the object to int.
        /// </summary>
        /// <param name="input">Object value</param>
        /// <returns>
        /// Retruns int value
        /// </returns>
        public static int ToInt(this object input)
        {
            var output = 0;

            if ((input == null) || (input == DBNull.Value))
                return output;
            try
            {
                if (string.Empty != input.ToString())
                    output = Convert.ToInt32(input);
            }
            catch { return output; }

            return output;
        }



    }
}
