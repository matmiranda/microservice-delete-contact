using DeletarContatos.Domain.Models.RabbitMq;

namespace DeletarContatos.Service.RabbitMq
{
    public interface IRabbitMqPublisherService
    {
        Task PublicarContatoAsync(ContactMessage contactMessage);
    }
}
