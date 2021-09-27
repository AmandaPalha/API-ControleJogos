using ApiControleJogos.Exceptions;
using ApiControleJogos.ImputModel;
using ApiControleJogos.Services;
using ApiControleJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiControleJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual a página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. Mínimo 1 e máximo 50</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar jogo pelo seu Id
        /// </summary>
        /// <param name="idJogo">Id do Jogo buscado</param>
        /// <returns></returns>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok();
        }

        /// <summary>
        /// Insere novo jogo
        /// </summary>
        /// <param name="jogoImputModel">Informações sobre o jogo</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoImputModel jogoImputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoImputModel);

                return Ok(jogo);
            }
            
            catch(JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        /// <summary>
        /// Autalizar jogo
        /// </summary>
        /// <param name="idJogo">Id do jogo</param>
        /// <param name="jogoImputModel">Informações do jogo</param>
        /// <returns></returns>
        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoImputModel jogoImputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoImputModel);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe esse jogo");
            }
        }

        /// <summary>
        /// Atualizar jogo
        /// </summary>
        /// <param name="idJogo">Id do jogo</param>
        /// <param name="preco">Preço do jogo</param>
        /// <returns></returns>
        [HttpPatch("{idJogo:guid}/preco;{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe esse jogo");
            }
        }

        /// <summary>
        /// Deletar jogo
        /// </summary>
        /// <param name="idJogo">Id do jogo</param>
        /// <returns></returns>
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo(Guid idJogo)
        {
            try
                {
                    await _jogoService.Remover(idJogo);

                    return Ok();
                }
            catch (JogoNaoCadastradoException ex)
                {
                    return NotFound("Não existe este jogo");
                }
        }

    }
}
