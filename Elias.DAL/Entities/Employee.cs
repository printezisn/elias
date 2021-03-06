﻿using Elias.DAL.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("First Name")]
        [SCIRequired]
        [SCIMaxLength(250)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [SCIRequired]
        [SCIMaxLength(250)]
        public string LastName { get; set; }

        [SCIRequired]
        [SCIEmail]
        [SCIMaxLength(250)]
        public string Email { get; set; }

        [DisplayName("Leave Days")]
        [SCIRequired]
        [SCIMin(0)]
        public int LeaveDays { get; set; }

        [DisplayName("Activation Code")]
        [SCIMaxLength(6)]
        public string ActivationCode { get; set; }

        public DateTime? CodeExpirationDateTime { get; set; }

        [DisplayName("Skype ID")]
        [SCIMaxLength(250)]
        public string SkypeId { get; set; }

        [DisplayName("Facebook ID")]
        [SCIMaxLength(250)]
        public string FacebookId { get; set; }

        [SCIMaxLength(1024)]
        public string ServiceUrl { get; set; }

        [SCIMaxLength(250)]
        public string LastUsedId { get; set; }

        [SCIMaxLength(250)]
        public string BotId { get; set; }

        #region Custom Properties

        [NotMapped]
        [DisplayName("Reserved Days")]
        public int ReservedDays { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

        #endregion

        #region Constructor

        public Employee()
        {
            this.LeaveRequests = new HashSet<LeaveRequest>();
        }

        #endregion
    }
}
