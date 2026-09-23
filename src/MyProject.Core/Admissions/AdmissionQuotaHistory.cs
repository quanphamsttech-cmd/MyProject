using Abp.Domain.Entities;
using System;

namespace MyProject.Admissions
{
    public class AdmissionQuotaHistory : Entity<long>
    {
        public long AdmissionQuotaId { get; set; }

        public int OldStatus { get; set; }

        public int NewStatus { get; set; }
        
        public long? UserId { get; set; }

        public DateTime ActionTime { get; set; }

        public string Reason { get; set; }
    }
}