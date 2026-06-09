// Importa o namespace para usar as anotações de validação de dados
using System.ComponentModel.DataAnnotations;

//
namespace ProjetosDB.Dto
{
    // Define a classe InformacaoDto que representa os dados de informação pessoal para transferência de dados
    public class ExperienciaDto
    {
        // Propriedade Id para identificar cada registro de experiência profissional
        public int Id { get; set; }

        // Propriedade Empresa para armazenar o nome da empresa onde a experiência profissional foi realizada
        public string Empresa { get; set; } = string.Empty;

        // Propriedade Cargo para armazenar o nome do cargo ocupado na experiência profissional
        public string Cargo { get; set; } = string.Empty;

        // Propriedade Inicio para armazenar o ano de início da experiência profissional
        public int AnoInicio { get; set; }

        // Propriedade Fim para armazenar o ano de término da experiência profissional
        public int AnoFim { get; set; }

        // Propriedade Atividades para armazenar as atividades desenvolvidas na experiência profissional
        public string Atividades { get; set; } = string.Empty;
    }
}