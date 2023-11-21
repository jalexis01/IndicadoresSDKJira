// See https://aka.ms/new-console-template for more information
using Atlassian.Jira;
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.Extensions.Logging;
using System.Data;


DbConnector dbConnector = new DbConnector();


JiraAccess jira = new JiraAccess();
var fechainicio = "2023-10-01";
var fechaFinal = "2023-11-02";

string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
string logFilePath = Path.Combine(projectDirectory, "ProgramLog.txt"); // Cambiado a ProgramLog.txt

try
{
    while (true)
    {
        List<ComponenteHV> listaIdComponentes = dbConnector.GetComponentesHV();

        if (listaIdComponentes != null && listaIdComponentes.Any())
        {
            foreach (var componente in listaIdComponentes)
            {
                if (componente.IdComponente != null)
                {
                    try
                    {
                        WriteToLog($"Procesando componente {componente.IdComponente}",logFilePath);

                        List<TicketHV> tickets = jira.GetTicketHVs(0, 0, componente.IdComponente);
                        jira.ExportTicketsToExcel(tickets);

                        WriteToLog($"Tickets del componente {componente.IdComponente} exportados correctamente.", logFilePath);

                        jira.ExportComponenteToExcel(componente.IdComponente);

                        WriteToLog($"Componente {componente.IdComponente} exportado correctamente.", logFilePath);

                        dbConnector.MarcarComoDescargado(componente.IdComponente);
                        WriteToLog($"Marcado como descargado en la base de datos para componente {componente.IdComponente}", logFilePath);
                    }
                    catch (Exception e)
                    {
                        string errorMessage = $"Error al exportar el componente {componente.IdComponente}: {e.Message}";
                        Console.WriteLine(errorMessage);

                        WriteToLog($"Error al exportar el componente {componente.IdComponente}: {e.Message}", logFilePath);

                        File.AppendAllText(logFilePath, errorMessage + Environment.NewLine);
                    }
                }
                else
                {
                    Console.WriteLine($"No se encontró el componente con ID: {componente.IdComponente}");
                    WriteToLog($"No se encontró el componente con ID: {componente.IdComponente}", logFilePath);
                }
            }
        }
        else
        {
            break;
        }
    }
}
catch (Exception ex)
{
    // Manejar errores generales al obtener la lista de componentes
    Console.WriteLine($"Error al obtener la lista de componentes: {ex.Message}");
    WriteToLog($"Error al obtener la lista de componentes: {ex.Message}", logFilePath);

    // Registrar el error en el archivo de registro
    File.AppendAllText(logFilePath, $"Error al obtener la lista de componentes: {ex.Message}" + Environment.NewLine);
}

static void WriteToLog(string message, string logFilePath)
{
    try
    {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }
    catch (Exception)
    {
        // Manejar cualquier error al escribir en el archivo de log
    }
}