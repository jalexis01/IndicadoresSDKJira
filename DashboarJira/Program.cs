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
List<ComponenteHV> listaIdComponentes = dbConnector.GetComponentesHV();
string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
string logFilePath = Path.Combine(projectDirectory, "ErroresLog.txt");

foreach (var componente in listaIdComponentes)
{
    // Puedes hacer algo con el componente obtenido, por ejemplo, imprimir sus propiedades
    if (componente.IdComponente != null)
    {
        try
        {
            jira.ExportComponenteToExcel(componente.IdComponente);
            List<TicketHV> tickets = jira.GetTicketHVs(0, 0, componente.IdComponente);
            jira.ExportTicketsToExcel(tickets);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error al exportar el componente {componente.IdComponente}: {e.Message}");

            // Registra el error en el archivo de log
            LogError(logFilePath, $"Error al exportar el componente {componente.IdComponente}: {e.ToString()}");
        }
    }
    else
    {
        Console.WriteLine($"No se encontró el componente con ID: {componente.IdComponente}");
    }
}

// ...

static void LogError(string filePath, string errorMessage)
{
    try
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"[{DateTime.Now}] {errorMessage}");
            writer.WriteLine("------------------------------------------------------------");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al escribir en el archivo de log: {ex.Message}");
    }
}

//var componente = "9120-WA-OR-2";
//var ticketsReales = jira.GetTikets(0, 0, null, null, componente);
//List<TicketHV> tickets = jira.GetTicketHVs(0, 0, componente);
//jira.ExportComponenteToExcel(componente);

//jira.ExportTicketsToExcel(tickets);

// Llamar a la función para obtener la DataTable
//DataTable dataTable = jira.getEstaciones();

//List<TicketHV> tickets = jira.GetTicketHVs(0, 0, "9110-WA-OR-2");
//jira.ExportComponenteToExcel("9120-WA-OR-1");
//jira.ExportTicketsToExcel(tickets);

