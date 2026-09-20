using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using MyProject.Admissions;
using MyProject.Authorization;
using MyProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyProject.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class AdmissionPeriodsController : MyProjectControllerBase
    {
        private readonly IAdmissionPeriodAppService _admissionPeriodAppService;

        public AdmissionPeriodsController(
            IAdmissionPeriodAppService admissionPeriodAppService)
        {
            _admissionPeriodAppService = admissionPeriodAppService;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}