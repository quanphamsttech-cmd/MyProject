using Abp.AspNetCore.Mvc.Authorization;
using MyProject.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.Web.Controllers;

[AbpMvcAuthorize]
public class AboutController : MyProjectControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
