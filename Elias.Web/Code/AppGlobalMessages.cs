using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elias.Web.Code
{
    /// <summary>
    /// A static class with messages used globally in the application
    /// </summary>
    public static class AppGlobalMessages
    {
        public static string UnexpectedErrorMessage
        {
            get
            {
                return "An unexpected error occured. Please try again later.";
            }
        }

        public static string FieldValueMustBeInteger
        {
            get
            {
                return "The value of the field must be an integer.";
            }
        }

        public static string FieldValueMustBeNumber
        {
            get
            {
                return "The value of the field must be a number.";
            }
        }
    }
}