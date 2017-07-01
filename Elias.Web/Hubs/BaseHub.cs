using Elias.DAL.Repository;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elias.Web.Hubs
{
    public class BaseHub : Hub
    {
        protected IDataRepository _db;

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();

            base.Dispose(disposing);
        }

        public BaseHub()
        {
            _db = new DataRepository();
        }
    }
}