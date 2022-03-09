using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This example is used for the standalone examples. They don't actually
    /// save to the database as the behaviour of such an edit will depend highly
    /// upon the structure of your database. However using the Database methods
    /// such as update() this can easily be done.
    ///
    /// See the .NET manual - http://editor.datatables.net/manual/net - for
    /// information on how to use the Editor .NET libraries.
    /// </summary>
    public class StandaloneController : Controller
    {
        [Route("api/standalone")]
        [HttpGet]
        [HttpPost]
        public ActionResult Standalone()
        {
            return Json(new Object());
        }
    }
}
