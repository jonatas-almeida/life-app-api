namespace Life.Domain
{
    public class Contato
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public int? ConsultaId { get; set; }

        public Consulta Consulta { get; }

        public int? MedicoId { get; set; }

        public Medico Medico { get; }
    }
}