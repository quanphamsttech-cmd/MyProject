using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using MyProject.Admissions.Dto;
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

        public override async Task<AdmissionQuotaDto> CreateAsync(
            AdmissionQuotaDto input)
        {
            input.Remaining = input.Quota - input.Enrolled;

            if (input.Remaining < 0)
            {
                input.Remaining = 0;
            }

            return await base.CreateAsync(input);
        }

        public override async Task<AdmissionQuotaDto> UpdateAsync(
            AdmissionQuotaDto input)
        {
            input.Remaining = input.Quota - input.Enrolled;

            if (input.Remaining < 0)
            {
                input.Remaining = 0;
            }

            return await base.UpdateAsync(input);
        }
    }
}