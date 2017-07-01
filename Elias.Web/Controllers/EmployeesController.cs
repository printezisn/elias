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
using Elias.Web.Models.Enums;

namespace Elias.Web.Controllers
{
    [SCIAuthorize]
    public class EmployeesController : BaseController
    {
        #region Fields

        public Dictionary<string, string> _sortableFieldMappings = new Dictionary<string, string>()
        {
            ["firstname"] = "FirstName",
            ["lastname"] = "LastName",
            ["email"] = "Email",
            ["leavedays"] = "LeaveDays"
        };

        #endregion

        #region CRUD

        /// <summary>
        /// Renders the index page. It contains a table with all the employees
        /// </summary>
        /// <param name="page">The page to read</param>
        /// <param name="searchTerm">The string used for search filtering</param>
        /// <param name="sortBy">The field to sort by</param>
        /// <param name="isAsc">Indicates if the sorting process is ascending or descending</param>
        /// <returns>The index page view</returns>
        [HttpGet]
        public ActionResult Index(int page = 1, string searchTerm = null, string sortBy = "", bool isAsc = true)
        {
            sortBy = _sortableFieldMappings.ContainsKey(sortBy.ToLower()) ? sortBy.ToLower() : "lastname";
            var collection = _db.GetPagedEmployees(page, AppConstants.DEFAULT_PAGE_SIZE, searchTerm, _sortableFieldMappings[sortBy], isAsc);
            collection.SearchTerm = searchTerm;
            collection.SortBy = sortBy;
            collection.IsAscendingOrder = isAsc;

            foreach (var model in collection)
            {
                EmployeeHelper.SetReservedDays(_db, model);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Grid", collection);
            }

            return View(collection);
        }

        /// <summary>
        /// Renders the employee details page
        /// </summary>
        /// <param name="id">The id of the employee</param>
        /// <returns>The employee details page view if the employee exists, otherwise a redirect result the index page</returns>
        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _db.GetEmployee(id);
            if (model == null)
            {
                ShowMessage("The employee was not found.", ToastrMessageTypeEnum.Error);
                return RedirectToAction("index");
            }

            EmployeeHelper.SetReservedDays(_db, model);

            return View(model);
        }

        /// <summary>
        /// Renders the create employee page
        /// </summary>
        /// <returns>The create employee page view</returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Creates a new employee
        /// </summary>
        /// <returns>A redirect result to the details page if the operation is successful, otherwise the create employee page view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Email,LeaveDays,SkypeId,FacebookId,ServiceUrl")] Employee model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                _db.Add(model);
                _db.Save();

                ShowMessage("The employee was created successfully!", ToastrMessageTypeEnum.Success);

                return RedirectToAction("details", new { id = model.Id });
            }
            catch (Exception ex)
            {
                this.ShowMessage(AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error);
                return View(model);
            }
        }

        /// <summary>
        /// Renders the edit employee page
        /// </summary>
        /// <param name="id">The id of the employee</param>
        /// <returns>A redirect result to the index page if the employee doesn't exist, otherwise the edit employee page view</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _db.GetEmployee(id);
            if (model == null)
            {
                ShowMessage("The employee was not found.", ToastrMessageTypeEnum.Error);
                return RedirectToAction("index");
            }

            return View(model);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Updates a employee
        /// </summary>
        /// <param name="id">The id of the employee</param>
        /// <param name="model">The model of the employee</param>
        /// <returns>A redirect result to the details page if the operation is successful, otherwise the edit employee page view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "FirstName,LastName,Email,LeaveDays,SkypeId,FacebookId,ServiceUrl")] Employee model)
        {
            var entity = _db.GetEmployee(id);
            if (entity == null)
            {
                ShowMessage("The employee was not found.", ToastrMessageTypeEnum.Error);
                return RedirectToAction("index");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entity);
                }

                if (!TryUpdateModel(entity, new string[] { "FirstName", "LastName", "Email", "LeaveDays", "SkypeId", "FacebookId", "ServiceUrl" }))
                {
                    this.ShowMessage(AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error);
                    return View(entity);
                }

                _db.Save();

                ShowMessage("The employee was updated successfully!", ToastrMessageTypeEnum.Success);

                return RedirectToAction("details", new { id = id });
            }
            catch (Exception ex)
            {
                this.ShowMessage(AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error);
                return View(entity);
            }
        }

        /// <summary>
        /// Renders the delete employee page
        /// </summary>
        /// <param name="id">The id of the employee</param>
        /// <returns>The delete employee page view if the employee exists, otherwise a redirect result the index page</returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = _db.GetEmployee(id);
            if (model == null)
            {
                ShowMessage("The employees was not found.", ToastrMessageTypeEnum.Error);
                return RedirectToAction("index");
            }

            EmployeeHelper.SetReservedDays(_db, model);

            return View(model);
        }

        /// <summary>
        /// Deletes a employee
        /// </summary>
        /// <param name="id">The id of the employee</param>
        /// <param name="formCollection">The collection of form values. This is only used only in order to make the method overload possible</param>
        /// <returns>A redirect result to the index page with a success message if the operation is successful, otherwise with an error message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection formCollection)
        {
            var model = _db.GetEmployee(id);
            if (model == null)
            {
                ShowMessage("The employee was not found.", ToastrMessageTypeEnum.Error);
                return RedirectToAction("index");
            }

            try
            {
                _db.Delete(model);
                _db.Save();

                ShowMessage("The employee was deleted successfully!", ToastrMessageTypeEnum.Success);

                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                this.ShowMessage(AppGlobalMessages.UnexpectedErrorMessage, ToastrMessageTypeEnum.Error);
                EmployeeHelper.SetReservedDays(_db, model);

                return View(model);
            }
        }

        #endregion
    }
}
