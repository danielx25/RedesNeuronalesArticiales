using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesNeuronalesArtificiales.Reportes
{
    class ReportePrediccion
    {
        public void crearReporte()
        {
            // Se crea el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"C:\Users\danie\Desktop\nombreDoc.pdf", FileMode.Create));

            // Se le coloca el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Mi primer PDF");
            doc.AddCreator("Ricardo Carrasco S.-");

            // Abrimos el archivo
            doc.Open();
            // Se crea el tipo de Font que vamos utilizar
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Se escribe el encabezamiento en el documento
            doc.Add(new Paragraph("Mi primer documento PDF"));
            doc.Add(Chunk.NEWLINE);

            // Se crean las tablas (en este caso 3)
            PdfPTable tblPrueba = new PdfPTable(3);
            tblPrueba.WidthPercentage = 100;

            // Se configura el título de las columnas de la tabla
            PdfPCell clNombrePrimera = new PdfPCell(new Phrase("nombrePrimeraTabla", _standardFont));
            clNombrePrimera.BorderWidth = 0;
            clNombrePrimera.BorderWidthBottom = 0.75f;

            PdfPCell clNombreSegunda = new PdfPCell(new Phrase("nombreSegundaTabla", _standardFont));
            clNombreSegunda.BorderWidth = 0;
            clNombreSegunda.BorderWidthBottom = 0.75f;

            PdfPCell clNombreTercera = new PdfPCell(new Phrase("nombreTerceraTabla", _standardFont));
            clNombreTercera.BorderWidth = 0;
            clNombreTercera.BorderWidthBottom = 0.75f;

            // se añade las celdas a la tabla
            tblPrueba.AddCell(clNombrePrimera);
            tblPrueba.AddCell(clNombreSegunda);
            tblPrueba.AddCell(clNombreTercera);

            // se llena la tabla con información
            clNombrePrimera = new PdfPCell(new Phrase("Info_Tabla1", _standardFont));
            clNombrePrimera.BorderWidth = 0;

            clNombreSegunda = new PdfPCell(new Phrase("Info_Tabla2", _standardFont));
            clNombreSegunda.BorderWidth = 0;

            clNombreTercera = new PdfPCell(new Phrase("Info_Tabla3", _standardFont));
            clNombreTercera.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombrePrimera);
            tblPrueba.AddCell(clNombreSegunda);
            tblPrueba.AddCell(clNombreTercera);

            // Finalmente, se añade la tabla al documento PDF y se cierra el documento
            doc.Add(tblPrueba);

            doc.Close();
            writer.Close();
        }
    }
}
