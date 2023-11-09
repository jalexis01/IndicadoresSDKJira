// See https://aka.ms/new-console-template for more information
using Atlassian.Jira;
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using System.Data;


DbConnector dbConnector = new DbConnector();

// Retrieve messages as JSON
//string messagesJson = dbConnector.GetMessagesAsJson();
//Console.WriteLine(messagesJson);

// Retrieve messages as string representation
//string messagesString = dbConnector.GetMessagesAsString();
//Console.WriteLine(messagesString);

JiraAccess jira = new JiraAccess();
var fechainicio = "2023-10-01";
var fechaFinal = "2023-11-02";
//List<ComponenteHV> listaIdComponentes = dbConnector.GetComponentesHV();

// Itera sobre cada identificador y llama al método GetComponenteHV
//foreach (var componente in listaIdComponentes)
//{


//    // Puedes hacer algo con el componente obtenido, por ejemplo, imprimir sus propiedades
//    if (componente.IdComponente != null)
//    {
//        jira.ExportComponenteToExcel(componente.IdComponente);
//        jira.ExportComponenteToExcel(componente.IdComponente);
//    }
//    else
//    {
//        Console.WriteLine($"No se encontró el componente con ID: {componente.IdComponente}");
//    }
//}
var componente = "9120-WA-OR-2";
var ticketsReales = jira.GetTikets(0, 0, null, null, componente);
List<TicketHV> tickets = jira.GetTicketHVs(0, 0, componente);
jira.ExportComponenteToExcel(componente);

jira.ExportTicketsToExcel(tickets);

// Llamar a la función para obtener la DataTable
//DataTable dataTable = jira.getEstaciones();

//List<TicketHV> tickets = jira.GetTicketHVs(0, 0, "9110-WA-OR-2");
//jira.ExportComponenteToExcel("9120-WA-OR-1");
//jira.ExportTicketsToExcel(tickets);
					
