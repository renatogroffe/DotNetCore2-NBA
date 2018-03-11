using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace CargaDadosNBA
{
    public class Conferencia
    {
        public ObjectId _id { get; set; }
        public string Temporada { get; set; }
        public string Nome { get; set; }
        public DateTime DataCarga { get; set; }
        public List<Equipe> Equipes { get; set; } = new List<Equipe>();
    }

    public class Equipe
    {
        public int Posicao { get; set; }
        public string Nome { get; set; }
        public int Vitorias { get; set; }
        public int Derrotas { get; set; }
        public string PercentualVitorias { get; set; }
    }
}