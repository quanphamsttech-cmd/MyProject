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

            var periods = await _admissionPeriodRepository
                .GetAll()
                .ToListAsync();

            ViewBag.AdmissionPeriods = periods;

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
        [HttpGet]
        public async Task<IActionResult> Search(
    AdmissionQuotaFilterInput input)
        {
            var result = await _admissionQuotaAppService
                .GetFilteredAsync(input);

            var periods = await _admissionPeriodRepository
                .GetAll()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.AdmissionPeriods = periods;

            return View("Index", result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var quota = await _admissionQuotaAppService
                .GetAsync(new EntityDto<long> { Id = id });

            var periods = await _admissionPeriodRepository
                .GetAll()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.AdmissionPeriods = periods;

            return View(quota);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdmissionQuotaDto input)
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

            await _admissionQuotaAppService.UpdateAsync(input);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(long id)
        {
            await _admissionQuotaAppService.SubmitAsync(
                new EntityDto<long>
                {
                    Id = id
                });

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            await _admissionQuotaAppService.DeleteAsync(
                new EntityDto<long>
                {
                    Id = id
                });

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> History(long id)
        {
            var history = await _admissionQuotaAppService.GetHistoryAsync(id);

            return View(history);
        }
    }
}