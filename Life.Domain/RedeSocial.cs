namespace Life.Domain
{
    public class RedeSocial{
        public int Id { get; set; }

        public string Nome { get; set; }

        public string URL { get; set; }

        public int? MedicoId { get; set; }

        public Medico Medico {get;}
    }
}