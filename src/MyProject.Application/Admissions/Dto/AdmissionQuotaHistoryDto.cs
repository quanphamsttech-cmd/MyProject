using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyProject.Admissions.Dto
{
    public class AdmissionQuotaHistoryDto : EntityDto<long>
    {
        public long AdmissionQuotaId { get; set; }

        public int OldStatus { get; set; }

        public int NewStatus { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }

        public DateTime ActionTime { get; set; }

        public string Reason { get; set; }
    }
}