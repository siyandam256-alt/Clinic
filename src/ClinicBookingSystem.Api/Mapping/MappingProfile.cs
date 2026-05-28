using AutoMapper;
using ClinicBookingSystem.Shared.DTOs;
using ClinicBookingSystem.Shared.Models;

namespace ClinicBookingSystem.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient Mappings
        CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<CreatePatientDto, Patient>();
        CreateMap<UpdatePatientDto, Patient>();

        // Clinic Mappings
        CreateMap<Clinic, ClinicDto>().ReverseMap();
        CreateMap<CreateClinicDto, Clinic>();

        // Provider Mappings
        CreateMap<Provider, ProviderDto>().ReverseMap();
        CreateMap<CreateProviderDto, Provider>();

        // TimeSlot Mappings
        CreateMap<TimeSlot, TimeSlotDto>().ReverseMap();
        CreateMap<CreateTimeSlotDto, TimeSlot>();

        // Appointment Mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        CreateMap<CreateAppointmentDto, Appointment>();
        CreateMap<UpdateAppointmentDto, Appointment>();

        // AppointmentNote Mappings
        CreateMap<AppointmentNote, AppointmentNoteDto>().ReverseMap();
        CreateMap<CreateAppointmentNoteDto, AppointmentNote>();
    }
}
