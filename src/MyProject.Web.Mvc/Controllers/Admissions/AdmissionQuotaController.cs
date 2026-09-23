using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Admissions;
using MyProject.Admissions.Dto;
using MyProject.Controllers;
using ClosedXML.Excel;
using System;
using System.IO;
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

        // =========================================================
        // DANH SÁCH CHỈ TIÊU
        // =========================================================

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

        // =========================================================
        // HIỂN THỊ FORM THÊM
        // =========================================================

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
                IsActive = false,
                Status = 0
            });
        }

        // =========================================================
        // LƯU CHỈ TIÊU
        // =========================================================

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

            input.Status = 0;

            await _admissionQuotaAppService.CreateAsync(input);

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // TÌM KIẾM
        // =========================================================

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

        // =========================================================
        // HIỂN THỊ FORM SỬA
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var quota = await _admissionQuotaAppService
                .GetAsync(new EntityDto<long>
                {
                    Id = id
                });

            var periods = await _admissionPeriodRepository
                .GetAll()
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            ViewBag.AdmissionPeriods = periods;

            return View(quota);
        }

        // =========================================================
        // CẬP NHẬT CHỈ TIÊU
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            AdmissionQuotaDto input)
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

            await _admissionQuotaAppService
                .UpdateAsync(input);

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // GỬI DUYỆT
        // =========================================================

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

        // =========================================================
        // DUYỆT
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(long id)
        {
            await _admissionQuotaAppService.ApproveAsync(
                new EntityDto<long>
                {
                    Id = id
                });

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // TỪ CHỐI
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(
            long id,
            string reason)
        {
            await _admissionQuotaAppService.RejectAsync(
                new EntityDto<long>
                {
                    Id = id
                },
                reason);

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // XÓA
        // =========================================================

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

        // =========================================================
        // LỊCH SỬ
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> History(long id)
        {
            var history =
                await _admissionQuotaAppService
                    .GetHistoryAsync(id);

            return View(history);
        }

        // =========================================================
        // EXPORT EXCEL
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> Export()
        {
            var quotas =
                await _admissionQuotaAppService.GetAllAsync(
                    new PagedAndSortedResultRequestDto
                    {
                        MaxResultCount = 10000
                    });

            using (var workbook = new XLWorkbook())
            {
                var worksheet =
                    workbook.Worksheets.Add(
                        "Chỉ tiêu tuyển sinh");

                // ==============================
                // TIÊU ĐỀ
                // ==============================

                worksheet.Cell(1, 1).Value =
                    "DANH SÁCH CHỈ TIÊU TUYỂN SINH";

                worksheet.Range(1, 1, 1, 10).Merge();

                worksheet.Cell(1, 1)
                    .Style.Font.Bold = true;

                worksheet.Cell(1, 1)
                    .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                // ==============================
                // HEADER
                // ==============================

                worksheet.Cell(3, 1).Value = "STT";
                worksheet.Cell(3, 2).Value = "Kỳ tuyển sinh";
                worksheet.Cell(3, 3).Value = "Ngành";
                worksheet.Cell(3, 4).Value = "Chỉ tiêu";
                worksheet.Cell(3, 5).Value = "Đã tuyển";
                worksheet.Cell(3, 6).Value = "Còn lại";
                worksheet.Cell(3, 7).Value = "Trạng thái";
                worksheet.Cell(3, 8).Value = "Người duyệt";
                worksheet.Cell(3, 9).Value = "Thời gian duyệt";
                worksheet.Cell(3, 10).Value = "Trạng thái hoạt động";

                worksheet.Range(3, 1, 3, 10)
                    .Style.Font.Bold = true;

                // ==============================
                // DỮ LIỆU
                // ==============================

                int row = 4;
                int stt = 1;

                foreach (var item in quotas.Items)
                {
                    worksheet.Cell(row, 1).Value = stt;

                    worksheet.Cell(row, 2).Value =
                        item.AdmissionPeriodId;

                    worksheet.Cell(row, 3).Value =
                        item.MajorName;

                    worksheet.Cell(row, 4).Value =
                        item.Quota;

                    worksheet.Cell(row, 5).Value =
                        item.Enrolled;

                    worksheet.Cell(row, 6).Value =
                        item.Remaining;

                    string status = item.Status switch
                    {
                        0 => "Nháp",
                        1 => "Chờ duyệt",
                        2 => "Đã duyệt",
                        3 => "Từ chối",
                        _ => "Không xác định"
                    };

                    worksheet.Cell(row, 7).Value =
                        status;

                    worksheet.Cell(row, 8).Value =
                        item.ApprovedByName ?? "";

                    worksheet.Cell(row, 9).Value =
                        item.ApprovedTime.HasValue
                            ? item.ApprovedTime.Value
                                .ToString("dd/MM/yyyy HH:mm")
                            : "";

                    worksheet.Cell(row, 10).Value =
                        item.IsActive ? "Có" : "Không";

                    row++;
                    stt++;
                }

                worksheet.Columns()
                    .AdjustToContents();

                // ==============================
                // TẠO FILE
                // ==============================

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"DanhSachChiTieuTuyenSinh_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                    );
                }
            }
        }

        // =========================================================
        // IMPORT EXCEL
        // =========================================================
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Bạn chưa chọn file Excel.";
                return RedirectToAction(nameof(Index));
            }

            var extension = Path.GetExtension(file.FileName);

            if (!extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "File phải có định dạng .xlsx.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);

                        var lastRowUsed = worksheet.LastRowUsed();

                        if (lastRowUsed == null)
                        {
                            TempData["Error"] = "File Excel không có dữ liệu.";
                            return RedirectToAction(nameof(Index));
                        }

                        int lastRow = lastRowUsed.RowNumber();

                        if (lastRow < 4)
                        {
                            TempData["Error"] =
                                "File Excel không có dữ liệu từ dòng 4.";
                            return RedirectToAction(nameof(Index));
                        }

                        int successCount = 0;
                        int skipCount = 0;

                        var errors = new System.Collections.Generic.List<string>();

                        // Dữ liệu bắt đầu từ dòng 4
                        for (int row = 4; row <= lastRow; row++)
                        {
                            try
                            {
                                // ==========================================
                                // CỘT B - KỲ TUYỂN SINH
                                // ==========================================

                                var periodCell = worksheet.Cell(row, 2);

                                if (periodCell.IsEmpty())
                                {
                                    skipCount++;
                                    continue;
                                }

                                long admissionPeriodId;

                                if (periodCell.DataType == XLDataType.Number)
                                {
                                    admissionPeriodId =
                                        Convert.ToInt64(periodCell.GetDouble());
                                }
                                else
                                {
                                    var periodText =
                                        periodCell.GetString().Trim();

                                    if (!long.TryParse(
                                            periodText,
                                            out admissionPeriodId))
                                    {
                                        errors.Add(
                                            $"Dòng {row}: Kỳ tuyển sinh không hợp lệ."
                                        );

                                        continue;
                                    }
                                }

                                // ==========================================
                                // KIỂM TRA KỲ TUYỂN SINH
                                // ==========================================

                                var periodExists =
                                    await _admissionPeriodRepository
                                        .GetAll()
                                        .AnyAsync(x =>
                                            x.Id == admissionPeriodId);

                                if (!periodExists)
                                {
                                    errors.Add(
                                        $"Dòng {row}: Kỳ tuyển sinh ID {admissionPeriodId} không tồn tại."
                                    );

                                    continue;
                                }

                                // ==========================================
                                // CỘT C - NGÀNH
                                // ==========================================

                                var majorName =
                                    worksheet.Cell(row, 3)
                                        .GetString()
                                        .Trim();

                                if (string.IsNullOrWhiteSpace(majorName))
                                {
                                    errors.Add(
                                        $"Dòng {row}: Chưa nhập tên ngành."
                                    );

                                    continue;
                                }

                                // ==========================================
                                // CỘT D - CHỈ TIÊU
                                // ==========================================

                                int quota;

                                var quotaCell =
                                    worksheet.Cell(row, 4);

                                if (quotaCell.DataType == XLDataType.Number)
                                {
                                    quota =
                                        Convert.ToInt32(
                                            quotaCell.GetDouble());
                                }
                                else
                                {
                                    var quotaText =
                                        quotaCell.GetString().Trim();

                                    if (!int.TryParse(
                                            quotaText,
                                            out quota))
                                    {
                                        errors.Add(
                                            $"Dòng {row}: Chỉ tiêu không hợp lệ."
                                        );

                                        continue;
                                    }
                                }

                                if (quota < 0)
                                {
                                    errors.Add(
                                        $"Dòng {row}: Chỉ tiêu không được âm."
                                    );

                                    continue;
                                }

                                // ==========================================
                                // CỘT E - ĐÃ TUYỂN
                                // ==========================================

                                int enrolled = 0;

                                var enrolledCell =
                                    worksheet.Cell(row, 5);

                                if (!enrolledCell.IsEmpty())
                                {
                                    if (enrolledCell.DataType ==
                                        XLDataType.Number)
                                    {
                                        enrolled =
                                            Convert.ToInt32(
                                                enrolledCell.GetDouble());
                                    }
                                    else
                                    {
                                        var enrolledText =
                                            enrolledCell.GetString().Trim();

                                        if (!int.TryParse(
                                                enrolledText,
                                                out enrolled))
                                        {
                                            errors.Add(
                                                $"Dòng {row}: Số đã tuyển không hợp lệ."
                                            );

                                            continue;
                                        }
                                    }
                                }

                                if (enrolled < 0)
                                {
                                    errors.Add(
                                        $"Dòng {row}: Số đã tuyển không được âm."
                                    );

                                    continue;
                                }

                                // ==========================================
                                // CỘT J - TRẠNG THÁI HOẠT ĐỘNG
                                // ==========================================

                                bool isActive = false;

                                var activeText =
                                    worksheet.Cell(row, 10)
                                        .GetString()
                                        .Trim();

                                if (activeText.Equals(
                                        "Có",
                                        StringComparison.OrdinalIgnoreCase) ||
                                    activeText.Equals(
                                        "Yes",
                                        StringComparison.OrdinalIgnoreCase) ||
                                    activeText.Equals(
                                        "true",
                                        StringComparison.OrdinalIgnoreCase) ||
                                    activeText.Equals("1"))
                                {
                                    isActive = true;
                                }

                                // ==========================================
                                // TẠO DỮ LIỆU
                                // ==========================================

                                var input = new AdmissionQuotaDto
                                {
                                    AdmissionPeriodId =
                                        admissionPeriodId,

                                    MajorName =
                                        majorName,

                                    Quota =
                                        quota,

                                    Enrolled =
                                        enrolled,

                                    Remaining =
                                        Math.Max(
                                            0,
                                            quota - enrolled),

                                    IsActive =
                                        isActive,

                                    Status = 0
                                };

                                await _admissionQuotaAppService
                                    .CreateAsync(input);

                                successCount++;
                            }
                            catch (Exception rowEx)
                            {
                                errors.Add(
                                    $"Dòng {row}: {rowEx.Message}"
                                );
                            }
                        }

                        // ==========================================
                        // THÔNG BÁO KẾT QUẢ
                        // ==========================================

                        if (successCount > 0 && errors.Count == 0)
                        {
                            TempData["Success"] =
                                $"Import thành công {successCount} dòng.";
                        }
                        else if (successCount > 0)
                        {
                            TempData["Success"] =
                                $"Import thành công {successCount} dòng, " +
                                $"bỏ qua {errors.Count} dòng lỗi.";
                        }

                        if (successCount == 0)
                        {
                            TempData["Error"] =
                                "Không có dòng nào được import.";
                        }

                        if (errors.Count > 0)
                        {
                            TempData["Error"] =
                                string.Join("<br>", errors.Take(10));

                            if (errors.Count > 0)
                            {
                                TempData["Error"] =
                                    string.Join("<br>", errors.Take(10));

                                if (errors.Count > 10)
                                {
                                    TempData["Error"] +=
                                        $"<br>... và còn {errors.Count - 10} lỗi khác.";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Không thể đọc file Excel: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
