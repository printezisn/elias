using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Web.Code
{
    /// <summary>
    /// Helper class used for managing DateTime objects
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Converts a DateTime object based on the local timezone
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The converted DateTime object</returns>
        public static DateTime? GetLocalDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return GetLocalDateTime(dateTime.Value);
        }

        /// <summary>
        /// Converts a DateTime object based on the local timezone
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The converted DateTime object</returns>
        public static DateTime GetLocalDateTime(DateTime dateTime)
        {
            return dateTime.ToLocalTime();
        }

        /// <summary>
        /// Renders the month and year part of a date object and takes into account the timezone of the application
        /// </summary>
        /// <param name="dateTime">The date object</param>
        /// <returns>The generated date string</returns>
        public static string RenderMonth(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return RenderMonth(dateTime.Value);
        }

        /// <summary>
        /// Renders the month and year part of a date object and takes into account the timezone of the application
        /// </summary>
        /// <param name="dateTime">The date object</param>
        /// <returns>The generated date string</returns>
        public static string RenderMonth(DateTime dateTime)
        {
            return dateTime.ToString("MM.yyyy");
        }

        /// <summary>
        /// Renders a date object and takes into account the timezone of the application
        /// </summary>
        /// <param name="dateTime">The date object to render</param>
        /// <param name="convertToLocal">Indicates if the date object must be converted based on the local timezone</param>
        /// <returns>The generated date string</returns>
        public static string RenderDate(DateTime? dateTime, bool convertToLocal = false)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return RenderDate(dateTime.Value, convertToLocal);
        }

        /// <summary>
        /// Renders a date object and takes into account the timezone of the application
        /// </summary>
        /// <param name="dateTime">The date object to render</param>
        /// <param name="convertToLocal">Indicates if the date object must be converted based on the local timezone</param>
        /// <returns>The generated date string</returns>
        public static string RenderDate(DateTime dateTime, bool convertToLocal = false)
        {
            if (convertToLocal)
            {
                dateTime = GetLocalDateTime(dateTime);
            }

            return dateTime.ToString("d");
        }

        /// <summary>
        /// Renders a datetime object and takes into account the timezone of the application
        /// </summary>
        /// <param name="dateTime">The datetime object to render</param>
        /// <returns>The generated datetime string</returns>
        public static string RenderDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return RenderDateTime(dateTime.Value);
        }

        /// <summary>
        /// Renders a datetime object and takes into account the timezone of the application
        /// </summary>
        /// <param name="dateTime">The datetime object to render</param>
        /// <returns>The generated datetime string</returns>
        public static string RenderDateTime(DateTime dateTime)
        {
            return GetLocalDateTime(dateTime).ToString("g");
        }
    }
}
