using DeletarContatos.Domain.Responses;
using DeletarContatos.Service.Contato;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeletarContatos.Api.Controllers
{
    /// <summary>
    /// Controller responsável pelo cadastro de contatos.
    /// </summary>
    [ApiController]
    [Route("deletar/contato")]
    public class ContatoController : Controller
    {
        private readonly IContatoService contatoService;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ContatoController"/>.
        /// </summary>
        /// <param name="contatoService">O serviço de cadastro de contatos.</param>
        public ContatoController(IContatoService contatoService)
        {
            this.contatoService = contatoService;
        }

        /// <summary>
        /// Exclui um contato pelo id.
        /// </summary>
        /// <param name="id">O id do contato a ser excluído.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        [SwaggerResponse(404, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            await contatoService.ExcluirContato(id);
            return Accepted();
        }
    }
}
