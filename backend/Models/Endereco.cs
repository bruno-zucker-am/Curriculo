// ProjetosDB/Models/Endereco.cs
namespace ProjetosDB.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;

        // Relação com a entidade Informacao
        public int InformacaoId { get; set; }
        public virtual Informacao Informacao { get; set; } = null!;
    }
}