using Elias.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elias.Web.Models
{
    /// <summary>
    /// The model class used for toastr messages in the front end
    /// </summary>
    public class ToastrMessage
    {
        #region Properties

        public string Title { get; set; }
        public string Message { get; set; }
        public ToastrMessageTypeEnum Type { get; set; }

        public bool IsSuccess
        {
            get
            {
                return this.Type == ToastrMessageTypeEnum.Success;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// The constructor
        /// </summary>
        public ToastrMessage()
        {

        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="title">(Optional) The title of the message</param>
        /// <param name="message">The message text</param>
        /// <param name="type">The type of the message</param>
        public ToastrMessage(string title, string message, ToastrMessageTypeEnum type)
        {
            this.Title = title;
            this.Message = message;
            this.Type = type;
        }

        #endregion
    }
}