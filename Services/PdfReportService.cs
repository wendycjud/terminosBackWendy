using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TerminosApi.DTOs;

namespace TerminosApi.Services;

public class PdfReportService
{
    private const string VerdeInstitucional = "#18453c";
    private const string VerdeTarjeta = "#337a6c";
    private const string GrisEncabezado = "#F3F4F6";

    private const float FuenteTituloPrincipal = 12;
    private const float FuenteTituloTarjeta = 10;
    private const float FuenteTituloTabla = 10;
    private const float FuenteTexto = 9;

    private const float PaddingGrande = 10;
    private const float PaddingMedio = 8;
    private const float PaddingPequeno = 5;
    public byte[] GenerarReporte(ReportePdfDto reporte)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
{
    page.Margin(30);

    page.Header()
        .Column(column =>
        {
            column.Item()
    .ShowOnce()
    .Column(c =>
    {
        DibujarEncabezado(c, reporte);
    });
            column.Item()
    .SkipOnce()
    .Border(1)
    .BorderColor(Colors.Grey.Lighten2)
    .Padding(8)
    .Row(row =>
    {
        // Izquierda
        row.RelativeItem()
            .Column(left =>
            {
                left.Item().Text($"Instancia: {reporte.Instancia}").FontSize(FuenteTexto);

                if (!string.IsNullOrWhiteSpace(reporte.Juzgado))
                    left.Item().Text($"Juzgado: {reporte.Juzgado}").FontSize(FuenteTexto);

                if (!string.IsNullOrWhiteSpace(reporte.Sala))
                    left.Item().Text($"Sala: {reporte.Sala}").FontSize(FuenteTexto);
            });

        row.Spacing(20);

        // Derecha
        row.RelativeItem()
            .AlignRight()
            .Column(right =>
            {
                right.Item()
                    .AlignRight()
                    .Text($"Periodo: {reporte.FechaInicial} al {reporte.FechaFinal}")
                    .FontSize(FuenteTexto);
            });
    });
        });

    page.Content()
        .Column(column =>
        {
            for (int i = 0; i < reporte.Documentos.Count; i++)
            {
                DibujarDocumento(
                    column,
                    reporte.Documentos[i],
                    i + 1,
                    reporte.Documentos.Count);
            }
        });
    page.Footer()
.AlignRight()
.Text(text =>
{
    text.Span("Página ");
    text.CurrentPageNumber();
    text.Span(" de ");
    text.TotalPages();
});
});
        })
        .GeneratePdf();

    }

    private void DibujarEncabezado(
    ColumnDescriptor column,
    ReportePdfDto reporte)
    {
        var logo = File.ReadAllBytes("Resources/logo.png");

        column.Item()
            .Background(VerdeInstitucional)
            .Padding(PaddingGrande)
            .Row(row =>
            {
                // Logo
                row.ConstantItem(30)
                    .Height(30)
                    .Image(logo);

                // Espacio entre logo y texto
                row.ConstantItem(10);

                // Texto
                row.RelativeItem()
                    .Column(c =>
                    {
                        c.Item()
                            .AlignCenter()
                            .Text("PODER JUDICIAL DEL ESTADO DE OAXACA")
                            .FontColor(Colors.White)
                            .Bold()
                            .FontSize(FuenteTituloPrincipal);

                        c.Item()
                            .AlignCenter()
                            .Text("REPORTE DE ESCRITOS")
                            .FontColor(Colors.White)
                            .Bold()
                            .FontSize(FuenteTituloPrincipal);
                    });
            });

        column.Item().PaddingBottom(5);

        column.Item()
    .Border(1)
    .BorderColor(Colors.Grey.Lighten2)
    .Padding(10)
    .Row(row =>
    {
        // ===== Columna izquierda =====
        row.RelativeItem()
            .Column(left =>
            {
                left.Item().Text(text =>
{
    text.DefaultTextStyle(x => x.FontSize(FuenteTexto));

    text.Span("Instancia: ").Bold();
    text.Span(reporte.Instancia);
});

                if (!string.IsNullOrWhiteSpace(reporte.Juzgado))
                {
                    left.Item().Text(text =>
{
    text.DefaultTextStyle(x => x.FontSize(FuenteTexto));

    text.Span("Juzgado: ").Bold();
    text.Span(reporte.Juzgado);
});
                }

                if (!string.IsNullOrWhiteSpace(reporte.Sala))
                {
                    left.Item().Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(FuenteTexto));

                        text.Span("Sala: ").Bold();
                        text.Span(reporte.Sala);
                    });
                }
            });

        row.Spacing(20);

        // ===== Columna derecha =====
        row.RelativeItem()
            .Column(right =>
            {
                right.Item()
    .AlignRight()
    .Text(text =>
    {
        text.DefaultTextStyle(x => x.FontSize(FuenteTexto));

        text.Span("Periodo: ").Bold();
        text.Span($"{reporte.FechaInicial} al {reporte.FechaFinal}");
    });

                right.Item()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.DefaultTextStyle(x => x.FontSize(FuenteTexto));

                        text.Span("Total de registros: ").Bold();
                        text.Span(reporte.TotalRegistros.ToString());
                    });
            });
    });

        column.Item().PaddingBottom(15);
    }


    private void DibujarDocumento(
    ColumnDescriptor column,
    DocumentoReporteDto documento,
    int numero,
    int total)
    {

        column.Item()
    .ShowEntire()
    .PaddingBottom(20)
    .Border(1)
    .BorderColor(Colors.Grey.Lighten2)
    .Column(card =>
            {
                // Encabezado
                card.Item()
    .Background(GrisEncabezado)
    .Padding(6)
    .Text($"ESCRITO No. {numero} de {total}")
    .Bold()
    .FontSize(FuenteTituloTabla)
    .FontColor(Colors.Grey.Darken3);

                // Encabezado verde
                card.Item()
                    .Background(VerdeTarjeta)
                    .Padding(8)
                    .Text((documento.TipoEscrito ?? "").ToUpper(new System.Globalization.CultureInfo("es-MX")))
                    .FontColor(Colors.White)
                    .Bold()
                    .FontSize(FuenteTituloTabla);

                // Contenido
                card.Item()
    .Background("#F8F9FA")
    .PaddingHorizontal(10)
    .PaddingVertical(5)
    .Text("INFORMACIÓN GENERAL")
    .Bold()
    .FontSize(FuenteTituloTabla)
    .FontColor("#18453c");
                card.Item()
    .Padding(10)
    .Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.ConstantColumn(110);
            columns.RelativeColumn();

            columns.ConstantColumn(110);
            columns.RelativeColumn();
        });

        void Fila(string etiqueta1, string valor1,
                  string etiqueta2, string valor2)
        {
            table.Cell().Element(CeldaTitulo).Text(etiqueta1).Bold().FontSize(FuenteTituloTabla); ;
            table.Cell().Element(CeldaValor).Text(valor1).FontSize(FuenteTexto); ;

            table.Cell().Element(CeldaTitulo).Text(etiqueta2).Bold().FontSize(FuenteTituloTabla);
            table.Cell().Element(CeldaValor).Text(valor2).FontSize(FuenteTexto);
        }

        Fila("Folio", documento.Folio ?? "",
             "Fecha", documento.FechaRecepcion ?? "");

        Fila("Expediente", documento.Expediente ?? "",
             "Hora", documento.HoraRecepcion ?? "");

        if (!string.IsNullOrWhiteSpace(documento.Juzgado))
        {
            // Primera Instancia
            Fila("Juzgado", documento.Juzgado ?? "",
                 "Secretaría", documento.Secretaria ?? "");

            Fila("Recibió", documento.QuienRecibio ?? "",
                 "Fojas", documento.Fojas ?? "");

            Fila("Traslados", documento.Traslados ?? "",
                 "", "");
        }
        else
        {
            // Segunda Instancia
            Fila("Sala", documento.Sala ?? "",
                 "Toca", documento.Toca ?? "");

            Fila("Recibió", documento.QuienRecibio ?? "",
                 "Fojas", documento.Fojas ?? "");

            Fila("Traslados", documento.Traslados ?? "",
                 "", "");
        }
    });
                card.Item()
    .PaddingTop(5)
    .Row(row =>
    {
        row.RelativeItem()
            .Column(left =>
            {
                DibujarSeccion(left, "PARTES", partes =>
                {
                    if (documento.Partes.Count == 0)
                    {
                        partes.Item().Text("Sin partes registradas.").FontSize(FuenteTexto); ;
                    }
                    else
                    {
                        foreach (var parte in documento.Partes)
                        {
                            partes.Item().Text($"• {parte}").FontSize(FuenteTexto); ;
                        }
                    }
                });
            });

        row.Spacing(10);

        row.RelativeItem()
    .Column(right =>
    {
        DibujarSeccion(right, "ANEXOS", anexos =>
        {
            if (documento.Anexos.Count == 0)
            {
                anexos.Item().Text("Sin anexos.").FontSize(FuenteTexto); ;
            }
            else
            {
                foreach (var anexo in documento.Anexos)
                {
                    anexos.Item()
                        .Text($"• {anexo.Descripcion} ({anexo.Cantidad})")
                        .FontSize(FuenteTexto);
                }
            }
        });
    });
    });



                card.Item()
     .PaddingTop(5)
     .Row(row =>
     {
         row.RelativeItem()
             .Column(left =>
             {
                 DibujarSeccion(left, "OBSERVACIONES", obs =>
                 {
                     obs.Item().Text(
                         string.IsNullOrWhiteSpace(documento.Observaciones)
                             ? "Sin observaciones."
                             : documento.Observaciones)
                         .FontSize(FuenteTexto);
                 });
             });

         row.Spacing(10);

         row.RelativeItem()
             .Column(right =>
             {
                 DibujarSeccion(right, "OTROS ANEXOS", otros =>
                 {
                     if (documento.OtrosAnexos.Count == 0)
                     {
                         otros.Item().Text("Sin otros anexos.").FontSize(FuenteTexto);
                     }
                     else
                     {
                         foreach (var anexo in documento.OtrosAnexos)
                         {
                             otros.Item().Text($"• {anexo}").FontSize(FuenteTexto);
                         }
                     }
                 });
             });
     });


            });

    }
    private IContainer CeldaTitulo(IContainer container)
    {
        return container
            .Background("#F3F4F6")
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5);
    }

    private IContainer CeldaValor(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5);
    }
    private void DibujarSeccion(
        ColumnDescriptor column,
        string titulo,
        Action<ColumnDescriptor> contenido)
    {
        column.Item()
            .PaddingTop(8)
            .Background(VerdeTarjeta)
            .Padding(5)
            .Text(titulo)
            .FontColor(Colors.White)
            .Bold()
            .FontSize(FuenteTituloTabla);

        column.Item()
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(8)
            .Column(contenido);
    }
}