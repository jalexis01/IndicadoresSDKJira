using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class TicketHV : Ticket
    {
        public List<Attachment>? Attachments {  get; set; }
    }
}
