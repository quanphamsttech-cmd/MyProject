using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using MyProject.Admissions.Dto;
using MyProject.Authorization;
using System;
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
        public AdmissionQuotaAppService(
            IRepository<AdmissionQuota, long> repository)
            : base(repository)
        {
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
        public async Task<AdmissionQuotaDto> SubmitAsync(
            EntityDto<long> input)
        {
            var entity = await Repository.GetAsync(input.Id);

            // Chỉ Nháp hoặc Từ chối mới được gửi duyệt
            if (entity.Status != 0 && entity.Status != 3)
            {
                throw new Exception(
                    "Chỉ tiêu chỉ có thể gửi duyệt khi đang ở trạng thái Nháp hoặc Từ chối.");
            }

            entity.Status = 1;

            // Xóa thông tin từ chối cũ
            entity.RejectReason = null;
            entity.ApprovedBy = null;
            entity.ApprovedTime = null;

            await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }


        // =====================================================
        // 6. DUYỆT
        // =====================================================

        [AbpAuthorize(PermissionNames.Pages_AdmissionQuota_Approve)]
        public async Task<AdmissionQuotaDto> ApproveAsync(
            EntityDto<long> input)
        {
            var entity = await Repository.GetAsync(input.Id);

            // Chỉ trạng thái Chờ duyệt mới được duyệt
            if (entity.Status != 1)
            {
                throw new Exception(
                    "Chỉ tiêu phải ở trạng thái Chờ duyệt mới được duyệt.");
            }

            entity.Status = 2;

            // Lưu người duyệt
            entity.ApprovedBy = AbpSession.UserId;

            // Lưu thời gian duyệt
            entity.ApprovedTime = DateTime.Now;

            // Xóa lý do từ chối nếu có
            entity.RejectReason = null;

            await Repository.UpdateAsync(entity);

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

            // Chỉ trạng thái Chờ duyệt mới được từ chối
            if (entity.Status != 1)
            {
                throw new Exception(
                    "Chỉ tiêu phải ở trạng thái Chờ duyệt mới được từ chối.");
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new Exception(
                    "Vui lòng nhập lý do từ chối.");
            }

            entity.Status = 3;

            entity.RejectReason = reason;

            // Xóa thông tin duyệt cũ
            entity.ApprovedBy = null;

            // Lưu thời gian xử lý
            entity.ApprovedTime = DateTime.Now;

            await Repository.UpdateAsync(entity);

            return MapToEntityDto(entity);
        }
    }
}