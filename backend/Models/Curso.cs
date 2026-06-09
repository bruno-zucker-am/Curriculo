// Models/Curso.cs
namespace ProjetosDB.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string NomeCurso { get; set; } = string.Empty;
        public string InstituicaoCurso { get; set; } = string.Empty;
        public int AnoConclusaoCurso { get; set; }

        // Relação com a entidade Informacao
        public int InformacaoId { get; set; }
        public virtual Informacao Informacao { get; set; } = null!;
    }
}