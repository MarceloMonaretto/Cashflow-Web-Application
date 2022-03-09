using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    public class CheckboxController : Controller
    {
        [HttpGet, HttpPost, Route("api/checkbox")]
        public ActionResult Checkbox()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "users")
                    .Model<CheckboxModel>()
                    .Field(new Field("image")
                        .SetFormatter((val, data) => (string)val == "" ? 0 : 1)
                    )
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}