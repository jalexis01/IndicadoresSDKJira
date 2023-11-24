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



JsonDocument document = JsonDocument.Parse(json);
string url = "";
string user = "";
string token = "";
string connectionString = "";
JiraAccess jiraAccess = null;

while (true)
{
    Console.WriteLine("Elige una conexión");
    Console.WriteLine("1. Manatee");
    Console.WriteLine("2. Assabloyd");
    Console.WriteLine("3. Salir");

    string opcionConexion = Console.ReadLine();

    if (opcionConexion == "3")
    {
        // The user chose to exit the program
        break;
    }

    // Map user input to internal representation
    string internalOption;
    if (opcionConexion == "1")
    {
        internalOption = "manatee";
    }
    else if (opcionConexion == "2")
    {
        internalOption = "assabloy";
    }
    else
    {
        Console.WriteLine("Opción no válida");
        continue; // Restart the loop to allow the user to enter a valid option
    }

    // Access properties from the JSON based on the selected option
    JsonElement connectionElement;
    if (document.RootElement.TryGetProperty(internalOption, out connectionElement))
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

            // Create an instance of JiraAccess
            jiraAccess = new JiraAccess(url, user, token, connectionString);
        }
        else
        {
            Console.WriteLine("Propiedades faltantes en el JSON");
            continue; // Restart the loop to allow the user to enter a valid option
        }
    }
    else
    {
        Console.WriteLine("Opción no válida");
        continue; // Restart the loop to allow the user to enter a valid option
    }

    var fechainicio = "2023-10-01";
    var fechaFinal = "2023-11-02";
    WriteToLog($"Inicio de descarga de componentes: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);

    DbConnector dbConnector = new DbConnector(connectionString);
    bool ce = true;
    while (ce)
    {
        // Your existing code for component operations here...

        Console.WriteLine("Conexion: " + internalOption);
        Console.WriteLine("Seleccione una opción:");
        Console.WriteLine("1. Descargar información de todos los componentes");
        Console.WriteLine("2. Descargar información de un componente");
        Console.WriteLine("3. Cambiar el estado de todos los componentes a noDescargado");
        Console.WriteLine("4. Cambiar la conexión");
        Console.WriteLine("5. Salir");

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
                    DescargarInformacionComponente(jiraAccess, componente, logFilePath,dbConnector);
                }
                else
                {
                    Console.Write("El id componente no existe ");
                }
                break;
            case "3":
                CambiarEstadoTodosComponentes(dbConnector);
                Console.Write("Estados cambiados");
                break;
            case "4":
                Console.Write("¿Está seguro que desea cambiar la conexion? (S/N): ");
                string respuesta = Console.ReadLine();

                if (respuesta.ToLower() == "s")
                {
                    // Agregar línea para registrar la hora de fin antes de salir del programa
                    WriteToLog($"Cambio de conexion: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);
                    Console.WriteLine("Saliendo de la aplicación.");

                    // Terminar la aplicación
                    ce = false;
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
            case "5":
                // User chose to exit the program
                Console.Write("¿Está seguro que desea salir de la aplicacion? (S/N): ");
                respuesta = Console.ReadLine();

                if (respuesta.ToLower() == "s")
                {
                    // Agregar línea para registrar la hora de fin antes de salir del programa
                    WriteToLog($"Salida de la aplicacion: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);
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
                Environment.Exit(0);
                break;
        }
    }
}

void DescargarInformacionTodosComponentes(JiraAccess jira, DbConnector db)
{

    WriteToLog($"Inicio de descarga de componentes: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);

    try
    {
        while (true)
        {
            List<ComponenteHV> listaIdComponentes = db.GetComponentesHV();

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
                                DescargarInformacionComponente(jiraAccess, componente, logFilePath, db);
                                break;
                            }
                            catch (Exception e)
                            {
                                intentos++;
                                if (intentos == 3)
                                {
                                    db.CambiarDescargado(componente.IdComponente, 3);
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
void DescargarInformacionComponente(JiraAccess jira, ComponenteHV componente, string logFilePath, DbConnector db)
{

    WriteToLog($"Procesando componente {componente.IdComponente}", logFilePath);

    List<TicketHV> tickets = jiraAccess.GetTicketHVs(0, 0, componente.IdComponente);
    jira.ExportTicketsToExcel(tickets);

    WriteToLog($"Tickets del componente {componente.IdComponente} exportados correctamente.", logFilePath);

    jira.ExportComponenteToExcel(componente.IdComponente);

    WriteToLog($"Componente {componente.IdComponente} exportado correctamente.", logFilePath);

    db.MarcarComoDescargado(componente.IdComponente);
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
void CambiarEstadoTodosComponentes(DbConnector db)
{
    db.MarcarTodosComoNoDescargados();
}