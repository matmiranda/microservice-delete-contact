﻿using System.Text.Json.Serialization;

namespace DeletarContatos.Domain.Responses
{
    public class ContatoIdResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nome")]
        public string? Nome { get; set; }
        [JsonPropertyName("telefone")]
        public string? Telefone { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("ddd")]
        public int DDD { get; set; }
        [JsonPropertyName("regiao")]
        public string? Regiao { get; set; }
    }
}
