// Models/Informacao.cs
namespace ProjetosDB.Models
{
    public class Informacao
    {
        public int Id { get; set; }
        public string Foto { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public string Relacionamento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Objetivo { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;

        // Propriedades de relacionamento
        public virtual Endereco? Endereco { get; set; }
        public virtual ICollection<Formacao> Formacoes { get; set; } = new List<Formacao>();
        public virtual ICollection<Experiencia> Experiencias { get; set; } = new List<Experiencia>();
        public virtual ICollection<Curso> Cursos { get; set; } = new List<Curso>();
    }
}