using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TerminosApi.DTOs;

namespace TerminosApi.Services;

public class PdfReportService
{
    public byte[] GenerarReporte(ReportePdfDto reporte)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Content()
                    .Column(column =>
                    {
                        DibujarEncabezado(column, reporte);

                        foreach (var documento in reporte.Documentos)
                        {
                            DibujarDocumento(column, documento);
                        }
                    });
            });
        })
        .GeneratePdf();
    }

    private void DibujarEncabezado(
    ColumnDescriptor column,
    ReportePdfDto reporte)
    {
        column.Item()
            .Background("#18453c")
            .Padding(12)
            .Column(c =>
            {
                c.Item()
                    .AlignCenter()
                    .Text("PODER JUDICIAL DEL ESTADO DE OAXACA")
                    .FontColor(Colors.White)
                    .Bold()
                    .FontSize(18);

                c.Item()
                    .AlignCenter()
                    .Text("REPORTE DE ESCRITOS")
                    .FontColor(Colors.White)
                    .Bold()
                    .FontSize(14);
            });

        column.Item().PaddingVertical(15);

        column.Item()
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(10)
            .Column(c =>
            {
                c.Item().Text($"Instancia: {reporte.Instancia}");

                if (!string.IsNullOrWhiteSpace(reporte.Juzgado))
                    c.Item().Text($"Juzgado: {reporte.Juzgado}");

                if (!string.IsNullOrWhiteSpace(reporte.Sala))
                    c.Item().Text($"Sala: {reporte.Sala}");

                c.Item().Text($"Periodo: {reporte.FechaInicial} al {reporte.FechaFinal}");

                c.Item().Text($"Total de registros: {reporte.TotalRegistros}");
            });

        column.Item().PaddingBottom(15);
    }

    private void DibujarDocumento(
    ColumnDescriptor column,
    DocumentoReporteDto documento)
    {
        column.Item()
            .PaddingBottom(15)
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Column(card =>
            {
                // Encabezado
                card.Item()
                    .Background("#18453c")
                    .Padding(8)
                    .Text(documento.TipoEscrito ?? "")
                    .FontColor(Colors.White)
                    .Bold()
                    .FontSize(13);

                // Contenido
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
            table.Cell().Element(CeldaTitulo).Text(etiqueta1);
            table.Cell().Element(CeldaValor).Text(valor1);

            table.Cell().Element(CeldaTitulo).Text(etiqueta2);
            table.Cell().Element(CeldaValor).Text(valor2);
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
                DibujarSeccion(card, "PARTES", partes =>
{
    if (documento.Partes.Count == 0)
    {
        partes.Item().Text("Sin partes registradas.");
    }
    else
    {
        foreach (var parte in documento.Partes)
        {
            partes.Item().Text($"• {parte}");
        }
    }
});
                DibujarSeccion(card, "ANEXOS", anexos =>
                {
                    if (documento.Anexos.Count == 0)
                    {
                        anexos.Item().Text("Sin anexos.");
                    }
                    else
                    {
                        foreach (var anexo in documento.Anexos)
                        {
                            anexos.Item()
                                .Text($"• {anexo.Descripcion} ({anexo.Cantidad})");
                        }
                    }
                });

                DibujarSeccion(card, "OTROS ANEXOS", otros =>
                {
                    if (documento.OtrosAnexos.Count == 0)
                    {
                        otros.Item().Text("Sin otros anexos.");
                    }
                    else
                    {
                        foreach (var anexo in documento.OtrosAnexos)
                        {
                            otros.Item()
                                .Text($"• {anexo}");
                        }
                    }
                });

                DibujarSeccion(card, "OBSERVACIONES", obs =>
                {
                    obs.Item().Text(
                        string.IsNullOrWhiteSpace(documento.Observaciones)
                            ? "Sin observaciones."
                            : documento.Observaciones);
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
            .PaddingTop(10)
            .Background("#18453c")
            .Padding(5)
            .Text(titulo)
            .FontColor(Colors.White)
            .Bold()
            .FontSize(11);

        column.Item()
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(8)
            .Column(contenido);
    }
}