using DeletarContatos.Domain.Requests;

namespace DeletarContatos.Service.Contato
{
    public interface IContatoService
    {
        Task AtualizarContato(ContatoRequest contato);
    }
}
