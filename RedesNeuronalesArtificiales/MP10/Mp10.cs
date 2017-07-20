using System;

namespace RedesNeuronalesArtificiales
{
	public class Mp10
	{
		public static string obtenerMP10HTML(double[,] matrizPesos, int numeroFilaMatriz, int numeroColumnasMatriz)
		{
			string texto = "<table>";
			double valorMP10Normalizado = 0;
			double valorMP10Real = 0;
			int neuronaActual = 0;
			for (int x = 0; x < numeroFilaMatriz; x++)
			{
				texto += "<tr>";
				for (int y = 0; y < numeroColumnasMatriz; y++)
				{
					if (x == 0 && y == 0)
						texto += "<td style=' background: #cbffcb; width: 10px; height: 10px;font-size: 10px;text-align: center;'>" + "-" + "</td>";
					if (x == 0)
						texto += "<td style=' background: #cbffcb; width: 10px; height: 10px;font-size: 10px;text-align: center;'>" + (y + 1) + "</td>";
				}
				if (x == 0)
					texto += "</tr>\n";
				for (int y = 0; y < numeroColumnasMatriz; y++)
				{
					valorMP10Normalizado = matrizPesos[7, neuronaActual];
					valorMP10Real = valorMP10Normalizado * 800;
					if (y == 0)
						texto += "<td style=' background: #cbffcb; width: 10px; height: 10px;font-size: 10px;text-align: center;'>" + (x + 1) + "</td>";

					if (valorMP10Real == 0)
						texto += "<td style=' background: #cbffcb; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
					else if (valorMP10Real > 0 && valorMP10Real <= 150)
					{//Sin Alerta
						if (valorMP10Real > 0 && valorMP10Real <= 50)
							texto += "<td style=' background: #92ff92; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
						else if (valorMP10Real > 50 && valorMP10Real <= 100)
							texto += "<td style=' background: #40e040; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
						else if (valorMP10Real > 100 && valorMP10Real <= 150)
							texto += "<td style=' background: #16a716; width: 10px; height: 10px;font-size: 10px;text-align: center;'>0</td>";
					}
					else if (valorMP10Real > 150 && valorMP10Real <= 250)
					{//Alerta 1
						if (valorMP10Real > 150 && valorMP10Real <= 200)
							texto += "<td style=' background: #00ffff; width: 10px; height: 10px;font-size: 10px;text-align: center;'>1</td>";
						else if (valorMP10Real > 200 && valorMP10Real <= 250)
							texto += "<td style=' background: #0090ff; width: 10px; height: 10px;font-size: 10px;text-align: center;'>1</td>";
					}
					else if (valorMP10Real > 250 && valorMP10Real <= 350)
					{//Alerta 2
						if (valorMP10Real > 250 && valorMP10Real <= 300)
							texto += "<td style=' background: #0055ff; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>2</td>";
						else if (valorMP10Real > 300 && valorMP10Real <= 350)
							texto += "<td style=' background: #0000ff; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>2</td>";
					}
					else if (valorMP10Real > 350 && valorMP10Real <= 500)
					{//Alerta 3
						if (valorMP10Real > 350 && valorMP10Real <= 400)
							texto += "<td style=' background: #ffff00; width: 10px; height: 10px;font-size: 10px;text-align: center;'>3</td>";
						else if (valorMP10Real > 400 && valorMP10Real <= 450)
							texto += "<td style=' background: #ff7f00; width: 10px; height: 10px;font-size: 10px;text-align: center;'>3</td>";
						else if (valorMP10Real > 450 && valorMP10Real <= 500)
							texto += "<td style=' background: #ff4600; width: 10px; height: 10px;font-size: 10px;text-align: center;'>3</td>";
					}
					else if (valorMP10Real > 500)
					{//Alerta 4
						if (valorMP10Real > 500 && valorMP10Real <= 600)
							texto += "<td style=' background: #ff0000; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>4</td>";
						else if (valorMP10Real > 600 && valorMP10Real <= 700)
							texto += "<td style=' background: #b20000; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>4</td>";
						else if (valorMP10Real > 700)
							texto += "<td style=' background: #480000; width: 10px; height: 10px;font-size: 10px;text-align: center;color:#ffffff;'>4</td>";
					}
					neuronaActual++;
				}
				texto += "</tr>\n";
			}
			texto += "</table>";
			return texto;
		}
	}
}

