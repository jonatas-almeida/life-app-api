using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Life.Repository;
using Life.WebAPI.DTOs;
using Life.Domain;

namespace Life.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ConsultasController : ControllerBase{
        
        //Instância do repositório
        private readonly ILifeRepository _repo;

        //Instância do Mapeamento
        private readonly IMapper _mapper;


        //Construtor da Controller
        public ConsultasController(ILifeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        //Retorna todos as consultas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try{
                var consultas = await _repo.GetAllConsultaAsync(true);
                //Pega os valores específicos de EventoDto, já que ele está sendo passado como parâmetro no método Map. No caso em questão, ele retorna um array de valores json
                var results = _mapper.Map<ConsultaDto[]>(consultas);
                return Ok(results);
            }
            catch(System.Exception){
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados falhou!");
            }
        }


        [HttpGet("{EventoId}")]
        public async Task<IActionResult> GetAction(int ConsultaId)
        {
            try
            {
                var consulta = await _repo.GetConsultaAsyncById(ConsultaId, true);
                var results = _mapper.Map<ConsultaDto>(consulta);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados falhou!");
            }
        }


        [HttpGet("getByName/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var results = await _repo.GetAllConsultaAsyncByName(name, true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados falhou!");
            }
        }


        
        //FUNÇÕES DA CONTROLLER
        //-----------------------------------------------------------------------

        //Função de Inserir (Post)
        [HttpPost]
        public async Task<IActionResult> Post(ConsultaDto model)
        {
            try
            {
                var consulta = _mapper.Map<Consulta>(model);

                _repo.Add(consulta);

                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/consulta/{model.Id}", _mapper.Map<ConsultaDto>(consulta));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados falhou!");
            }

            return BadRequest();
        }
    }


}