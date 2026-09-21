using Abp.Domain.Entities;
using System;

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

        // 0: Nháp
        // 1: Chờ duyệt
        // 2: Đã duyệt
        // 3: Từ chối
        public int Status { get; set; }

        // Người duyệt
        public long? ApprovedBy { get; set; }

        // Thời gian duyệt
        public DateTime? ApprovedTime { get; set; }

        // Lý do từ chối
        public string RejectReason { get; set; }
    }
}