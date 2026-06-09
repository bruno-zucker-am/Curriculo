// Models/Experiencia.cs
namespace ProjetosDB.Models
{
    public class Experiencia
    {
        public int Id { get; set; }
        public string Empresa { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public int AnoInicio { get; set; }
        public int AnoFim { get; set; }
        public string Atividades { get; set; } = string.Empty;

        // Relação com a classe Informacao
        public int InformacaoId { get; set; }
        public virtual Informacao Informacao { get; set; } = null!;
    }
}