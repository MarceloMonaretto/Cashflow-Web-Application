using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This example shows how a field can use formatters and validators to
    /// work with date fields - traditionally a fairly complex task, made easy
    /// here.
    ///
    /// Note how the <code>ValidationOpts</code> class is used to specify a
    /// custom error message for the validation methods.
    /// </summary>
    public class DatesController : Controller
    {
        [Route("api/dates")]
        [HttpGet]
        [HttpPost]
        public ActionResult Dates()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                // Allow a number of different formats to be submitted for the various demos
                var update = Format.DATE_ISO_8601;
                var registered = Format.DATE_ISO_8601;
                var format = Request.Query["format"]; // null if not given

                if (format == "custom")
                {
                    update = "M/d/yyyy";
                    registered = "dddd d MMMM yyyy";
                }

                var response = new Editor(db, "users")
                    .Model<DatesModel>()
                    .Field(new Field("updated_date")
                        .Validator(Validation.DateFormat(
                            update,
                            new ValidationOpts { Message = "Please enter a date in the format yyyy-mm-dd" }
                            ))
                        .GetFormatter(Format.DateSqlToFormat(update))
                        .SetFormatter(Format.DateFormatToSql(update))
                    )
                    .Field(new Field("registered_date")
                        .Validator(Validation.DateFormat(
                            registered,
                            new ValidationOpts { Message = "Please enter a date in the format ddd, d MMM yy" }
                            ))
                        .GetFormatter(Format.DateSqlToFormat(registered))
                        .SetFormatter(Format.DateFormatToSql(registered))
                    )
                    .TryCatch(false)
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}
