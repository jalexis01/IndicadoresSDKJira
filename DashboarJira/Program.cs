// See https://aka.ms/new-console-template for more information
using Atlassian.Jira;
using DashboarJira.Controller;
using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.Extensions.Logging;
using System.Data;

string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
string logFilePath = Path.Combine(projectDirectory, "ProgramLog.txt"); // Cambiado a ProgramLog.txt

// Ruta al archivo JSON que contiene las cadenas de conexión
string jsonPath = "C:\\Users\\juana\\OneDrive\\Documentos\\GitHub\\IndicadoresSDKJira\\DashboarJira\\jsconfig1.json";

// Nombre de la conexión que deseas utilizar (puedes cambiar esto según tus necesidades)
string connectionName = "manatee";
string connectionName1 = "assaabloy";

// Crear una instancia de DbConnector proporcionando la ruta del JSON y el nombre de la conexión
DbConnector dbConnector = new DbConnector(jsonPath, connectionName);

JiraAccess jira = new JiraAccess();
var fechainicio = "2023-10-01";
var fechaFinal = "2023-11-02";
WriteToLog($"Inicio de descarga de componentes: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", logFilePath);


Console.WriteLine("Seleccione la base de datos a la que desea conectar:");
Console.WriteLine("1. Manatee");
Console.WriteLine("2. Assaabloy");
string databaseOption = Console.ReadLine();

DbConnector dbConnector;

switch (databaseOption)
{
    case "1":
        dbConnector = new DbConnector("Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3");
        break;
    case "2":
        dbConnector = new DbConnector("Server=manatee.database.windows.net;Database=PuertasTransmilenioDBAssaabloy;User Id=administrador;Password=2022/M4n4t334zur3");
        break;
    default:
        Console.WriteLine("Opción no válida. Saliendo de la aplicación.");
        return; // Terminar la aplicación
}


while (true)
{
   

    Console.WriteLine("Bienvenido a la aplicación de consola de MANATEE");
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

 void DescargarInformacionTodosComponentes(JiraAccess jira, DbConnector dbConnector)
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