using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyProject.Admissions.Dto;

namespace MyProject.Admissions
{
    public interface IAdmissionPeriodAppService :
        IAsyncCrudAppService<
            AdmissionPeriodDto,
            long,
            PagedAndSortedResultRequestDto,
            AdmissionPeriodDto,
            AdmissionPeriodDto>
    {
    }
}