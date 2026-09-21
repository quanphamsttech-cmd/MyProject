using Abp.Application.Services.Dto;

namespace MyProject.Admissions.Dto
{
    public class AdmissionQuotaDto : EntityDto<long>
    {
        public long AdmissionPeriodId { get; set; }

        public string MajorName { get; set; }

        public int Quota { get; set; }

        public int Enrolled { get; set; }

        public int Remaining { get; set; }

        public bool IsActive { get; set; }
    }
}