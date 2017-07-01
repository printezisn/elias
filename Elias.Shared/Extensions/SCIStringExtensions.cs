using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared.Extensions
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static class SCIStringExtensions
    {
        /// <summary>
        /// Trims the ending part of a string and adds a tail. This is done only if the string exceeds a maximum length
        /// </summary>
        /// <param name="item">The string to trim</param>
        /// <param name="maxLength">The maximum length for the string</param>
        /// <param name="tail">(Optional) The ending part of the string if it exists the maximum length</param>
        /// <returns>The trimmed string</returns>
        public static string TrimLength(this string item, int maxLength, string tail = "...")
        {
            if (item == null)
            {
                return null;
            }
            if (item.Length <= maxLength)
            {
                return item;
            }

            return item.Substring(0, maxLength - tail.Length) + tail;
        }

        /// <summary>
        /// Converts a string to lower case but checks if it is null
        /// </summary>
        /// <param name="item">The string to convert</param>
        /// <returns>The converted string</returns>
        public static string ToLowerOrNull(this string item)
        {
            if (item == null)
            {
                return null;
            }

            return item.ToLower();
        }

        /// <summary>
        /// Sanitizes a file name by removing invalid characters
        /// </summary>
        /// <param name="fileName">The file name to sanitize</param>
        /// <returns>The sanitized file name</returns>
        public static string SanitizedFileName(this string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char ch in invalidChars)
            {
                fileName = fileName.Replace(ch, '-');
            }

            return fileName;
        }

        /// <summary>
        /// Sanitizes a file path by removing invalid characters
        /// </summary>
        /// <param name="filePath">The file path to sanitize</param>
        /// <returns>The sanitized file path</returns>
        public static string SanitizedFilePath(this string filePath)
        {
            char[] invalidChars = Path.GetInvalidPathChars();
            foreach (char ch in invalidChars)
            {
                filePath = filePath.Replace(ch, '-');
            }

            return filePath;
        }

        /// <summary>
        /// Converts a string to integer
        /// </summary>
        /// <param name="item">The string item to convert</param>
        /// <param name="defaultValue">The default value to return if the conversion is not successful</param>
        /// <returns>The converted value</returns>
        public static int ToInt(this string item, int defaultValue = 0)
        {
            int value;
            if (!int.TryParse(item, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Converts a string to double
        /// </summary>
        /// <param name="item">The string item to convert</param>
        /// <param name="defaultValue">The default value to return if the conversion is not successful</param>
        /// <returns>The converted value</returns>
        public static double ToDouble(this string item, double defaultValue = 0)
        {
            double value;
            if (!double.TryParse(item, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Converts a string to decimal
        /// </summary>
        /// <param name="item">The string item to convert</param>
        /// <param name="defaultValue">The default value to return if the conversion is not successful</param>
        /// <returns>The converted value</returns>
        public static decimal ToDecimal(this string item, decimal defaultValue = 0)
        {
            decimal value;
            if (!decimal.TryParse(item, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Converts a string to float
        /// </summary>
        /// <param name="item">The string item to convert</param>
        /// <param name="defaultValue">The default value to return if the conversion is not successful</param>
        /// <returns>The converted value</returns>
        public static float ToFloat(this string item, float defaultValue = 0)
        {
            float value;
            if (!float.TryParse(item, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Converts a string to bool
        /// </summary>
        /// <param name="item">The string item to convert</param>
        /// <param name="defaultValue">The default value to return if the conversion is not successful</param>
        /// <returns>The converted value</returns>
        public static bool ToBool(this string item, bool defaultValue = false)
        {
            bool value;
            if (!bool.TryParse(item, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Checks if a string is an integer
        /// </summary>
        /// <param name="item">The string item to check</param>
        /// <returns>True if the string is an integer, otherwise false</returns>
        public static bool IsInt(this string item)
        {
            int value;

            return int.TryParse(item, out value);
        }

        /// <summary>
        /// Checks if a string is a float
        /// </summary>
        /// <param name="item">The string item to check</param>
        /// <returns>True if the string is a float, otherwise false</returns>
        public static bool IsFloat(this string item)
        {
            float value;

            return float.TryParse(item, out value);
        }

        /// <summary>
        /// Checks if a string is a double
        /// </summary>
        /// <param name="item">The string item to check</param>
        /// <returns>True if the string is a double, otherwise false</returns>
        public static bool IsDouble(this string item)
        {
            double value;

            return double.TryParse(item, out value);
        }

        /// <summary>
        /// Checks if a string is a decimal
        /// </summary>
        /// <param name="item">The string item to check</param>
        /// <returns>True if the string is a decimal, otherwise false</returns>
        public static bool IsDecimal(this string item)
        {
            decimal value;

            return decimal.TryParse(item, out value);
        }

        /// <summary>
        /// Checks if a string is a boolean
        /// </summary>
        /// <param name="item">The string item to check</param>
        /// <returns>True if the string is a boolean, otherwise false</returns>
        public static bool IsBool(this string item)
        {
            bool value;

            return bool.TryParse(item, out value);
        }
    }
}
