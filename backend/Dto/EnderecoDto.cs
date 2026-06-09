// Importa o namespace para usar as anotações de validação de dados
using System.ComponentModel.DataAnnotations;

//
namespace ProjetosDB.Dto
{
    // Define a classe InformacaoDto que representa os dados de informação pessoal para transferência de dados
    public class EnderecoDto
    {
        // Propriedade Id para identificar cada registro de endereço
        public int Id { get; set; }

        // Propriedade Rua para armazenar o nome da rua do endereço
        public string Rua { get; set; } = string.Empty;

        // Propriedade Numero para armazenar o número do endereço
        public string Numero { get; set; } = string.Empty;

        // Propriedade para o bairro do endereço
        public string Bairro { get; set; } = string.Empty;

        // Propriedade Cidade para armazenar o nome da cidade do endereço
        public string Cidade { get; set; } = string.Empty;

        // Propriedade Estado para armazenar o nome do estado do endereço
        public string Estado { get; set; } = string.Empty;

        // Propriedade Cep para armazenar o código postal do endereço
        public string Cep { get; set; } = string.Empty;
    }
}