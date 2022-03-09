using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This example is used to show data that is shown in the table only.
    /// Specifically, the `updated_date` field updated automatically by the
    /// database, so we don't need to update ourselves, but we still want to
    /// read that information. The `Set()` method is used to make this a read
    /// only field.
    /// </summary>
    public class TableOnlyDataController : Controller
    {
        [Route("api/tableOnlyData")]
        [HttpGet]
        [HttpPost]
        public ActionResult TableOnlyData()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "users")
                    .Model<TableOnlyDataModel>()
                    .Field(new Field("updated_date")
                        .Set(false)
                        .GetFormatter(Format.DateSqlToFormat(Format.DATE_ISO_822))
                    )
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}
