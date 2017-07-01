using Elias.DAL.Enums;
using Elias.Web.Code.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elias.Web.Controllers
{
    [SCIAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Calendar(DateTime start, DateTime end)
        {
            var leaveDays = _db.GetLeaveRequests()
                .Where(w =>
                    w.StatusId == (byte)LeaveRequestStatusEnum.Accepted &&
                    w.FromDate <= end && start <= w.ToDate
                )
                .ToList()
                .Select(s => new
                {
                    start = s.FromDate.ToString("yyyy-MM-dd"),
                    end = s.ToDate.ToString("yyyy-MM-dd"),
                    title = s.Employee.FirstName + " " + s.Employee.LastName
                });

            return Json(leaveDays, JsonRequestBehavior.AllowGet);
        }
    }
}