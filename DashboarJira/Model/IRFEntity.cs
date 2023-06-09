namespace DashboarJira.Model
{
    public class IRFEntity
    {
        private List<ReporteFallasPorPuerta> fallasPorPuerta;
        private double total_puertas;
        private List<Ticket> tickets;



        public IRFEntity(List<ReporteFallasPorPuerta> fallasPorPuerta, double total_puertas, List<Ticket> tickets)
        {
            this.fallasPorPuerta = fallasPorPuerta;
            this.total_puertas = total_puertas;
            this.tickets = tickets;
        }

        public IRFEntity()
        {
        }

        public double getTotalPuertas()
        {
            return total_puertas;
        }

        public double calculoIRF()
        {
            double totalPuertasIRF = 0.0;
            double sumatoria = 0.0;
            foreach (ReporteFallasPorPuerta reporte in fallasPorPuerta)
            {
                totalPuertasIRF++;
                double NTA = 0.0;
                double NAR = 0.0;
                double calculo = 0.0;
                Console.WriteLine("Puerta: " + reporte.Puerta);
                foreach (FallaPorPuerta falla in reporte.Fallas)
                {

                    Console.WriteLine("Fallas Por puerta: " + falla.CodigoFalla);
                    NTA += Convert.ToDouble(falla.Cantidad);
                    double aux = Convert.ToDouble(falla.Cantidad) - 1.0;
                    NAR += aux;
                    Console.WriteLine("NTA: " + NTA + "\nNAR: " + NAR);


                }
                Console.WriteLine("fuera del for \nNTA: " + NTA + "\nNAR: " + NAR);
                if (NTA != 0)
                {
                    calculo = 1.0 - (NAR / NTA);
                }

                sumatoria += calculo;
                Console.WriteLine("Sumatoria dentro del for: " + calculo);
            }
            Console.WriteLine("Total Puertas: " + total_puertas + "\nPuertas IRF: " + totalPuertasIRF + "\nSumatoria: " + sumatoria);
            double indicadorIRF = (((total_puertas - totalPuertasIRF) + sumatoria) / total_puertas);
            return indicadorIRF;
        }
        public void pintar()
        {
            foreach (ReporteFallasPorPuerta reporte in fallasPorPuerta)
            {
                Console.WriteLine($"Puerta: {reporte.Puerta}");

                foreach (FallaPorPuerta falla in reporte.Fallas)
                {
                    Console.WriteLine($"   Código de falla: {falla.CodigoFalla}, Cantidad: {falla.Cantidad}");
                }

                Console.WriteLine();
            }

        }
    }
    public class ReporteFallasPorPuerta
    {
        public string Puerta { get; set; }
        public List<FallaPorPuerta> Fallas { get; set; }
    }

    public class FallaPorPuerta
    {
        public string CodigoFalla { get; set; }
        public int Cantidad { get; set; }
    }
}

