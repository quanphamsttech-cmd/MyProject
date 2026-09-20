using Abp.Domain.Entities;
using System;

namespace MyProject.Admissions
{
    public class AdmissionPeriod : Entity<long>
    {
        public string Name { get; set; }

        public string SchoolYear { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}