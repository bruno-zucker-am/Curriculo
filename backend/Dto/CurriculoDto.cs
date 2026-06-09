// Dto/CurriculoDto.cs

using System.Collections.Generic;

// DTO para representar o currículo completo
namespace ProjetosDB.Dto
{
    public class CurriculoDto
    {
        public InformacaoDto Informacao { get; set; } = new InformacaoDto();
        public EnderecoDto Endereco { get; set; } = new EnderecoDto();
        public List<FormacaoDto> Formacoes { get; set; } = new List<FormacaoDto>();
        public List<CursoDto> Cursos { get; set; } = new List<CursoDto>();
        public List<ExperienciaDto> Experiencias { get; set; } = new List<ExperienciaDto>();
    }
}