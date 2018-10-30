using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace CatchApp.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : CatchAppControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}