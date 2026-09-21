using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Admissions;
using MyProject.Admissions.Dto;
using MyProject.Controllers;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Web.Mvc.Controllers
{
    public class AdmissionQuotaController : MyProjectControllerBase
    {
        private readonly AdmissionQuotaAppService _admissionQuotaAppService;

        private readonly IRepository<AdmissionPeriod, long>
            _admissionPeriodRepository;

        public AdmissionQuotaController(
            AdmissionQuotaAppService admissionQuotaAppService,
            IRepository<AdmissionPeriod, long> admissionPeriodRepository)
        {
            _admissionQuotaAppService = admissionQuotaAppService;
            _admissionPeriodRepository = admissionPeriodRepository;
        }

        // ==============================
        // DANH SÁCH CHỈ TIÊU
        // ==============================
        public async Task<IActionResult> Index()
        {
            var result = await _admissionQuotaAppService.GetAllAsync(
                new PagedAndSortedResultRequestDto()
            );

            return View(result);
        }

        // ==============================
        // HIỂN THỊ FORM THÊM
        // ==============================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var periods = await _admissionPeriodRepository
                .GetAll()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.AdmissionPeriods = periods;

            return View(new AdmissionQuotaDto
            {
                Enrolled = 0,
                IsActive = false
            });
        }

        // ==============================
        // LƯU CHỈ TIÊU
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdmissionQuotaDto input)
        {
            if (!ModelState.IsValid)
            {
                var periods = await _admissionPeriodRepository
                    .GetAll()
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                ViewBag.AdmissionPeriods = periods;

                return View(input);
            }

            // Chỉ tiêu mới mặc định là Nháp
            input.Status = 0;

            await _admissionQuotaAppService.CreateAsync(input);

            return RedirectToAction(nameof(Index));
        }
    }
}