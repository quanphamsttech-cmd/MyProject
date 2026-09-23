using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MyProject.Admissions.Dto;
using MyProject.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Admissions
{
    public class AdmissionQuotaAppService :
        AsyncCrudAppService<
            AdmissionQuota,
            AdmissionQuotaDto,
            long,
            PagedAndSortedResultRequestDto,
            AdmissionQuotaDto,
            AdmissionQuotaDto>
    {
        private readonly IRepository<AdmissionQuotaHistory, long> _historyRepository;

        public AdmissionQuotaAppService(
            IRepository<AdmissionQuota, long> repository,
            IRepository<AdmissionQuotaHistory, long> historyRepository)
            : base(repository)
        {
            _historyRepository = historyRepository;
        }

        // =====================================================
        // 1. XEM DANH SÁCH
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota)]
        public override async Task<PagedResultDto<AdmissionQuotaDto>> GetAllAsync(
            PagedAndSortedResultRequestDto input)
        {
            return await base.GetAllAsync(input);
        }


        // =====================================================
        // 2. THÊM CHỈ TIÊU
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Create)]
        public override async Task<AdmissionQuotaDto> CreateAsync(
            AdmissionQuotaDto input)
        {
            input.Remaining = input.Quota - input.Enrolled;

            if (input.Remaining < 0)
            {
                input.Remaining = 0;
            }

            // Chỉ tiêu mới tạo ở trạng thái Nháp
            input.Status = 0;

            return await base.CreateAsync(input);
        }


        // =====================================================
        // 3. SỬA CHỈ TIÊU
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Edit)]
        public override async Task<AdmissionQuotaDto> UpdateAsync(
            AdmissionQuotaDto input)
        {
            var entity = await Repository.GetAsync(input.Id);

            // Không cho sửa chỉ tiêu đã duyệt
            if (entity.Status == 2)
            {
                throw new Exception(
                    "Chỉ tiêu đã được duyệt, không thể sửa.");
            }

            input.Remaining = input.Quota - input.Enrolled;

            if (input.Remaining < 0)
            {
                input.Remaining = 0;
            }

            return await base.UpdateAsync(input);
        }


        // =====================================================
        // 4. XÓA CHỈ TIÊU
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Edit)]
        public override async Task DeleteAsync(
            EntityDto<long> input)
        {
            var entity = await Repository.GetAsync(input.Id);

            // Không cho xóa chỉ tiêu đã duyệt
            if (entity.Status == 2)
            {
                throw new Exception(
                    "Chỉ tiêu đã được duyệt, không thể xóa.");
            }

            await base.DeleteAsync(input);
        }


        // =====================================================
        // 5. GỬI DUYỆT
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Submit)]
        public async Task<AdmissionQuotaDto> SubmitAsync(EntityDto<long> input)
        {
            var entity = await Repository.GetAsync(input.Id);

            if (entity.Status != 0 && entity.Status != 3)
                throw new Exception(
                    "Chỉ tiêu chỉ có thể gửi duyệt khi đang ở trạng thái Nháp hoặc Từ chối.");

            var oldStatus = entity.Status;

            entity.Status = 1;
            entity.RejectReason = null;
            entity.ApprovedBy = null;
            entity.ApprovedTime = null;

            await Repository.UpdateAsync(entity);

            await _historyRepository.InsertAsync(new AdmissionQuotaHistory
            {
                AdmissionQuotaId = entity.Id,
                OldStatus = oldStatus,
                NewStatus = 1,
                UserId = AbpSession.UserId,
                ActionTime = DateTime.Now,
                Reason = oldStatus == 3
                    ? "Gửi duyệt lại"
                    : "Gửi duyệt"
            });

            return MapToEntityDto(entity);
        }


        // =====================================================
        // 6. DUYỆT
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Approve)]
        public async Task<AdmissionQuotaDto> ApproveAsync(EntityDto<long> input)
        {
            var entity = await Repository.GetAsync(input.Id);

            if (entity.Status != 1)
                throw new Exception(
                    "Chỉ tiêu phải ở trạng thái Chờ duyệt mới được duyệt.");

            var oldStatus = entity.Status;

            entity.Status = 2;
            entity.ApprovedBy = AbpSession.UserId;
            entity.ApprovedTime = DateTime.Now;
            entity.RejectReason = null;

            await Repository.UpdateAsync(entity);

            await _historyRepository.InsertAsync(new AdmissionQuotaHistory
            {
                AdmissionQuotaId = entity.Id,
                OldStatus = oldStatus,
                NewStatus = 2,
                UserId = AbpSession.UserId,
                ActionTime = DateTime.Now,
                Reason = "Duyệt chỉ tiêu"
            });

            return MapToEntityDto(entity);
        }


        // =====================================================
        // 7. TỪ CHỐI
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Reject)]
        public async Task<AdmissionQuotaDto> RejectAsync(
    EntityDto<long> input,
    string reason)
        {
            var entity = await Repository.GetAsync(input.Id);

            if (entity.Status != 1)
                throw new Exception(
                    "Chỉ tiêu phải ở trạng thái Chờ duyệt mới được từ chối.");

                if (string.IsNullOrWhiteSpace(reason))
                    
            {
                throw new Exception(
                    "Vui lòng nhập lý do từ chối.");
            }
            var oldStatus = entity.Status;

            entity.Status = 3;
            entity.RejectReason = reason;
            entity.ApprovedBy = null;
            entity.ApprovedTime = DateTime.Now;

            await Repository.UpdateAsync(entity);

            await _historyRepository.InsertAsync(new AdmissionQuotaHistory
            {
                AdmissionQuotaId = entity.Id,
                OldStatus = oldStatus,
                NewStatus = 3,
                UserId = AbpSession.UserId,
                ActionTime = DateTime.Now,
                Reason = reason
            });

            return MapToEntityDto(entity);
        }
        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota)]
        public async Task<PagedResultDto<AdmissionQuotaDto>> GetFilteredAsync(
    AdmissionQuotaFilterInput input)
        {
            var query = Repository.GetAll();

            // Lọc theo kỳ tuyển sinh
            if (input.AdmissionPeriodId.HasValue)
            {
                query = query.Where(x =>
                    x.AdmissionPeriodId == input.AdmissionPeriodId.Value);
            }

            // Lọc theo tên ngành
            if (!string.IsNullOrWhiteSpace(input.MajorName))
            {
                query = query.Where(x =>
                    x.MajorName.Contains(input.MajorName));
            }

            // Lọc theo trạng thái
            if (input.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == input.Status.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            var result = new PagedResultDto<AdmissionQuotaDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<AdmissionQuotaDto>>(items)
            };

            return result;
        }
        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota)]
        public async Task<List<AdmissionQuotaHistoryDto>> GetHistoryAsync(long admissionQuotaId)
        {
            var histories = await _historyRepository
                .GetAll()
                .Where(x => x.AdmissionQuotaId == admissionQuotaId)
                .OrderByDescending(x => x.ActionTime)
                .ToListAsync();

            return ObjectMapper.Map<List<AdmissionQuotaHistoryDto>>(histories);
        }
    }
}