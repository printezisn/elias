﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elias.Web.Code
{
    public static class AppConstants
    {
        public const string SESSION_CURRENT_USER_KEY = "CurrentUser";
        public const string PAGE_ERRORS_TEMPDATA_KEY = "PageErrors";
        public const string APP_MESSAGES_TEMPDATA_KEY = "AppMessages";
        public const string ASSETS_CACHE_KEY = "Assets";
        public const string HTTP_CONTEXT_ITEMS_USER_SESSION_KEY = "UserSession";
        public const int DEFAULT_PAGE_SIZE = 10;
    }
}