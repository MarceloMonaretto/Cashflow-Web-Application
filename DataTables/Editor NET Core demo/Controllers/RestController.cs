using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This example shows how the Editor libraries can be used in a REST like
    /// environment, where different actions are sent to different URLs and / or
    /// use different HTTP verbs.
    /// 
    /// Note that the Editor instance will automatically detect which request
    /// type is being made, so all of the routes can be passed through to a
    /// single controller method.
    /// </summary>
    public class RestController : Controller
    {
        public ActionResult Rest(HttpRequest request)
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "datatables_demo")
                    .Model<StaffModel>()
                    .Field(new Field("first_name").Validator(Validation.NotEmpty()))
                    .Field(new Field("last_name").Validator(Validation.NotEmpty()))
                    .Field(new Field("extn").Validator(Validation.Numeric()))
                    .Field(new Field("age").Validator(Validation.Numeric()))
                    .Field(new Field("salary").Validator(Validation.Numeric()))
                    .Field(new Field("start_date")
                        .Validator(Validation.DateFormat(
                            Format.DATE_ISO_8601,
                            new ValidationOpts { Message = "Please enter a date in the format yyyy-mm-dd" }
                            ))
                        .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                        .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Process(request)
                    .Data();

                return Json(response);
            }
        }

        [Route("api/rest/get")]
        [HttpGet]
        public ActionResult Get()
        {
            return Rest(Request);
        }

        [Route("api/rest/create")]
        [HttpPost]
        public ActionResult Create()
        {
            return Rest(Request);
        }

        [Route("api/rest/edit")]
        [HttpPut]
        public ActionResult Edit()
        {
            return Rest(Request);
        }

        [Route("api/rest/remove")]
        [HttpDelete]
        public ActionResult Remove()
        {
            return Rest(Request);
        }
    }
}
