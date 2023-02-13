using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CashFlowUI.HttpClients;

namespace CashFlowUI.Controllers
{
    public class pdfController : Controller
    {

        // GET: pdfController
        public ActionResult Index()
        {
            return View("PdfView");
        }
    }
}
