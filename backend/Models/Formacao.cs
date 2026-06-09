// Models/Formacao.cs
namespace ProjetosDB.Models
{
    public class Formacao
    {
        public int Id { get; set; }
        public string Curso { get; set; } = string.Empty;
        public string Instituicao { get; set; } = string.Empty;
        public int AnoConclusao { get; set; }

        // Relação com a classe Informacao
        public int InformacaoId { get; set; }
        public virtual Informacao Informacao { get; set; } = null!;
    }
}