using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This example shows a modification of the default `Staff` controller
    /// whereby the `id` field is ready directly from the database, allowing the
    /// client-side to show its use of the `idSrc` option.
    /// </summary>
    public class JsonIdController : Controller
    {
        [Route("api/jsonId")]
        [HttpGet]
        [HttpPost]
        public ActionResult Staff()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "datatables_demo")
                    .Model<StaffModel>()
                    .Field(new Field("id").Set(false)) // ID is automatically set by the database on create
                    .Field(new Field("first_name").Validator(Validation.NotEmpty()))
                    .Field(new Field("last_name").Validator(Validation.NotEmpty()))
                    .Field(new Field("extn").Validator(Validation.Numeric()))
                    .Field(new Field("age").Validator(Validation.Numeric()))
                    .Field(new Field("salary").Validator(Validation.Numeric()))
                    .Field(new Field("start_date")
                        .Validator(DataTables.Validation.DateFormat(
                            Format.DATE_ISO_8601,
                            new ValidationOpts { Message = "Please enter a date in the format yyyy-mm-dd" }
                            ))
                        .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_8601))
                        .SetFormatter(Format.DateFormatToSql(Format.DATE_ISO_8601))
                    )
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}
