using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Life.WebAPI.DTOs
{
    public class ConsultaDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DataConsulta { get; set; }

        public string Phone { get; set; }

        public List<DoctorConsultaDto> DoctorConsultas { get; set; }
    }
}