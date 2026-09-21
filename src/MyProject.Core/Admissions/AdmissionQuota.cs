using Abp.Domain.Entities;

namespace MyProject.Admissions
{
    public class AdmissionQuota : Entity<long>
    {
        public long AdmissionPeriodId { get; set; }

        public string MajorName { get; set; }

        public int Quota { get; set; }  

        public int Enrolled { get; set; }

        public int Remaining { get; set; }

        public bool IsActive { get; set; }
    }
}