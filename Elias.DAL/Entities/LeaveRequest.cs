using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Entities
{
    public partial class LeaveRequest
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Employee")]
        public int EmployeeId { get; set; }

        [DisplayName("From Date")]
        public DateTime FromDate { get; set; }

        [DisplayName("To Date")]
        public DateTime ToDate { get; set; }

        [DisplayName("Total Days")]
        public int TotalDays { get; set; }

        [DisplayName("Status")]
        public byte StatusId { get; set; }

        #region Navigation Properties

        [JsonIgnore]
        public virtual Employee Employee { get; set; }

        [JsonIgnore]
        public virtual LeaveRequestStatus Status { get; set; }

        #endregion

        #region Custom Methods

        public void ComputeTotalDays()
        {
            this.TotalDays = 0;
            for (var date = this.FromDate; date <= this.ToDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    this.TotalDays++;
                }
            }
        }

        #endregion
    }
}
