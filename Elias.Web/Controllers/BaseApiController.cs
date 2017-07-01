using Elias.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Elias.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        protected IDataRepository _db;

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();

            base.Dispose(disposing);
        }

        public BaseApiController()
        {
            _db = new DataRepository();
        }
    }
}