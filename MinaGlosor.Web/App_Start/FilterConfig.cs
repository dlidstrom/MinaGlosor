﻿using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace MinaGlosor.Web
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}