using DeletarContatos.Domain.Requests;

namespace DeletarContatos.Service.Contato
{
    public interface IContatoService
    {
        Task ExcluirContato(int id);
    }
}
