using AutoMapper;
using MyProject.Admissions.Dto;

namespace MyProject.Admissions
{
    public class AdmissionPeriodMapProfile : Profile
    {
        public AdmissionPeriodMapProfile()
        {
            CreateMap<AdmissionPeriod, AdmissionPeriodDto>();
            CreateMap<AdmissionPeriodDto, AdmissionPeriod>();
        }
    }
}