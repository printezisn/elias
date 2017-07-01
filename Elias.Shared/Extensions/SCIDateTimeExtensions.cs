using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared.Extensions
{
    /// <summary>
    /// Extension methods for DateTime objects
    /// </summary>
    public static class SCIDateTimeExtensions
    {
        /// <summary>
        /// Applies timezone difference to a DateTime object
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <param name="timezoneInfo">The timezone to apply</param>
        /// <returns>A new DateTime object with the timezone difference applied</returns>
        public static DateTime ApplyTimezone(this DateTime dateTime, TimeZoneInfo timezoneInfo)
        {
            return dateTime.Add(timezoneInfo.GetUtcOffset(dateTime));
        }

        /// <summary>
        /// Returns the month start for a specific DateTime object
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The month start DateTime object</returns>
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Returns the month end for a specific DateTime object
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The month end DateTime object</returns>
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Returns the previous month start for a specific DateTime object
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The previous month start DateTime object</returns>
        public static DateTime StartOfPreviousMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(-1).StartOfMonth();
        }

        /// <summary>
        /// Returns the previous month end for a specific DateTime object
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The previous month end DateTime object</returns>
        public static DateTime EndOfPreviousMonth(this DateTime dateTime)
        {
            return dateTime.AddMonths(-1).EndOfMonth();
        }

        /// <summary>
        /// Returns the difference in months between two DateTime objects
        /// </summary>
        /// <param name="dateTime">The first DateTime object</param>
        /// <param name="comparedDateTime">The second DateTime object to be compared</param>
        /// <returns>The difference in months</returns>
        public static int MonthDifference(this DateTime dateTime, DateTime comparedDateTime)
        {
            return (dateTime.Month - comparedDateTime.Month) + 12 * (dateTime.Year - comparedDateTime.Year);
        }

        /// <summary>
        /// Returns the minimum value between two DateTime object
        /// </summary>
        /// <param name="dateTime">The first DateTime object</param>
        /// <param name="comparedDateTime">The second DateTime object to be compared</param>
        /// <returns>The minimum value</returns>
        public static DateTime Min(this DateTime dateTime, DateTime comparedDateTime)
        {
            if (dateTime < comparedDateTime)
            {
                return dateTime;
            }

            return comparedDateTime;
        }

        /// <summary>
        /// Returns the maximum value between two DateTime object
        /// </summary>
        /// <param name="dateTime">The first DateTime object</param>
        /// <param name="comparedDateTime">The second DateTime object to be compared</param>
        /// <returns>The maximum value</returns>
        public static DateTime Max(this DateTime dateTime, DateTime comparedDateTime)
        {
            if (dateTime > comparedDateTime)
            {
                return dateTime;
            }

            return comparedDateTime;
        }

        /// <summary>
        /// Returns the year's quarter (3months period) of a given date
        /// </summary>
        /// <param name="dateTime">The DateTime object</param>
        /// <returns>The number of quarter 1,2,3,4</returns>
        public static byte GetYearQuarter(this DateTime dateTime)
        {
            return Convert.ToByte((dateTime.Month - 1) / 3 + 1); 
        }
    }
}
