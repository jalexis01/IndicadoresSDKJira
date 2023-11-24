// See https://aka.ms/new-console-template for more information
using Atlassian.Jira;
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
string logFilePath = Path.Combine(projectDirectory, "ProgramLog.txt"); // Cambiado a ProgramLog.txt

string jsonFilePath = "jsconfig1.json";
string json = File.ReadAllText(jsonFilePath);
JiraAccess jiraAccess;
string url = "";
string user = "";
string token = "";
string connectionString = "";
// Deserializa el JSON en un objeto JsonDocument
JsonDocument document = JsonDocument.Parse(json);
// Aquí puedes preguntar al usuario qué conexión desea utilizar (manatee o assabloy)
Console.WriteLine("Elige una conexión (manatee/assabloy):");
string opcion = Console.ReadLine();

// Accede directamente a las propiedades del JSON según la opción elegida
JsonElement connectionElement;
if (document.RootElement.TryGetProperty(opcion, out connectionElement))
{
    if (connectionElement.TryGetProperty("url", out JsonElement urlElement) &&
        connectionElement.TryGetProperty("user", out JsonElement userElement) &&
        connectionElement.TryGetProperty("token", out JsonElement tokenElement) &&
        connectionElement.TryGetProperty("connectionString", out JsonElement connectionStringElement))
    {
        url = urlElement.GetString();
        user = userElement.GetString();
        token = tokenElement.GetString();
        connectionString = connectionStringElement.GetString();

        // Crea la instancia de JiraAccess
        jiraAccess = new JiraAccess(url, user, token, connectionString);
    }
    else
    {
        Console.WriteLine("Propiedades faltantes en el JSON");
    }
}
else
{
    Console.WriteLine("Opción no válida");
}
jiraAccess = new JiraAccess(url, user, token, connectionString);
var fechainicio = "2023-10-01";
var fechaFinal = "2023-11-02";
WriteToLog($"Inicio de descarga de componentes: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);




DbConnector dbConnector = new DbConnector(connectionString);

while (true)
{
    // Pregunta al usuario por la cadena de conexión
 
    // Crea la instancia de JiraAccess
    jiraAccess = new JiraAccess(url, user, token, connectionString);
    
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
            DescargarInformacionTodosComponentes(jiraAccess, dbConnector);
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
            Console.Write("¿Está seguro que desea salir del aplicativo? (S/N): ");
            string respuesta = Console.ReadLine();

            if (respuesta.ToLower() == "s")
            {
                // Agregar línea para registrar la hora de fin antes de salir del programa
                WriteToLog($"Fin de operación: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);
                Console.WriteLine("Saliendo de la aplicación.");

                // Terminar la aplicación
                Environment.Exit(0);
            }
            else if (respuesta.ToLower() == "n")
            {
                // No hace nada y vuelve al menú anterior
            }
            else
            {
                Console.WriteLine("Opción no válida. Volviendo al menú anterior.");
            }
            break;

        default:
            Console.WriteLine("Opción no válida. Saliendo de la aplicación.");
            // Terminar la aplicación
            Environment.Exit(0);
            break;
    }
}
void DescargarInformacionTodosComponentes(JiraAccess jiraAccess, DbConnector dbConnector)
{

    WriteToLog($"Inicio de descarga de componentes: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);

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
                            WriteToLog($"Fin de descarga de componentes: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);
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

    List<TicketHV> tickets = jiraAccess.GetTicketHVs(0, 0, componente.IdComponente);
    jiraAccess.ExportTicketsToExcel(tickets);

    WriteToLog($"Tickets del componente {componente.IdComponente} exportados correctamente.", logFilePath);

    jiraAccess.ExportComponenteToExcel(componente.IdComponente);

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