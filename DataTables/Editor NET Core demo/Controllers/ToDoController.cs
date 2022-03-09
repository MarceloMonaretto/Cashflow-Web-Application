using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This controller is used by the "To Do" examples on the client-side to
    /// show how Editor can display fields of different types.
    /// </summary>
    public class ToDoController : Controller
    {
        [Route("api/todo")]
        [HttpGet]
        [HttpPost]
        public ActionResult ToDo()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION");

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "todo")
                    .Model<ToDoModel>()
                    .Field(new Field("done").Validator(Validation.Boolean()))
                    .Field(new Field("priority").Validator(Validation.Numeric()))
                    .Process(Request)
                    .Data();

                return Json(response);
            }
        }
    }
}
