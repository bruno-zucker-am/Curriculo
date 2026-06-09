// Importa o namespace para usar as anotações de validação de dados
using System.ComponentModel.DataAnnotations;

namespace ProjetosDB.Dto
{
    // Define a classe InformacaoDto que representa os dados de informação pessoal para transferência de dados
    public class CursoDto
    {
        // Propriedade Curso para armazenar o nome do curso
        public string NomeCurso { get; set; } = string.Empty;

        // Propriedade Instituicao para armazenar o nome da instituição onde o curso foi realizado
        public string InstituicaoCurso { get; set; } = string.Empty;

        // Propriedade Ano para armazenar o ano de conclusão do curso
        public int AnoConclusaoCurso { get; set; }
    }
}