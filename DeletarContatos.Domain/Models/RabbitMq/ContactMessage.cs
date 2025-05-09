﻿using DeletarContatos.Domain.Enum;

namespace DeletarContatos.Domain.Models.RabbitMq
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public int DDD { get; set; }
        public RegiaoEnum Regiao { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Action { get; } = "DELETE";
    }
}
