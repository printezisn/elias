using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Elias.DAL.Repository;
using Elias.Web.Code.Attributes;
using Elias.DAL;
using Elias.DAL.Entities;
using Elias.Web.Code;
using Elias.Web.Models;
using Elias.Web.Models.Enums;
using Elias.DAL.Enums;
using Elias.Web.Hubs;
using System.Threading.Tasks;

namespace Elias.Web.Controllers
{
    [SCIAuthorize]
    public class LeaveRequestsController : BaseController
    {
        #region Fields

        public Dictionary<string, string> _sortableFieldMappings = new Dictionary<string, string>()
        {
            ["name"] = "Employee.FirstName",
            ["status"] = "Status.Name",
            ["requestdate"] = "RequestDate"
        };

        #endregion

        #region CRUD

        /// <summary>
        /// Renders the index page. It contains a table with all the leave requests
        /// </summary>
        /// <param name="page">The page to read</param>
        /// <param name="searchTerm">The string used for search filtering</param>
        /// <param name="sortBy">The field to sort by</param>
        /// <param name="isAsc">Indicates if the sorting process is ascending or descending</param>
        /// <returns>The index page view</returns>
        [HttpGet]
        public ActionResult Index(int page = 1, string searchTerm = null, string sortBy = "", bool isAsc = false)
        {
            sortBy = _sortableFieldMappings.ContainsKey(sortBy.ToLower()) ? sortBy.ToLower() : "requestdate";
            var collection = _db.GetPagedLeaveRequests(page, AppConstants.DEFAULT_PAGE_SIZE, searchTerm, _sortableFieldMappings[sortBy], isAsc);
            collection.SearchTerm = searchTerm;
            collection.SortBy = sortBy;
            collection.IsAscendingOrder = isAsc;

            foreach (var model in collection)
            {
                EmployeeHelper.SetReservedDays(_db, model.Employee);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Grid", collection);
            }

            return View(collection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Accept(Guid id)
        {
            try
            {
                var leaveRequest = _db.GetLeaveRequest(id);
                if (leaveRequest == null)
                {
                    return Json(new { Message = new ToastrMessage(null, "The leave request was not found.", ToastrMessageTypeEnum.Error) });
                }

                if (leaveRequest.StatusId != (byte)LeaveRequestStatusEnum.Pending)
                {
                    return Json(new { Message = new ToastrMessage(null, "The leave request is not valid anymore.", ToastrMessageTypeEnum.Error) });
                }

                EmployeeHelper.SetReservedDays(_db, leaveRequest.Employee);
                if (leaveRequest.Employee.ReservedDays + leaveRequest.TotalDays > leaveRequest.Employee.LeaveDays)
                {
                    return Json(new { Message = new ToastrMessage(null, "The employee doesn't have enough remaining days.", ToastrMessageTypeEnum.Error) });
                }

                leaveRequest.StatusId = (byte)LeaveRequestStatusEnum.Accepted;
                _db.Save();

                NotificationHub.UpdateLeaveRequests();

                // TODO: Send message to employee
                await EmployeeHelper.SendMessage(leaveRequest.Employee, $"Your leave request from {leaveRequest.FromDate.ToString("yyyy-MM-dd")} to {leaveRequest.ToDate.ToString("yyyy-MM-dd")} was accepted!");

                return Json(new { Message = new ToastrMessage(null, "The leave request was accepted successfully!", ToastrMessageTypeEnum.Success) });
            }
            catch (Exception ex)
            {
                return Json(new { Message = new ToastrMessage(null, AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Reject(Guid id)
        {
            try
            {
                var leaveRequest = _db.GetLeaveRequest(id);
                if (leaveRequest == null)
                {
                    return Json(new { Message = new ToastrMessage(null, "The leave request was not found.", ToastrMessageTypeEnum.Error) });
                }

                if (leaveRequest.StatusId != (byte)LeaveRequestStatusEnum.Pending)
                {
                    return Json(new { Message = new ToastrMessage(null, "The leave request is not valid anymore.", ToastrMessageTypeEnum.Error) });
                }

                leaveRequest.StatusId = (byte)LeaveRequestStatusEnum.Rejected;
                _db.Save();

                NotificationHub.UpdateLeaveRequests();

                // TODO: Send message to employee
                await EmployeeHelper.SendMessage(leaveRequest.Employee, $"Your leave request from {leaveRequest.FromDate.ToString("yyyy-MM-dd")} to {leaveRequest.ToDate.ToString("yyyy-MM-dd")} was rejected.");

                return Json(new { Message = new ToastrMessage(null, "The leave request was rejected successfully!", ToastrMessageTypeEnum.Success) });
            }
            catch (Exception ex)
            {
                return Json(new { Message = new ToastrMessage(null, AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error) });
            }
        }

        public JsonResult TotalLeaveRequests()
        {
            var date = new DateTime(DateTime.UtcNow.Year, 1, 1);

            int total = _db.GetLeaveRequests()
                .Count(c => c.StatusId == (byte)LeaveRequestStatusEnum.Pending && c.FromDate >= date && date <= c.ToDate);

            return Json(total, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
