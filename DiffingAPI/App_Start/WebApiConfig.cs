﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DiffingAPI {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "diffApi",
                routeTemplate: "v1/diff/{id}/{action}",
                defaults: new { controller = "diff", action = RouteParameter.Optional }
            );
        }
    }
}
