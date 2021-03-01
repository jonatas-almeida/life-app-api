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

    public class ConsultasController : ControllerBase {

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

        //RETORNANDO OS DADOS DAS CONSULTAS DA API
        //------------------------------------------------------------------------------------------------------------------

        //Retorna todos as consultas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try {
                var consultas = await _repo.GetAllConsultaAsync(true);
                //Pega os valores específicos de EventoDto, já que ele está sendo passado como parâmetro no método Map. No caso em questão, ele retorna um array de valores json
                var results = _mapper.Map<ConsultaDto[]>(consultas);
                return Ok(results);
            }
            catch (System.Exception) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados falhou!");
            }
        }


        //Faz o upload da imagem no Banco de Dados
        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try
            {
                //Arquivo
                var file = Request.Form.Files[0];

                //Diretório onde o arquivo será armazenado
                var folderName = Path.Combine("Resources/Images", "");

                //Combina o diretório onde será armazenado + o diretório da aplicação
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


                if (file.Length > 0)
                {
                    //Analisa o nome do arquivo
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;

                    //Se o nome do arquivo vier com espaços ou caracteres especiais, será substituído e depois criado um 'full path' (o caminho completo da imagem)
                    var fullpPath = Path.Combine(pathToSave, fileName.Replace("\"", "").Trim());

                    //Aqui o fullPath será referenciado para a criação do diretório do arquivo
                    using(var stream = new FileStream(fullpPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();

            }
            catch (System.Exception)
            {
                this.StatusCode(StatusCodes.Status500InternalServerError, "Falha ao tentar fazer o upload da imagem!");
            }

            return BadRequest();
        }


        //Retorna as consultas pelo ID
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


        //Retorna as consultas pelo nome
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

                if (await _repo.SaveChangesAsync())
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



        //Função Update
        [HttpPut("{ConsultaId}")]
        public async Task<IActionResult> Put(int ConsultaId, ConsultaDto model)
        {
            try
            {
                var consulta = await _repo.GetConsultaAsyncById(ConsultaId, false);if(consulta == null) return NotFound();

                _mapper.Map(model, consulta);
                _repo.Update(consulta);

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



        //Função delete
        [HttpDelete("{ConsultaId}")]
        public async Task<IActionResult> Delete(int ConsultaId)
        {
            try
            {
                var consulta = await _repo.GetConsultaAsyncById(ConsultaId, false);

                if(consulta == null)
                {
                    return NotFound();
                }

                _repo.Delete(consulta);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de Dados falhou!");
            }

            return BadRequest();
        }


    }


}