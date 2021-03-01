using AutoMapper;
using Life.Domain;
using Life.WebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Consulta, ConsultaDto>().ForMember(dest => dest.DoctorConsultas, opt => {
                opt.MapFrom(src => src.DoctorsConsultas.Select(x => x.Consulta).ToList());
               }).ReverseMap();

            //CONTINUAR DAQUI
            //Analisar as classes de Dtos de Médicos e Consultas
        }
    }
}
