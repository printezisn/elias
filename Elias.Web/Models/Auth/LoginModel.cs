using Elias.DAL.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Elias.Web.Models.Auth
{
    /// <summary>
    /// The model class used in the login page
    /// </summary>
    public class LoginModel
    {
        [SCIRequired]
        public string Username { get; set; }

        [SCIRequired]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}