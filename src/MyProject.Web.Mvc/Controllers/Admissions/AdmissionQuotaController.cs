using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using MyProject.Admissions;
using MyProject.Admissions.Dto;
using MyProject.Controllers;
using System.Threading.Tasks;
using MyProject.Admissions;

namespace MyProject.Web.Mvc.Controllers
{
    public class AdmissionQuotaController : MyProjectControllerBase
    {
        private readonly AdmissionQuotaAppService _admissionQuotaAppService;

        public AdmissionQuotaController(
            AdmissionQuotaAppService admissionQuotaAppService)
        {
            _admissionQuotaAppService = admissionQuotaAppService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _admissionQuotaAppService.GetAllAsync(
                new PagedAndSortedResultRequestDto()
            );

            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdmissionQuotaDto input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _admissionQuotaAppService.CreateAsync(input);

            return RedirectToAction(nameof(Index));
        }
    }
}