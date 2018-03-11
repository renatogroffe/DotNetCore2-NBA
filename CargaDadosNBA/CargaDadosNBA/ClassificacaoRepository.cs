using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CargaDadosNBA
{
    public class ClassificacaoRepository
    {
        private MongoClient _client;
        private IMongoDatabase _db;

        public ClassificacaoRepository(
            IConfiguration configuration)
        {
            _client = new MongoClient(
                configuration.GetConnectionString("BaseNBA"));
            _db = _client.GetDatabase("NBA");
        }

        public void Incluir(List<Conferencia> conferencias)
        {
            _db.DropCollection("Classificacao");
            var classificacaoNBA =
                _db.GetCollection<Conferencia>("Classificacao");
            classificacaoNBA.InsertMany(conferencias);
        }
    }
}