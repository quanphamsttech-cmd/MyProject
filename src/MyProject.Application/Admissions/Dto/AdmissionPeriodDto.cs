using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace MyProject.Admissions.Dto
{
    public class AdmissionPeriodDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string SchoolYear { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}