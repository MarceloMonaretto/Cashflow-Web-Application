using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataTables;
using EditorNetCoreDemo.Models;

namespace EditorNetCoreDemo.Controllers
{
    /// <summary>
    /// This Controller is used to demonstrate SearchPanes use through Server Side Processing
    /// </summary>
    public class SearchPanesController : Controller
    {
        [Route("api/searchPanes")]
        [HttpGet]
        [HttpPost]
        public ActionResult SearchPanes()
        {
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            var dbConnection = Environment.GetEnvironmentVariable("DBCONNECTION"); 

            using (var db = new Database(dbType, dbConnection))
            {
                var response = new Editor(db, "users")
                    .Model<UploadManyModel>()
                    .Field(new Field("users.first_name")
                        .SearchPaneOptions( new SearchPaneOptions() )
                    )
                    .Field(new Field("users.last_name")
                        .SearchPaneOptions( new SearchPaneOptions() )
                    )
                    .Field(new Field("users.phone")
                        .SearchPaneOptions(new SearchPaneOptions()
                            .Table("users")
                            .Value("phone")
                        )
                    )
                    .Field(new Field("sites.name")
                        .SearchPaneOptions(new SearchPaneOptions()
                            .Label("sites.name")
                            .Value("sites.name")
                            .LeftJoin("sites", "sites.id", "=", "users.site")
                        )
                    )
                    .Field(new Field("users.site")
                        .Options(new Options()
                            .Table("sites")
                            .Value("id")
                            .Label("name")
                        )
                    )
                    .LeftJoin("sites", "sites.id", "=", "users.site")
                    .Debug(true)
                    .TryCatch(false)
                    .Process(Request)
                    .Data();
                return Json(response);
            }
        }
    }
}
