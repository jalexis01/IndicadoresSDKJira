using System.Text;

namespace DashboarJira.Model
{
    public class RANOEntity
    {
        //TAN: # total de tickets con nivel de falla igual ANIO, ya sean abiertos o cerrados del periodo.
        //TCN: # total de tickets con nivel de falla igual ANIO, cerrados en un intervalo menor o igual a 24 horas.
        public List<Ticket> TicketTAN { get; set; }

        public List<Ticket> TicketTCN { get; set; }


        public RANOEntity(List<Ticket> TicketTAN, List<Ticket> TicketTCN)
        {
            this.TicketTAN = TicketTAN;
            this.TicketTCN = TicketTCN;
        }
        public double CalcularIndicadorRANO()
        {
            double RANOGeneral;
            if (TicketTAN.Count != 0)
            {
                RANOGeneral = ((double)TicketTAN.Count / (double)TicketTCN.Count) * 100;
            }
            else
            {
                RANOGeneral = 100;
            }
            return Math.Round(RANOGeneral,1);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("TicketTAN: ");
            sb.Append(TicketTAN.Count);
            /*
            foreach (var ticket in TicketTAN)
            {
                sb.Append(ticket.id_ticket);
                sb.Append(", ");
            }*/
            sb.Append("TicketTCN: ");
            sb.Append(TicketTCN.Count);
            /*foreach (var ticket in TicketTCN)
            {
                sb.Append(ticket.id_ticket);
                sb.Append(", ");
            }*/
            return sb.ToString();
        }
    }
}
