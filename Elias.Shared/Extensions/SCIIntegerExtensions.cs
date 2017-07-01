using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared.Extensions
{
    /// <summary>
    /// Extension methods for integers
    /// </summary>
    public static class SCIIntegerExtensions
    {
        /// <summary>
        /// Converts an integer to a size string (10MB, 40KB, etc.)
        /// </summary>
        /// <param name="num">The integer to convert</param>
        /// <returns>The size string</returns>
        public static string ToSizeString(this int num)
        {
            double result = num;
            string resultLabel = "B";

            if (num >= Math.Pow(1024, 3))
            {
                result /= Math.Pow(1024, 3);
                resultLabel = "GB";
            }
            else if (num >= Math.Pow(1024, 2))
            {
                result /= Math.Pow(1024, 2);
                resultLabel = "MB";
            }
            else if (num >= 1024)
            {
                result /= 1024;
                resultLabel = "KB";
            }

            return result.ToString("0.##") + resultLabel;
        }
    }
}
