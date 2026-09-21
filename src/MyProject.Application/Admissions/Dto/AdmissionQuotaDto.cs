using Abp.Application.Services.Dto;
using System;

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

        public int Status { get; set; }

        public long? ApprovedBy { get; set; }

        public string ApprovedByName { get; set; }

        public DateTime? ApprovedTime { get; set; }

        public string RejectReason { get; set; }
    }
}