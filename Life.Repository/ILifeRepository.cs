using System.Threading.Tasks;
using Life.Domain;

namespace Life.Repository
{
    public interface ILifeRepository
    {
        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveChangesAsync();

        
        //CONSULTAS
        Task<Consulta[]> GetAllConsultaAsyncByName(string name, bool includeDoctors);

        Task<Consulta[]> GetAllConsultaAsync(bool includeDoctors);

        Task<Consulta[]> GetConsultaAsyncById(int ConsultaId, bool includeDoctors);


        //MÉDICOS
        Task<Medico[]> GetAllMedicosAsyncByName(string name, bool includeConsultas);

        Task<Medico> GetMedicoAsync(int DoctorId, bool includeConsultas);
    }
}
