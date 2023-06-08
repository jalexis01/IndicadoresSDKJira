using Newtonsoft.Json;

namespace DashboarJira.Model
{
    public class RAIOEntity
    {

        //TAI: # total de tickets con nivel de falla igual AIO.
        //TCI: # total de tickets con nivel de falla igual AIO, cerrados en un intervalo menor o igual a 6 horas.
        public List<Ticket> TicketTCI { get; set; }
        public List<Ticket> TicketTAI { get; set; }

        public RAIOEntity(List<Ticket> TicketTCI, List<Ticket> TicketTAI)
        {
            this.TicketTCI = TicketTCI;
            this.TicketTAI = TicketTAI;
        }


        public double CacularIndicadorRAIO()
        {

            double RAIOGeneral = 0.0;
            if (TicketTAI.Count != 0)
            {
                RAIOGeneral = ((double)TicketTCI.Count / (double)TicketTAI.Count) * 100;

            }
            else
            {
                RAIOGeneral = 100;
            }
            return Math.Round(RAIOGeneral, 1);
        }
        public override string ToString()
        {
            string ticketTCIJson = JsonConvert.SerializeObject(TicketTCI);
            string ticketTAIJson = JsonConvert.SerializeObject(TicketTAI);

            return JsonConvert.SerializeObject(new
            {
                RAIO = CacularIndicadorRAIO(),
                TotalTai = TicketTAI.Count,
                TotalTCI = TicketTCI.Count,
                TicketTCI = TicketTCI,
                Espacio = "ESPACIO#######################",
                TicketTAI = TicketTAI,

            });
        }
    }
}
