using System;
using System.Collections.Generic;
using Life.Domain;

namespace Life.WebAPI.DTOs
{
    public class DoctorConsultaDto
    {
        public int DoctorId { get; set;}

        public Medico Medico { get; set; }

        public int ConsultaId { get; set;}

        public ConsultaDto ConsultaDto { get; set;}
    }
}