using Life.Domain;

namespace Life.Domain
{
    public class DoctorConsulta
    {
        public int DoctorId {get; set;}

        public Medico Medico {get; set;}

        public int ConsultaId {get; set;}

        public Consulta Consulta {get; set;}
    }
}