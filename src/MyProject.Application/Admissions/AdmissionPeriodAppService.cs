using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using MyProject.Admissions.Dto;
using System;
using System.Threading.Tasks;

namespace MyProject.Admissions
{
    public class AdmissionPeriodAppService :
        AsyncCrudAppService<
            AdmissionPeriod,
            AdmissionPeriodDto,
            long,
            PagedAndSortedResultRequestDto,
            AdmissionPeriodDto,
            AdmissionPeriodDto>,
        IAdmissionPeriodAppService
    {
        public AdmissionPeriodAppService(
            IRepository<AdmissionPeriod, long> repository)
            : base(repository)
        {
        }

        public override async Task<AdmissionPeriodDto> CreateAsync(
            AdmissionPeriodDto input)
        {
            try
            {
                Console.WriteLine("========== CREATE ADMISSION PERIOD ==========");
                Console.WriteLine($"Name: {input.Name}");
                Console.WriteLine($"SchoolYear: {input.SchoolYear}");
                Console.WriteLine($"StartDate: {input.StartDate}");
                Console.WriteLine($"EndDate: {input.EndDate}");
                Console.WriteLine($"IsActive: {input.IsActive}");

                var result = await base.CreateAsync(input);

                Console.WriteLine("========== CREATE SUCCESS ==========");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== CREATE ERROR ==========");
                Console.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}