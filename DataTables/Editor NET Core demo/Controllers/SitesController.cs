using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    public class SitesController : Controller
    {
        [Route("api/sites")]
        [HttpGet]
        [HttpPost]
        public ActionResult Sites()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION"); 

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "sites")
                    .Field(new Field("id")
                        .Validator(Validation.NotEmpty())
                    )
                    .Field(new Field("name")
                        .Validator(Validation.NotEmpty())
                    )
                    .MJoin(new MJoin("users")
                        .Link("sites.id", "users.site")
                        .Field(new Field("id"))
                    )
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}
