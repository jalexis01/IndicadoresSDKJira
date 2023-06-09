namespace DashboarJira.Model
{
    public class IANOEntity
    {


        private double totalPuertas { get; set; }
        private List<List<Ticket>> ANIO_POR_PUERTA { get; set; }



        public IANOEntity(List<List<Ticket>> ANIO_POR_PUERTA, double totalPuertas)
        {

            this.totalPuertas = totalPuertas;
            this.ANIO_POR_PUERTA = ANIO_POR_PUERTA;
        }

        public double CalcularIndicadorIANO()
        {
            double suma_pano = pano();
            double iano = (((totalPuertas - (double)ANIO_POR_PUERTA.Count) + (double)suma_pano) / totalPuertas) * 100;
            return iano;
        }

        public double pano()
        {
            double suma_pano = 0.0;
            foreach (var pano in ANIO_POR_PUERTA)
            {
                suma_pano += 1 - ((double)pano.Count / 30.0);
            }
            Console.WriteLine("suma pano " + suma_pano);
            return suma_pano;

        }
    }
}
