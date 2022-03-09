using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This example shows how cascading lists can be created.
    /// </summary>
    public class CountriesController : Controller
    {
        [Route("api/countries")]
        [HttpPost]
        public ActionResult Countries()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");
            var request = Request;

            var values = request.Form["values[team.continent]"];
            if (values.Count == 0 || values[0].Length == 0) {
                var result = new Dictionary<string, object>();
                return Json(result);
            }
         
            using (var db = new Database(dbType, dbConnection))
            {
                var query = db.Select(
                    "country",
                    new[] {"id as value", "name as label"},
                    new Dictionary<string, dynamic>(){{"continent", request.Form["values[team.continent]"]}}
                );
         
                var result = new Dictionary<string, object>();
                var options = new Dictionary<string, object>();
                options["team.country"] = query.FetchAll();
                result["options"] = options;

                return Json(result);
            }
        }
    }
}
