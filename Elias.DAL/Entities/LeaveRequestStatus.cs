using Elias.DAL.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elias.DAL.Entities
{
    [Table("LeaveRequestStatuses")]
    public partial class LeaveRequestStatus
    {
        [Key]
        public byte Id { get; set; }

        [SCIRequired]
        [SCIMaxLength(20)]
        public string Name { get; set; }
    }
}
