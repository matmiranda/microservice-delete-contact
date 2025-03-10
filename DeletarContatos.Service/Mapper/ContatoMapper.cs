using DeletarContatos.Domain.Enum;
using DeletarContatos.Domain.Models.RabbitMq;
using DeletarContatos.Domain.Requests;

namespace DeletarContatos.Service.Mapper
{
    public static class ContatoMapper
    {
        public static ContactMessage ToContactMessage(ContatoRequest request, string regiao)
        {
            return new ContactMessage
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), regiao),
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            };
        }
    }
}
