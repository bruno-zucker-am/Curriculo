// Utils/CurriculoLayout.cs

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using ProjetosDB.Dto;

namespace ProjetosDB.Utils
{
    public static class CurriculoLayout
    {
        public static byte[] GerarPdf(CurriculoDto curriculo)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.2f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    
                    page.DefaultTextStyle(x => x.FontSize(10.5f).FontFamily(Fonts.Arial).FontColor(Colors.Black));

                    // Mapeia o Layout da Página de forma que o Cabeçalho NÃO se repita nas próximas páginas
                    page.Content().Column(contentCol =>
                    {
                        // Renderiza a foto e dados principais no topo
                        contentCol.Item().Element(x => ComposeHeader(x, curriculo));

                        // Renderiza o restante das seções do currículo logo abaixo
                        contentCol.Item().Element(x => ComposeContent(x, curriculo));
                    });

                    // Rodapé, repete em todas as páginas informando o número da página
                    page.Footer().AlignCenter().PaddingTop(10).Text(x =>
                    {
                        x.Span("Página ").FontSize(9).FontColor(Colors.Grey.Medium);
                        x.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                        x.Span(" de ").FontSize(9).FontColor(Colors.Grey.Medium);
                        x.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
                    });
                });
            }).GeneratePdf(); 
        }

        // Desenho das seções
        static void ComposeHeader(IContainer container, CurriculoDto curriculo)
        {
            container.PaddingBottom(10).Row(row =>
            {
                // Coluna da Foto Alinhada à esquerda
                if (!string.IsNullOrEmpty(curriculo.Informacao.Foto))
                {
                    var fotoBytes = ConvertBase64ToBytes(curriculo.Informacao.Foto);
                    if (fotoBytes != null)
                    {
                        row.ConstantItem(85).Image(fotoBytes).FitArea();
                    }
                }

                // Coluna dos Dados Centralizados
                row.RelativeItem().AlignMiddle().Column(column =>
                {
                    column.Item().AlignCenter().Text($"{curriculo.Informacao.Nome}").FontSize(24).Bold();

                    if (curriculo.Endereco != null)
                    {
                        // "Rua X, Número Y, Bairro: Z"
                        var enderecoLinha1 = $"Rua: {curriculo.Endereco.Rua}, Número: {curriculo.Endereco.Numero}, Bairro: {curriculo.Endereco.Bairro}";
                        
                        column.Item().PaddingTop(2).AlignCenter().Text(enderecoLinha1).FontSize(11);
                        
                        // Cidade/Estado
                        column.Item().AlignCenter().Text($"Cidade: {curriculo.Endereco.Cidade} - Estado: {curriculo.Endereco.Estado}").FontSize(11);
                    }

                    // Contatos
                    var contatos = string.Empty;
                    if (!string.IsNullOrEmpty(curriculo.Informacao.Telefone)) contatos += curriculo.Informacao.Telefone;
                    if (!string.IsNullOrEmpty(curriculo.Informacao.Email))
                    {
                        if (contatos.Length > 0) contatos += " | ";
                        contatos += curriculo.Informacao.Email;
                    }
                    
                    if (!string.IsNullOrEmpty(contatos))
                    {
                        column.Item().PaddingTop(2).AlignCenter().Text(contatos).FontSize(11);
                    }
                });
            });
        }

        // Converte uma string Base64 ou Data URL para um array de bytes, retornando null se a conversão falhar
        static byte[]? ConvertBase64ToBytes(string base64OrDataUrl)
        {
            try
            {
                var base64 = base64OrDataUrl;
                var commaIndex = base64OrDataUrl.IndexOf(',');
                if (commaIndex >= 0)
                {
                    base64 = base64OrDataUrl[(commaIndex + 1)..];
                }
                return Convert.FromBase64String(base64);
            }
            catch
            {
                return null;
            }
        }

        // Valores dimensionados
        static void ComposeContent(IContainer container, CurriculoDto curriculo)
        {
            container.Column(column =>
            {
                column.Spacing(4); // Espaçamento entre as seções

                // Informação Pessoal
                if (curriculo.Informacao.Idade > 0)
                {
                    ComposeSectionTitle(column, "Informação Pessoal");
                    column.Item().PaddingLeft(4).PaddingTop(2).Row(row =>
                    {
                        row.ConstantItem(130).Text("• Idade");
                        row.ConstantItem(15).Text(":");
                        row.RelativeItem().Text($"{curriculo.Informacao.Idade} anos");
                    });

                    // Relacionamento / Estado Civil
                    if (!string.IsNullOrWhiteSpace(curriculo.Informacao.Relacionamento))
                    {
                        column.Item().PaddingLeft(4).PaddingTop(2).Row(row =>
                        {
                            row.ConstantItem(130).Text("• Estado Civil");
                            row.ConstantItem(15).Text(":");
                            row.RelativeItem().Text($"{curriculo.Informacao.Relacionamento}");
                        });
                    }
                }

                // Formação Educacional
                if (curriculo.Formacoes != null && curriculo.Formacoes.Count > 0)
                {
                    ComposeSectionTitle(column, "Formação Educacional");
                    foreach (var formacao in curriculo.Formacoes)
                    {
                        column.Item().PaddingLeft(4).PaddingTop(2).Column(formacaoCol =>
                        {
                            formacaoCol.Item().Row(row =>
                            {
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("• ").Bold();
                                    t.Span($"{formacao.Instituicao}").Bold();
                                });
                                row.AutoItem().Text($"{formacao.AnoConclusao}").Bold();
                            });
                            formacaoCol.Item().PaddingLeft(12).Text($"{formacao.Curso}");
                        });
                    }
                }

                // Objetivo Profissional
                if (!string.IsNullOrEmpty(curriculo.Informacao.Objetivo))
                {
                    ComposeSectionTitle(column, "Objetivo Profissional");
                    column.Item().PaddingLeft(4).PaddingTop(2).Text($"{curriculo.Informacao.Objetivo}").Justify();
                }

                // Cursos De Qualificação
                if (curriculo.Cursos != null && curriculo.Cursos.Count > 0)
                {
                    ComposeSectionTitle(column, "Cursos De Qualificação");
                    foreach (var curso in curriculo.Cursos)
                    {
                        var inst = string.IsNullOrEmpty(curso.InstituicaoCurso) ? "" : $" - {curso.InstituicaoCurso}";
                        var anoStr = $"{curso.AnoConclusaoCurso}";
                        var ano = (string.IsNullOrWhiteSpace(anoStr) || anoStr == "0") ? "" : $" ({anoStr})";
                        
                        column.Item().PaddingLeft(4).PaddingTop(2).Text($"• {curso.NomeCurso}{inst}{ano}");
                    }
                }

                // Experiência Profissional
                if (curriculo.Experiencias != null && curriculo.Experiencias.Count > 0)
                {
                    ComposeSectionTitle(column, "Experiência Profissional");
                    foreach (var exp in curriculo.Experiencias)
                    {
                        column.Item().PaddingLeft(4).PaddingTop(4).Column(expCol =>
                        {
                            expCol.Item().Row(row =>
                            {
                                row.RelativeItem().Text(t =>
                                {
                                    t.Span("• ").Bold();
                                    t.Span($"{exp.Empresa}").Bold();
                                });
                                
                                var inicio = $"{exp.AnoInicio}";
                                var fim = $"{exp.AnoFim}";
                                var dataFormatada = (string.IsNullOrWhiteSpace(fim) || fim == "0") ? inicio : $"{inicio} - {fim}";
                                
                                row.AutoItem().Text(dataFormatada);
                            });
                            
                            expCol.Item().PaddingLeft(12).PaddingTop(1).Text($"{exp.Cargo}");
                            expCol.Item().PaddingLeft(12).PaddingTop(1).Text($"{exp.Atividades}").Justify();
                        });
                    }
                }

                // Perfil Profissional
                if (!string.IsNullOrEmpty(curriculo.Informacao.Perfil))
                {
                    ComposeSectionTitle(column, "Perfil Profissional");
                    column.Item().PaddingLeft(4).PaddingTop(2).Text($"{curriculo.Informacao.Perfil}").Justify();
                }
            });
        }

        // Desenha os títulos de seção
        static void ComposeSectionTitle(ColumnDescriptor column, string title)
        {
            column.Item()
                 // Espaçamento antes do título
                .PaddingTop(5) 
                // Aplica o fundo cinza claro corrido
                .Background(Colors.Grey.Lighten4) 
                // Aplica um padding interno para destacar o título
                .PaddingVertical(4)
                // Aplica um recuo à esquerda para destacar o título
                .PaddingLeft(6)
                // Escreve o título em negrito e um pouco maior que o texto normal
                .Text(title)
                // Configura o estilo do título: fonte um pouco maior e em negrito
                .FontSize(11.5f)
                // Deixa o título em negrito para destacá-lo do restante do texto
                .Bold();
        }
    }
}