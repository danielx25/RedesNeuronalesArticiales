using iTextSharp.text;
using iTextSharp.text.pdf;
using RedesNeuronalesArtificiales.AnalisisDeRNA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace RedesNeuronalesArtificiales.Reportes
{
    class ReportePrediccion
    {
        ConstruccionConjuntos[] resultados;
        ConstruccionConjuntos resultado;
        DateTime hoy = DateTime.Now;
        Chart graficoMP10;
        Chart graficoSinAlerta;
        Chart graficoAlerta1;
        Chart graficoAlerta2;
        Chart graficoAlerta3;
        Chart graficoAlerta4;
        iTextSharp.text.Font _standardFont;

        // Se crea el documento con el tamaño de página tradicional
        Document doc = new Document(PageSize.LETTER);
        // Indicamos donde vamos a guardar el documento
        PdfWriter writer;//= PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));


        public ReportePrediccion(ConstruccionConjuntos[] resultados, Chart grafico, Chart grafico1, Chart grafico2, Chart grafico3, Chart grafico4, Chart grafico5)
        {
            this.resultados = resultados;
            graficoMP10 = grafico;
            graficoSinAlerta = grafico1;
            graficoAlerta1 = grafico2;
            graficoAlerta2 = grafico3;
            graficoAlerta3 = grafico4;
            graficoAlerta4 = grafico5;

            doc = new Document(PageSize.LETTER);
        }

        public void crearReporte(string rutaArchivo)
        {
 
            writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));

            // Se le coloca el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Reporte de prediccion Mp10");
            doc.AddCreator("Daniel Barraza Cortes y Carlo Barraza Escalante.-");

            // Abrimos el archivo
            doc.Open();
            // Se crea el tipo de Font que vamos utilizar
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Se escribe el encabezamiento en el documento
            Paragraph titulo = new Paragraph("Reporte de predicción Mp10");
            titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(new Paragraph(titulo));
            doc.Add(Chunk.NEWLINE);

            var titleFont = FontFactory.GetFont("Courier", 8, BaseColor.BLACK);
            Paragraph fecha = new Paragraph("fecha: "+hoy.ToString(), titleFont);
            doc.Add(fecha);


            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            for(int indiceDia=0; indiceDia<resultados.Length; indiceDia++)
            {
                resultado = resultados[indiceDia];
                contruirGraficoMp10(indiceDia+1);
                //contruirGraficoMp10Pertenencia(indiceDia + 1);
                //contruirGraficoMp10Horas(indiceDia + 1);
            }
            
            doc.Close();
            writer.Close();
        }


        public void crearReporteDetallado(string rutaArchivo)
        {

            writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));

            // Se le coloca el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Reporte de prediccion Mp10");
            doc.AddCreator("Daniel Barraza Cortes y Carlo Barraza Escalante.-");

            // Abrimos el archivo
            doc.Open();
            // Se crea el tipo de Font que vamos utilizar
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Se escribe el encabezamiento en el documento
            Paragraph titulo = new Paragraph("Reporte de predicción Mp10");
            titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(new Paragraph(titulo));
            doc.Add(Chunk.NEWLINE);

            var titleFont = FontFactory.GetFont("Courier", 8, BaseColor.BLACK);
            Paragraph fecha = new Paragraph("fecha: " + hoy.ToString(), titleFont);
            doc.Add(fecha);


            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            for (int indiceDia = 0; indiceDia < resultados.Length; indiceDia++)
            {
                resultado = resultados[indiceDia];
                contruirGraficoMp10(indiceDia + 1);
                contruirGraficoMp10Pertenencia(indiceDia + 1);
                //contruirGraficoMp10Horas(indiceDia + 1);
            }
            tablaGrupos();
            graficosNivelesConfianza();
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            /*
            for (int indiceDia = 0; indiceDia < resultados.Length; indiceDia++)
            {
                resultado = resultados[indiceDia];
                //contruirGraficoMp10(indiceDia + 1);
                contruirGraficoMp10Pertenencia(indiceDia + 1);
                //contruirGraficoMp10Horas(indiceDia + 1);
            }
           */
            doc.Close();
            writer.Close();
        }

        private void contruirGraficoMp10(int indiceDia)
        {
            /*foreach (var series in graficoMP10.Series)
            {
                series.Points.Clear();
            }*/
            graficoMP10.Series.Clear();
            double mp10 = resultado.Mp10predecido;

            for (int indice_alerta = 0; indice_alerta < resultado.gruposGanadores.GetLength(0); indice_alerta++)
            {
                double[] vectorDistancia = new double[resultado.vectorDistanciaMP10.GetLength(1)];
                for (int indice_distancia = 0; indice_distancia < vectorDistancia.Length; indice_distancia++)
                {
                    vectorDistancia[indice_distancia] = resultado.vectorDistanciaMP10[indice_alerta, indice_distancia];
                }

                var serieMp10 = new Series(resultado.gruposGanadores[indice_alerta].clase);
                serieMp10.ChartType = SeriesChartType.Line;
                serieMp10.Points.DataBindXY(resultado.rangoMP10, vectorDistancia);

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "sin alerta") == 0)
                    serieMp10.Color = Color.Green;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 1") == 0)
                    serieMp10.Color = Color.Yellow;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 2") == 0)
                    serieMp10.Color = Color.Orange;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 3") == 0)
                    serieMp10.Color = Color.Purple;

                if (String.Compare(resultado.gruposGanadores[indice_alerta].clase, "alerta 4") == 0)
                    serieMp10.Color = Color.Red;

                graficoMP10.Series.Add(serieMp10);
            }
            var seriePuntoMinimo = new Series("punto minimo");
            seriePuntoMinimo.ChartType = SeriesChartType.Line;
            seriePuntoMinimo.Points.DataBindXY(new double[] { mp10, mp10 }, new double[] { 0, resultado.distanciaMinima });
            graficoMP10.Series.Add(seriePuntoMinimo);

            //textoNivelMp10.Text = mp10.ToString();


            double sinAlerta = 150;
            double alerta1 = 250;
            double alerta2 = 350;
            double alerta3 = 500;
            string cadenaAlerta = "Sin alerta";
            
            if (mp10 < sinAlerta)// sin alerta
                cadenaAlerta = "Sin alerta";
            if (sinAlerta <= mp10 && mp10 <= alerta1)
                cadenaAlerta = "Alerta 1";
            if (alerta1 < mp10 && mp10 <= alerta2)//alerta 2
                cadenaAlerta = "Alerta 2";
            if (alerta2 < mp10 && mp10 <= alerta3)//alerta 3
                cadenaAlerta = "Alerta 3";
            if (alerta3 < mp10)//alerta 4
                cadenaAlerta = "Alerta 4";


            Paragraph tituloGrafico = new Paragraph("Grafico de Mp10 dia " + indiceDia);
            doc.Add(tituloGrafico);
            var imagenGrafico = new MemoryStream();
            graficoMP10.SaveImage(imagenGrafico, ChartImageFormat.Png);
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(imagenGrafico.GetBuffer());
            imagen.ScalePercent(65f);
            imagen.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen);
            doc.Add(Chunk.NEWLINE);


            // Se crean las tablas (en este caso 3)
            PdfPTable tblPrueba = new PdfPTable(2);
            tblPrueba.WidthPercentage = 100;

            // Se configura el título de las columnas de la tabla
            PdfPCell clNombrePrimera = new PdfPCell(new Phrase("Tipo Alerta", _standardFont));
            clNombrePrimera.BorderWidth = 0;
            clNombrePrimera.BorderWidthBottom = 0.75f;

            PdfPCell clNombreSegunda = new PdfPCell(new Phrase("Nivel Mp10", _standardFont));
            clNombreSegunda.BorderWidth = 0;
            clNombreSegunda.BorderWidthBottom = 0.75f;

            //PdfPCell clNombreTercera = new PdfPCell(new Phrase("nombreTerceraTabla", _standardFont));
            //clNombreTercera.BorderWidth = 0;
            //clNombreTercera.BorderWidthBottom = 0.75f;

            // se añade las celdas a la tabla
            tblPrueba.AddCell(clNombrePrimera);
            tblPrueba.AddCell(clNombreSegunda);
            //tblPrueba.AddCell(clNombreTercera);

            // se llena la tabla con información
            clNombrePrimera = new PdfPCell(new Phrase(cadenaAlerta, _standardFont));
            clNombrePrimera.BorderWidth = 0;

            clNombreSegunda = new PdfPCell(new Phrase(mp10.ToString(), _standardFont));
            clNombreSegunda.BorderWidth = 0;

            //clNombreTercera = new PdfPCell(new Phrase("Info_Tabla3", _standardFont));
            //clNombreTercera.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombrePrimera);
            tblPrueba.AddCell(clNombreSegunda);
            //tblPrueba.AddCell(clNombreTercera);

            // Finalmente, se añade la tabla al documento PDF y se cierra el documento
            doc.Add(tblPrueba);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);





        }

        private void tablaGrupos()
        {
            /*
             * public int numeroActivacion;
                public double[] vector;
                public double media;
                public double desviacionEstandar;
                public double nivelPertenencia;
                public string clase ="SinEtiqueta";//tipo de clase en la SOM
                public bool etiquetada = false;
             */
            // Se crean las tablas (en este caso 3)

            Paragraph tituloGrafico = new Paragraph("Grupos de Sin Alerta");
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);
            agregarTabla(0);

            tituloGrafico = new Paragraph("Grupos de Alerta 1");
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);
            agregarTabla(1);

            tituloGrafico = new Paragraph("Grupos de Alerta 2");
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);
            agregarTabla(2);

            tituloGrafico = new Paragraph("Grupos de Alerta 3");
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);
            agregarTabla(3);

            tituloGrafico = new Paragraph("Grupos de Alerta 4");
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);
            agregarTabla(4);

            //tblPrueba.AddCell(clNombreTercera);
            /*
            // se llena la tabla con información
            clNombrePrimera = new PdfPCell(new Phrase(cadenaAlerta, _standardFont));
            clNombrePrimera.BorderWidth = 0;

            clNombreSegunda = new PdfPCell(new Phrase(mp10.ToString(), _standardFont));
            clNombreSegunda.BorderWidth = 0;

            //clNombreTercera = new PdfPCell(new Phrase("Info_Tabla3", _standardFont));
            //clNombreTercera.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombrePrimera);
            tblPrueba.AddCell(clNombreSegunda);
            //tblPrueba.AddCell(clNombreTercera);
            */
            // Finalmente, se añade la tabla al documento PDF y se cierra el documento

        }

        private void agregarTabla(int indiceClase)
        {
            PdfPTable tblPruebaAlerta1 = new PdfPTable(6);
            tblPruebaAlerta1.WidthPercentage = 100;

            // Se configura el título de las columnas de la tabla
            PdfPCell clNombrePrimera = new PdfPCell(new Phrase("Numero", _standardFont));
            clNombrePrimera.BorderWidth = 0;
            clNombrePrimera.BorderWidthBottom = 0.75f;

            PdfPCell clNombreSegunda = new PdfPCell(new Phrase("Etiqueta", _standardFont));
            clNombreSegunda.BorderWidth = 0;
            clNombreSegunda.BorderWidthBottom = 0.75f;

            PdfPCell clNombreTercera = new PdfPCell(new Phrase("total activación", _standardFont));
            clNombreTercera.BorderWidth = 0;
            clNombreTercera.BorderWidthBottom = 0.75f;

            PdfPCell clNombreCuarta = new PdfPCell(new Phrase("Media", _standardFont));
            clNombreCuarta.BorderWidth = 0;
            clNombreCuarta.BorderWidthBottom = 0.75f;

            PdfPCell clNombreQuinta = new PdfPCell(new Phrase("Desviación Estandar", _standardFont));
            clNombreQuinta.BorderWidth = 0;
            clNombreQuinta.BorderWidthBottom = 0.75f;

            PdfPCell clNombreSexta = new PdfPCell(new Phrase("Nivel Pertenecia", _standardFont));
            clNombreSexta.BorderWidth = 0;
            clNombreSexta.BorderWidthBottom = 0.75f;

            // se añade las celdas a la tabla

            tblPruebaAlerta1.AddCell(clNombrePrimera);
            tblPruebaAlerta1.AddCell(clNombreSegunda);
            tblPruebaAlerta1.AddCell(clNombreTercera);
            tblPruebaAlerta1.AddCell(clNombreCuarta);
            tblPruebaAlerta1.AddCell(clNombreQuinta);
            tblPruebaAlerta1.AddCell(clNombreSexta);

            for (int indice = 0; indice < resultado.gruposxclases.GetLength(1); indice++)
            {
                Grupo grupoAux = resultado.gruposxclases[indiceClase, indice];
                clNombrePrimera = new PdfPCell(new Phrase(indice.ToString(), _standardFont));
                clNombrePrimera.BorderWidth = 0;

                clNombreSegunda = new PdfPCell(new Phrase(grupoAux.clase, _standardFont));
                clNombreSegunda.BorderWidth = 0;

                clNombreTercera = new PdfPCell(new Phrase(grupoAux.numeroActivacion.ToString(), _standardFont));
                clNombreTercera.BorderWidth = 0;

                clNombreCuarta = new PdfPCell(new Phrase(grupoAux.media.ToString(), _standardFont));
                clNombreCuarta.BorderWidth = 0;

                clNombreQuinta = new PdfPCell(new Phrase(grupoAux.desviacionEstandar.ToString(), _standardFont));
                clNombreQuinta.BorderWidth = 0;

                clNombreSexta = new PdfPCell(new Phrase(grupoAux.nivelPertenencia.ToString(), _standardFont));
                clNombreSexta.BorderWidth = 0;

                tblPruebaAlerta1.AddCell(clNombrePrimera);
                tblPruebaAlerta1.AddCell(clNombreSegunda);
                tblPruebaAlerta1.AddCell(clNombreTercera);
                tblPruebaAlerta1.AddCell(clNombreCuarta);
                tblPruebaAlerta1.AddCell(clNombreQuinta);
                tblPruebaAlerta1.AddCell(clNombreSexta);
            }
            doc.Add(tblPruebaAlerta1);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
        }

        private void contruirGraficoMp10Pertenencia(int indiceDia)
        {
            /*foreach (var series in graficoMP10.Series)
            {
                series.Points.Clear();
            }*/

            ChartArea chartArea6 = new ChartArea();
            chartArea6.AxisX.Title = "Nivel Mp10";
            chartArea6.AxisY.Title = "Nivel Pertenencia";
            chartArea6.Name = "ChartArea1";
            Legend legend6 = new Legend();
            legend6.Name = "legend";
            Chart graficoPertenencia = new Chart();
            graficoPertenencia.BackColor = System.Drawing.Color.Silver;

            graficoPertenencia.ChartAreas.Add(chartArea6);
            graficoPertenencia.Name = "Legend1";
            graficoPertenencia.Legends.Add(legend6);
            graficoPertenencia.Location = new System.Drawing.Point(25, 21);
            graficoPertenencia.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            graficoPertenencia.Name = "graficoMP10";
            graficoPertenencia.Size = new System.Drawing.Size(1092, 402);
            graficoPertenencia.TabIndex = 0;
            graficoPertenencia.Text = "chart1";


            //graficoMP10.Series.Clear();
            double mp10 = resultado.Mp10predecido;


            var serieMp10 = new Series("Pertenencia");
            serieMp10.ChartType = SeriesChartType.Line;
            serieMp10.Points.DataBindXY(resultado.rangoMP10, resultado.vectorNivelPertenencia);

            graficoPertenencia.Series.Add(serieMp10);

            Paragraph tituloGrafico = new Paragraph("Grafico Pertenecia de alerta de Mp10 del dia "+indiceDia);
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);

            var imagenGrafico = new MemoryStream();
            graficoPertenencia.SaveImage(imagenGrafico, ChartImageFormat.Png);
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(imagenGrafico.GetBuffer());
            imagen.ScalePercent(50f);
            imagen.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
        }

        private void contruirGraficoMp10Horas(int indiceDia)
        {
            /*foreach (var series in graficoMP10.Series)
            {
                series.Points.Clear();
            }*/

            ChartArea chartArea6 = new ChartArea();
            chartArea6.AxisX.Title = "horas";
            chartArea6.AxisY.Title = "Nivel Mp10";
            chartArea6.Name = "ChartArea1";
            Legend legend6 = new Legend();
            legend6.Name = "legend";
            Chart graficoPertenencia = new Chart();
            graficoPertenencia.BackColor = System.Drawing.Color.Silver;

            graficoPertenencia.ChartAreas.Add(chartArea6);
            graficoPertenencia.Name = "Legend1";
            graficoPertenencia.Legends.Add(legend6);
            graficoPertenencia.Location = new System.Drawing.Point(25, 21);
            graficoPertenencia.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            graficoPertenencia.Name = "graficoMP10";
            graficoPertenencia.Size = new System.Drawing.Size(1092, 402);
            graficoPertenencia.TabIndex = 0;
            graficoPertenencia.Text = "chart1";


            //graficoMP10.Series.Clear();
            double mp10 = resultado.Mp10predecido;

            double[] horas = new double[24];
            for (int hora = 0; hora < 24; hora++)
                horas[hora] = hora;

                var serieMp10 = new Series("mp10");
            serieMp10.ChartType = SeriesChartType.Line;
            serieMp10.Points.DataBindXY(horas, resultado.vectorMp10Hora);//, resultado.vectorNivelPertenencia);

            graficoPertenencia.Series.Add(serieMp10);


            var imagenGrafico = new MemoryStream();
            graficoPertenencia.SaveImage(imagenGrafico, ChartImageFormat.Png);
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(imagenGrafico.GetBuffer());
            imagen.ScalePercent(50f);
            imagen.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen);
            doc.Add(Chunk.NEWLINE);
        }

        private void graficosNivelesConfianza()
        {
            Paragraph tituloGrafico = new Paragraph("Grafico Confianza Sin Alerta");
            doc.Add(tituloGrafico);
            doc.Add(Chunk.NEWLINE);

            var imagenGrafico = new MemoryStream();
            graficoSinAlerta.SaveImage(imagenGrafico, ChartImageFormat.Png);
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(imagenGrafico.GetBuffer());
            imagen.ScalePercent(65f);
            imagen.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            Paragraph tituloGrafico1 = new Paragraph("Grafico Confianza Alert 1");
            doc.Add(tituloGrafico1);
            doc.Add(Chunk.NEWLINE);

            var imagenGrafico1 = new MemoryStream();
            graficoAlerta1.SaveImage(imagenGrafico1, ChartImageFormat.Png);
            iTextSharp.text.Image imagen1 = iTextSharp.text.Image.GetInstance(imagenGrafico1.GetBuffer());
            imagen1.ScalePercent(65f);
            imagen1.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen1);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            Paragraph tituloGrafico2 = new Paragraph("Grafico Confianza Alert 2");
            doc.Add(tituloGrafico2);
            doc.Add(Chunk.NEWLINE);

            var imagenGrafico2 = new MemoryStream();
            graficoAlerta2.SaveImage(imagenGrafico2, ChartImageFormat.Png);
            iTextSharp.text.Image imagen2 = iTextSharp.text.Image.GetInstance(imagenGrafico2.GetBuffer());
            imagen2.ScalePercent(65f);
            imagen2.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen2);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            Paragraph tituloGrafico3 = new Paragraph("Grafico Confianza Alert 3");
            doc.Add(tituloGrafico3);
            doc.Add(Chunk.NEWLINE);

            var imagenGrafico3 = new MemoryStream();
            graficoAlerta3.SaveImage(imagenGrafico3, ChartImageFormat.Png);
            iTextSharp.text.Image imagen3 = iTextSharp.text.Image.GetInstance(imagenGrafico3.GetBuffer());
            imagen3.ScalePercent(65f);
            imagen3.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen3);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);

            Paragraph tituloGrafico4 = new Paragraph("Grafico Confianza Alert4");
            doc.Add(tituloGrafico4);
            doc.Add(Chunk.NEWLINE);

            var imagenGrafico4 = new MemoryStream();
            graficoAlerta4.SaveImage(imagenGrafico4, ChartImageFormat.Png);
            iTextSharp.text.Image imagen4 = iTextSharp.text.Image.GetInstance(imagenGrafico4.GetBuffer());
            imagen4.ScalePercent(65f);
            imagen4.Alignment = Element.ALIGN_CENTER;
            doc.Add(imagen4);
            doc.Add(Chunk.NEWLINE);

        }

    }
}
