﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ProductService.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ProductService
{
    public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // New code:
        ODataModelBuilder builder = new ODataConventionModelBuilder();
        builder.EntitySet<Product>("Products");
        // New code:
        builder.EntitySet<Supplier>("Suppliers");
        //config.MapODataServiceRoute("ODataRoute", null, builder.GetEdmModel());

        // New code:
        builder.Namespace = "ProductService";
        builder.EntityType<Product>()
            .Action("Rate")
            .Parameter<int>("Rating");

        builder.EntityType<Product>().Collection
            .Function("MostExpensive")
            .Returns<double>();

        builder.Function("GetSalesTaxRate")
    .Returns<double>()
    .Parameter<int>("PostalCode");

        config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel());

    }
}
}
