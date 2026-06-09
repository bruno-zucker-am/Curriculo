// Controllers/CurriculoController.cs
using Microsoft.AspNetCore.Mvc;
using ProjetosDB.Dto;
using ProjetosDB.Services;
using ProjetosDB.Utils;
namespace ProjetosDB.Controllers
{
    [ApiController]
    [Route("api/Curriculo")]
    public class CurriculoController : ControllerBase
    {
        private readonly CurriculoService _curriculoService;

        public CurriculoController(CurriculoService curriculoService)
        {
            _curriculoService = curriculoService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarCurriculo([FromBody] CurriculoDto curriculoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Salva no banco
            var curriculoCriado = await _curriculoService.CriarCurriculo(curriculoDto);

            // Transforma o DTO em PDF através do Layout
            var pdfBytes = CurriculoLayout.GerarPdf(curriculoCriado);

            // Remove os espaços do nome para gerar o arquivo: ex: "Curriculo_Joao_Silva.pdf"
            var nomeArquivo = $"Curriculo_{curriculoCriado.Informacao.Nome.Replace(" ", "_")}.pdf";

            // Retorna o arquivo e receberá um Blob
            return File(pdfBytes, "application/pdf", nomeArquivo);
        }
    }
}
