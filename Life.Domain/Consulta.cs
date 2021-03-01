using System;
using System.Collections.Generic;
using Life.Domain;

namespace Life.Domain
{
    public class Consulta
    {

        public int Id {get; set;}

        public string Name {get; set;}

        public string Description {get; set;}

        public DateTime DataConsulta {get; set;}

        public string Phone {get; set;}

        public List<DoctorConsulta> DoctorsConsultas {get; set;}

    }
}
