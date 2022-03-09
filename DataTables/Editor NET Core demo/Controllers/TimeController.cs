using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    public class TimeController : Controller
    {
        [Route("api/time")]
        [HttpGet]
        [HttpPost]
        public ActionResult Join()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "users")
                    .TryCatch(false)
                    .Model<TimeModel>()
                    .Field(new Field("shift_start")
                        .Validator(Validation.DateFormat(
                            "h:mm tt"
                        ))
                        .GetFormatter(Format.DateTime("HH:mm:ss", "h:mm tt"))
                        .SetFormatter(Format.DateTime("h:mm tt", "HH:mm:ss"))
                    )
                    .Field(new Field("shift_end")
                        .Validator(Validation.DateFormat(
                            "HH:mm:ss"
                        ))
                        .GetFormatter(Format.DateTime("HH:mm:ss"))
                    // No set formatter required, as already in IS0 format
                    )
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}