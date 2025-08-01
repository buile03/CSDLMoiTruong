﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TechLife.CSDLMoiTruong.Common
{
    public class SystemConstants
    {
        public const string MainConnectionString = "ConnectionString";
        public const string Alerts = "Alerts";
        public const string UrlBack = "UrlBack";
        public const string UrlIndex = "UrlIndex";
        public const string loginFailed = "loginFailed";

        public const int pageSize = 20;
        public const int pageIndex = 1;
        public class AppSettings
        {
            public const string Key = "App";
            public const string Token = "Token";
            public const string UniqueCode = "00.08.H57";

            public const int ExpireMinutes = 30;
        }

        public class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 4;
            public const int NumberOfLatestProducts = 6;
        }

        public class ProductConstants
        {
            public const string NA = "N/A";
        }
    }
}
