using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class ICPMEntity
    {
        

        private List<Ticket> TicketTAP { get; set; }

        private List<Ticket> TicketAPE { get; set; }

        public ICPMEntity( List<Ticket> ticketTAP, List<Ticket> ticketAPE)
        {
           
            TicketTAP = ticketTAP;
            TicketAPE = ticketAPE;
        }

        public double CalcularIndicadorICPM()
        {
            double icpmCalculado = 0;
            if (TicketTAP.Count > 0)
            {
                icpmCalculado = (double)TicketAPE.Count / (double)TicketTAP.Count;
            }
            if (icpmCalculado != 0)
            {
                return icpmCalculado * 100;
            }
            else
            {
                return 0;
            }

        }
    }
}
