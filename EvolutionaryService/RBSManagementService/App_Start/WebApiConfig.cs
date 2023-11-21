﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RBSManagementService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}