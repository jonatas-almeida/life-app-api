using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Life.WebAPI.DTOs
{
    public class ConsultaDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "Campo obrigat�rio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da consulta � obrigat�rio")]
        public string Nome { get; set; }

        [Required (ErrorMessage = "Campo obrigat�rio")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "A descri��o da consulta � obrigat�ria")]
        public string Descricao { get; set; }

        [Required (ErrorMessage = "A data da consulta deve ser definida")]
        public DateTime DataConsulta { get; set; }

        [Phone]
        public string Telefone { get; set; }

        public List<DoctorConsultaDto> DoctorConsultas { get; set; }
    }
}