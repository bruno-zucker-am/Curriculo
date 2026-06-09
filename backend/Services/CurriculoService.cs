// Services/CurriculoService.cs

using ProjetosDB.Data;
using ProjetosDB.Models;
using ProjetosDB.Dto;

namespace ProjetosDB.Services
{
    public class CurriculoService
    {
        private readonly AppDbContext _context;

        public CurriculoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CurriculoDto> CriarCurriculo(CurriculoDto curriculoDto)
        {
            // Cria e salva Informacao
            var informacao = new Informacao
            {
                Foto = curriculoDto.Informacao.Foto,
                Nome = curriculoDto.Informacao.Nome,
                Idade = curriculoDto.Informacao.Idade ?? 0,
                Telefone = curriculoDto.Informacao.Telefone,
                Relacionamento = curriculoDto.Informacao.Relacionamento,
                Email = curriculoDto.Informacao.Email,
                Objetivo = curriculoDto.Informacao.Objetivo,
                Perfil = curriculoDto.Informacao.Perfil
            };

            // Salva a Informacao para gerar o Id
            _context.Informacao.Add(informacao);
            await _context.SaveChangesAsync();

            // Cria e salva o Endereco: 1 para 1
            if (curriculoDto.Endereco != null)
            {
                var endereco = new Endereco
                {
                    Rua = curriculoDto.Endereco.Rua,
                    Numero = curriculoDto.Endereco.Numero,
                    Bairro = curriculoDto.Endereco.Bairro,
                    Cidade = curriculoDto.Endereco.Cidade,
                    Estado = curriculoDto.Endereco.Estado,
                    Cep = curriculoDto.Endereco.Cep,
                    InformacaoId = informacao.Id
                };
                _context.Endereco.Add(endereco);
            }

            // Cria e salva as Formacoes: 1 para Muitos
            foreach (var fDto in curriculoDto.Formacoes)
            {
                var formacao = new Formacao
                {
                    Curso = fDto.Curso,
                    Instituicao = fDto.Instituicao,
                    AnoConclusao = fDto.AnoConclusao,
                    InformacaoId = informacao.Id
                };
                _context.Formacao.Add(formacao);
            }

            // Cria e salva os Cursos: 1 para Muitos
            foreach (var cDto in curriculoDto.Cursos)
            {
                var curso = new Curso
                {
                    NomeCurso = cDto.NomeCurso,
                    InstituicaoCurso = cDto.InstituicaoCurso,
                    AnoConclusaoCurso = cDto.AnoConclusaoCurso,
                    InformacaoId = informacao.Id
                };
                _context.Curso.Add(curso);
            }

            // Cria e salva as Experiencias: 1 para Muitos
            foreach (var eDto in curriculoDto.Experiencias)
            {
                var experiencia = new Experiencia
                {
                    Empresa = eDto.Empresa,
                    Cargo = eDto.Cargo,
                    AnoInicio = eDto.AnoInicio,
                    AnoFim = eDto.AnoFim,
                    Atividades = eDto.Atividades,
                    InformacaoId = informacao.Id
                };
                _context.Experiencia.Add(experiencia);
            }

            // Salva todas as entidades filhas de uma vez
            await _context.SaveChangesAsync();

            return curriculoDto;
        }
    }
}