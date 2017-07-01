using Elias.DAL.Entities;
using Elias.DAL.Enums;
using Elias.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elias.Web.Code
{
    public static class EmployeeHelper
    {
        public static void SetReservedDays(IDataRepository db, Employee employee)
        {
            var date = new DateTime(DateTime.UtcNow.Year, 1, 1);

            employee.ReservedDays = db.GetLeaveRequests()
                    .Where(w =>
                        w.EmployeeId == employee.Id &&
                        w.StatusId == (byte)LeaveRequestStatusEnum.Accepted &&
                        w.FromDate <= date && date <= w.ToDate
                    )
                    .Sum(sum => (int?)sum.TotalDays) ?? 0;
        }
    }
}