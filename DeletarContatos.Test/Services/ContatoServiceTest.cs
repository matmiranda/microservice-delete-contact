using DeletarContatos.Domain.Models.RabbitMq;
using DeletarContatos.Domain.Responses;
using DeletarContatos.Infrastructure.Exceptions;
using DeletarContatos.Service.Contato;
using DeletarContatos.Service.RabbitMq;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace DeletarContatos.Test.Services
{
    [TestFixture]
    public class ContatoServiceTest
    {
        private Mock<IRabbitMqPublisherService> _mockRabbitMq;
        private Mock<IConfiguration> _mockConfig;
        private Mock<HttpMessageHandler> _mockHttpHandler;
        private HttpClient _httpClient;
        private ContatoService _contatoService;

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _mockRabbitMq = new Mock<IRabbitMqPublisherService>();
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["ApiAzure:Key"]).Returns("fake-api-key");

            // Mock do HttpClient
            _mockHttpHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpHandler.Object);

            // Instanciar a classe de serviço com os mocks
            _contatoService = new ContatoService(_mockRabbitMq.Object, _httpClient, _mockConfig.Object);
        }

        [Test]
        public void ExcluirContato_ContatoNaoExiste_DeveLancarExcecao()
        {
            // Simula uma resposta 404 (contato não encontrado)
            var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            _mockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            var ex = Assert.ThrowsAsync<CustomException>(async () => await _contatoService.ExcluirContato(1));

            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Message, Is.EqualTo("O id do contato não existe."));
        }

        [Test]
        public async Task ExcluirContato_ContatoExiste_DevePublicarNoRabbitMq()
        {
            int contatoId = 123;
            var contatoResponse = new ContatoIdResponse { Id = contatoId, DDD = 11, Regiao = "4" };
            var jsonResponse = JsonSerializer.Serialize(contatoResponse);

            // Simula resposta 200 da API com o contato existente
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
            };
            _mockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            await _contatoService.ExcluirContato(contatoId);

            // Verifica se o contato foi publicado corretamente no RabbitMQ
            _mockRabbitMq.Verify(mq => mq.PublicarContatoAsync(It.Is<ContactMessage>(msg => msg.Id == contatoId)), Times.Once);
        }

    }
}
