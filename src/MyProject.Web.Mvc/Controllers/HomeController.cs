using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Admissions;
using MyProject.Controllers;
using System.Threading.Tasks;

namespace MyProject.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : MyProjectControllerBase
{
    private readonly IRepository<AdmissionQuota, long> _admissionQuotaRepository;

    public HomeController(
        IRepository<AdmissionQuota, long> admissionQuotaRepository)
    {
        _admissionQuotaRepository = admissionQuotaRepository;
    }
    public async Task<IActionResult> Index()
    {
        var query = _admissionQuotaRepository.GetAll();

        // Tổng chỉ tiêu
        var totalQuota = await query
            .SumAsync(x => (int?)x.Quota) ?? 0;

        // Tổng đã tuyển
        var totalEnrolled = await query
            .SumAsync(x => (int?)x.Enrolled) ?? 0;

        // Tổng còn lại
        var totalRemaining = await query
            .SumAsync(x => (int?)x.Remaining) ?? 0;

        // Số chỉ tiêu đang chờ duyệt
        var pendingCount = await query
            .CountAsync(x => x.Status == 1);

        // Số đã duyệt
        var approvedCount = await query
            .CountAsync(x => x.Status == 2);

        // Số từ chối
        var rejectedCount = await query
            .CountAsync(x => x.Status == 3);

        // Số bản nháp
        var draftCount = await query
            .CountAsync(x => x.Status == 0);

        ViewBag.TotalQuota = totalQuota;
        ViewBag.TotalEnrolled = totalEnrolled;
        ViewBag.TotalRemaining = totalRemaining;
        ViewBag.PendingCount = pendingCount;

        ViewBag.ApprovedCount = approvedCount;
        ViewBag.RejectedCount = rejectedCount;
        ViewBag.DraftCount = draftCount;

        return View();
    }
}
