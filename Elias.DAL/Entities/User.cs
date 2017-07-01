using Elias.DAL.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Entities
{
    public partial class User
    {
        [Key]
        public Guid Id { get; set; }

        [SCIRequired]
        [SCIMaxLength(250)]
        public string Username { get; set; }

        [SCIRequired]
        [SCIMaxLength(250)]
        public string Email { get; set; }

        [SCIRequired]
        [SCIMaxLength(500)]
        public string Password { get; set; }

        [SCIRequired]
        [SCIMaxLength(500)]
        public string PasswordSalt { get; set; }

        public Guid? SessionId { get; set; }
    }
}
