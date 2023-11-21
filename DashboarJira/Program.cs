// See https://aka.ms/new-console-template for more information
using Atlassian.Jira;
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.Extensions.Logging;
using System.Data;

string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
string logFilePath = Path.Combine(projectDirectory, "ProgramLog.txt"); // Cambiado a ProgramLog.txt
DbConnector dbConnector = new DbConnector();
JiraAccess jira = new JiraAccess();
var fechainicio = "2023-10-01";
var fechaFinal = "2023-11-02";
while (true)
{
    Console.WriteLine("Bienvenido a la aplicación de consola");
    Console.WriteLine("Seleccione una opción:");
    Console.WriteLine("1. Descargar información de todos los componentes");
    Console.WriteLine("2. Descargar información de un componente");
    Console.WriteLine("3. Cambiar el estado de todos los componentes a noDescargado");
    Console.WriteLine("4. Salir");

    string input = Console.ReadLine();

    switch (input)
    {
        case "1":
            DescargarInformacionTodosComponentes(jira,dbConnector);
            break;
        case "2":
            Console.Write("Ingrese el ID del componente: ");
            string componenteId = Console.ReadLine();
            var componente = dbConnector.GetComponenteHV(componenteId);
            if (componente != null)
            {
                Console.Write("Descargando componente: " + componenteId);
                DescargarInformacionComponente(componente, logFilePath);
            }
            else
            {
                Console.Write("El id componente no existe ");
            }
            break;
        case "3":
            CambiarEstadoTodosComponentes();
            Console.Write("Estados cambiados");
            break;
        case "4":
            Console.WriteLine("Saliendo de la aplicación.");
            break;
        default:
            Console.WriteLine("Opción no válida. Saliendo de la aplicación.");
            break;
    }
}

 void DescargarInformacionTodosComponentes(JiraAccess jira, DbConnector dbConnector)
{
    

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
                        int intentos = 0;

                        while (intentos < 3)
                        {
                            try
                            {
                                Console.Write("Descargando id componente: " + componente.IdComponente);
                                DescargarInformacionComponente(componente, logFilePath);
                                break;
                            }
                            catch (Exception e)
                            {
                                intentos++;
                                if (intentos == 3)
                                {
                                    dbConnector.CambiarDescargado(componente.IdComponente, 3);
                                }
                                string errorMessage = $"Error al exportar el componente {componente.IdComponente}, intento {intentos}: {e.Message}";
                                Console.WriteLine(errorMessage);

                                WriteToLog($"Error al exportar el componente {componente.IdComponente}, intento {intentos}: {e.Message}", logFilePath);

                                File.AppendAllText(logFilePath, errorMessage + Environment.NewLine);

                                // Puedes añadir algún tipo de pausa o espera entre intentos si es necesario
                            }
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

    // Más código aquí si es necesario...

}
void DescargarInformacionComponente(ComponenteHV componente, string logFilePath)
{

    WriteToLog($"Procesando componente {componente.IdComponente}", logFilePath);

    List<TicketHV> tickets = jira.GetTicketHVs(0, 0, componente.IdComponente);
    jira.ExportTicketsToExcel(tickets);

    WriteToLog($"Tickets del componente {componente.IdComponente} exportados correctamente.", logFilePath);

    jira.ExportComponenteToExcel(componente.IdComponente);

    WriteToLog($"Componente {componente.IdComponente} exportado correctamente.", logFilePath);

    dbConnector.MarcarComoDescargado(componente.IdComponente);
    WriteToLog($"Marcado como descargado en la base de datos para componente {componente.IdComponente}", logFilePath);
}

void WriteToLog(string message, string logFilePath)
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
void CambiarEstadoTodosComponentes()
{
    dbConnector.MarcarTodosComoNoDescargados();
}