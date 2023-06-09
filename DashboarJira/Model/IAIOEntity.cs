using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class IAIOEntity
    {
        public IAIOEntity(List<List<Ticket>> aIO_POR_PUERTA, double total_puertas)
        {
            AIO_POR_PUERTA = aIO_POR_PUERTA;
            this.total_puertas = total_puertas;
        }
        private List<List<Ticket>> AIO_POR_PUERTA { get; set; }
        private double total_puertas { get; set; }



        /*
         * INDICADOR DE TIEMPO ENTRE AVERÍAS AIO
         * IAIO = ƩPAIO/n
         * #AIO=0 => PAIO=100%
         * #AIO=1 => PAIO=90%
         * #AIO=2 => PAIO=40%
         * #AIO>=3 => PAIO=0
         */
        public double CalcularIndicadorIAIO()
        {
            double suma_pano = pano();
            double iaio = Convert.ToDouble(((total_puertas - (double)AIO_POR_PUERTA.Count) * 100 + (double)suma_pano) / total_puertas);
            return Math.Round(iaio,1);
        }

        public double pano() 
        {
            double suma_pano = 0.0;
            foreach (var pano in AIO_POR_PUERTA)
            {
                if (pano.Count == 0)
                {
                    suma_pano += 100;
                }
                else if (pano.Count == 1)
                {
                    suma_pano += 90;
                }
                else if (pano.Count == 2)
                {
                    suma_pano += 40;
                }
            }
            return suma_pano;

        }
    }
}
