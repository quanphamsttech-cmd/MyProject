using Abp.Application.Services.Dto;

namespace MyProject.Admissions.Dto
{
    public class AdmissionQuotaFilterInput : PagedAndSortedResultRequestDto
    {
        public long? AdmissionPeriodId { get; set; }

        public string MajorName { get; set; }

        public int? Status { get; set; }
    }
}