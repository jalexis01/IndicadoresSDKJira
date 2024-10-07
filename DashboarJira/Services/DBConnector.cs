using DashboarJira.Model;
using Microsoft.Data.SqlClient;
using MQTT.FunctionApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace DashboarJira.Services
{
    public class DbConnector
    {
        const string PETICIONEVENTOS = "SELECT [Id], [versionTrama], [idRegistro], [idOperador], [idEstacion], [idVagon], [idPuerta], [codigoPuerta], [fechaHoraLecturaDato], [fechaHoraEnvioDato], [tipoTrama], [tramaRetransmitida], [codigoEvento], [estadoAperturaCierrePuertas], [usoBotonManual],[estadoBotonManual], [estadoErrorCritico], [porcentajeCargaBaterias], [ciclosApertura], [horasServicio], [tipoEnergizacion], [velocidaMotor], [fuerzaMotor], [modoOperacion], [numeroEventoBusEstacion], [idVehiculo], [placaVehiculo], [tipologiaVehiculo], [numeroParada], [nombreEstacion], [nombreVagon], [tipoTramaBusEstacion], [codigoAlarma], [codigoNivelAlarma], [tiempoApertura] FROM [Operation].[tbMessages] ";

        private string connectionString;

        public DbConnector(string connectionName)
        {
            connectionString = connectionName;
        }
        public void UpdateEstacionYRegistroHV(ConexionEstacionUpdater updater)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Actualizar la tabla Estaciones
                    UpdateEstacion(connection, updater);

                    // Actualizar la tabla registroHV (Puertas)
                    UpdateRegistroHV(connection, updater);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        private void UpdateEstacion(SqlConnection connection, ConexionEstacionUpdater updater)
        {
            string query = @"UPDATE [dbo].[Estaciones]
                     SET [UltimaConexion] = @UltimaConexion,
                         [InicioOperacion] = CASE WHEN @Evento = 'EVP8' THEN @UltimaConexion ELSE [InicioOperacion] END,
                         [FinOperacion] = CASE WHEN @Evento = 'EVP9' THEN @UltimaConexion ELSE [FinOperacion] END
                     WHERE [IdEstacion] = @IdEstacion";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UltimaConexion", updater.UltimaConexion);
                command.Parameters.AddWithValue("@Evento", updater.Evento);
                command.Parameters.AddWithValue("@IdEstacion", updater.IdEstacion);

                command.ExecuteNonQuery();
            }
        }

        private void UpdateRegistroHV(SqlConnection connection, ConexionEstacionUpdater updater)
        {
            string query = @"UPDATE [dbo].[registroHV]
                     SET [UltimaConexion] = @UltimaConexion,
                         [EstadoApertura] = CASE WHEN @EstadoApertura IS NOT NULL THEN @EstadoApertura ELSE [EstadoApertura] END,
                         [EstadoErrorCritico] = CASE WHEN @EstadoErrorCritico IS NOT NULL THEN @EstadoErrorCritico ELSE [EstadoErrorCritico] END
                     WHERE [Serial] = @Serial";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UltimaConexion", updater.UltimaConexion);
                command.Parameters.AddWithValue("@EstadoApertura", (object)updater.EstadoApertura ?? DBNull.Value);
                command.Parameters.AddWithValue("@EstadoErrorCritico", (object)updater.EstadoErrorCritico ?? DBNull.Value);
                command.Parameters.AddWithValue("@Serial", updater.Serial);

                int rowsAffected = command.ExecuteNonQuery();

                // Si no se encuentra la puerta (ninguna fila actualizada), solo se actualiza la estación
                if (rowsAffected == 0)
                {
                    Console.WriteLine($"No se encontró la puerta con el serial: {updater.Serial}. Solo se actualizó la estación.");
                }
            }
        }

        public DataTable GetMessages()
        {
            DataTable messagesTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to retrieve the tb.Messages table
                    string query = "SELECT TOP (1000) [Id]\r\n      ,[IdHeaderMessage]\r\n      ,[versionTrama]\r\n      ,[idRegistro]\r\n      ,[idOperador]\r\n      ,[idEstacion]\r\n      ,[idVagon]\r\n      ,[idPuerta]\r\n      ,[codigoPuerta]\r\n      ,[fechaHoraLecturaDato]\r\n      ,[fechaHoraEnvioDato]\r\n      ,[tipoTrama]\r\n      ,[tramaRetransmitida]\r\n      ,[codigoEvento]\r\n      ,[estadoAperturaCierrePuertas]\r\n      ,[usoBotonManual]\r\n      ,[estadoBotonManual]\r\n      ,[estadoErrorCritico]\r\n      ,[porcentajeCargaBaterias]\r\n      ,[ciclosApertura]\r\n      ,[horasServicio]\r\n      ,[tipoEnergizacion]\r\n      ,[velocidaMotor]\r\n      ,[fuerzaMotor]\r\n      ,[modoOperacion]\r\n      ,[numeroEventoBusEstacion]\r\n      ,[idVehiculo]\r\n      ,[placaVehiculo]\r\n      ,[tipologiaVehiculo]\r\n      ,[numeroParada]\r\n      ,[nombreEstacion]\r\n      ,[nombreVagon]\r\n      ,[tipoTramaBusEstacion]\r\n      ,[codigoAlarma]\r\n      ,[codigoNivelAlarma]\r\n      ,[tiempoApertura]\r\n  FROM [Operation].[tbMessages]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill the DataTable with the data from the query
                            adapter.Fill(messagesTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return messagesTable;
        }
        public DataTable GetEstaciones()
        {
            DataTable messagesTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to retrieve the tb.Messages table
                    string query = "SELECT  [Id],[idEstacion],[nombreEstacion] FROM[dbo].[Estaciones]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill the DataTable with the data from the query
                            adapter.Fill(messagesTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return messagesTable;
        }
        public List<Estacion> GetEstacionesV()
        {
            List<Estacion> estaciones = new List<Estacion>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to retrieve all the columns from the Estaciones table
                    string query = "SELECT [Id], [idEstacion], [nombreEstacion], [Vagones], [InicioOperacion], [FinOperacion], [UltimaConexion] FROM [dbo].[Estaciones]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new Estacion object and map the fields
                                Estacion estacion = new Estacion
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    idEstacion = reader.GetInt32(reader.GetOrdinal("idEstacion")),
                                    nombreEstacion = reader.GetString(reader.GetOrdinal("nombreEstacion")),
                                    Vagones = reader.IsDBNull(reader.GetOrdinal("Vagones")) ? null : reader.GetString(reader.GetOrdinal("Vagones")),
                                    InicioOperacion = reader.IsDBNull(reader.GetOrdinal("InicioOperacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("InicioOperacion")),
                                    FinOperacion = reader.IsDBNull(reader.GetOrdinal("FinOperacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FinOperacion")),
                                    UltimaConexion = reader.IsDBNull(reader.GetOrdinal("UltimaConexion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UltimaConexion"))
                                };

                                // Add the Estacion object to the list
                                estaciones.Add(estacion);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return estaciones;
        }
        public List<ComponenteHV> GetPuertasByEstacionId(int estacionId)
        {
            List<ComponenteHV> componentes = new List<ComponenteHV>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to retrieve components where idComponente contains the estacionId and tipoComponente is 'Puerta'
                    string query = @"SELECT [IdComponente], [Serial],[aniodefabricacion],[descargado] ,[tipoComponente] ,[Modelo] ,[fechaInicio], [Estado], [FechaFin],
                                    [Vagon], [Canal], [EstadoErrorCritico], [EstadoApertura]
                             FROM [dbo].[registroHV] 
                             WHERE [IdComponente] LIKE @EstacionIdPattern
                             AND [tipoComponente] = 'Puerta'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the parameter for estacionId with wildcard (%) for LIKE
                        command.Parameters.AddWithValue("@EstacionIdPattern", estacionId + "-%");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Mapear los resultados a un objeto ComponenteHV
                                ComponenteHV componente = new ComponenteHV
                                {
                                    IdComponente = reader.IsDBNull(reader.GetOrdinal("IdComponente")) ? null : reader.GetString(reader.GetOrdinal("IdComponente")),
                                    Serial = reader.IsDBNull(reader.GetOrdinal("Serial")) ? null : reader.GetString(reader.GetOrdinal("Serial")),
                                    AnioFabricacion = reader.IsDBNull(reader.GetOrdinal("aniodefabricacion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("aniodefabricacion")),
                                    Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? null : reader.GetString(reader.GetOrdinal("Modelo")),
                                    FechaInicio = reader.IsDBNull(reader.GetOrdinal("fechaInicio")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                    descargado = reader.IsDBNull(reader.GetOrdinal("descargado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("descargado")),
                                    Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Estado")), // Aquí mapeamos el campo bit
                                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")), // Aquí mapeamos el campo DateTime

                                    Vagon = reader.IsDBNull(reader.GetOrdinal("Vagon")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Vagon")),
                                    Canal = reader.IsDBNull(reader.GetOrdinal("Canal")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Canal")),
                                    EstadoErrorCritico = reader.IsDBNull(reader.GetOrdinal("EstadoErrorCritico")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("EstadoErrorCritico")),
                                    EstadoApertura = reader.IsDBNull(reader.GetOrdinal("EstadoApertura")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("EstadoApertura"))
                                };

                                // Add the ComponenteHV object to the list
                                componentes.Add(componente);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return componentes;
        }



        public ComponenteHV GetComponenteHV(string idComponente)
        {
            ComponenteHV componente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    string query = "SELECT  [IdComponente], [Serial],[aniodefabricacion],[descargado] ,[tipoComponente] ,[Modelo] ,[fechaInicio], [Estado], [FechaFin] FROM[dbo].[registroHV] " +
                                   "WHERE [IdComponente] = @IdComponente  ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdComponente", idComponente);

                        // ExecuteScalar devuelve la primera columna de la primera fila del conjunto de resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                componente = new ComponenteHV
                                {
                                    IdComponente = !reader.IsDBNull(reader.GetOrdinal("IdComponente")) ? reader.GetString(reader.GetOrdinal("IdComponente")) : null,
                                    Serial = !reader.IsDBNull(reader.GetOrdinal("Serial")) ? reader.GetString(reader.GetOrdinal("Serial")) : null,
                                    AnioFabricacion = !reader.IsDBNull(reader.GetOrdinal("aniodefabricacion")) ? reader.GetInt32(reader.GetOrdinal("aniodefabricacion")) : (int?)null,
                                    Modelo = !reader.IsDBNull(reader.GetOrdinal("Modelo")) ? reader.GetString(reader.GetOrdinal("Modelo")) : null,
                                    FechaInicio = !reader.IsDBNull(reader.GetOrdinal("fechaInicio")) ? reader.GetDateTime(reader.GetOrdinal("fechaInicio")) : (DateTime?)null,
                                    tipoComponente = !reader.IsDBNull(reader.GetOrdinal("tipoComponente")) ? reader.GetString(reader.GetOrdinal("tipoComponente")) : null,
                                    descargado = !reader.IsDBNull(reader.GetOrdinal("descargado")) ? reader.GetInt32(reader.GetOrdinal("descargado")) : (int?)null,
                                    horasDeOperacion = "0",
                                    Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Estado")), // Aquí mapeamos el campo bit
                                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")) // Aquí mapeamos el campo DateTime
                                };
                                componente.CalcularHorasDeOperacion();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return componente;
        }
        public void MarcarComoDescargado(string idComponente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Actualizar el estado de descargado a verdadero
                    string updateQuery = "UPDATE [dbo].[registroHV] SET [descargado] = 1 WHERE [IdComponente] = @IdComponente";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@IdComponente", idComponente);

                        // Ejecutar la consulta de actualización
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("El componente con IdComponente {0} ha sido marcado como descargado.", idComponente);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró ningún componente con IdComponente {0}.", idComponente);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
        public void MarcarTodosComoNoDescargados()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Actualizar el estado de descargado a falso para todos los registros
                    string updateQuery = "UPDATE [dbo].[registroHV] SET [descargado] = 0";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        // Ejecutar la consulta de actualización
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        Console.WriteLine("{0} componentes han sido marcados como no descargados.", rowsAffected);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }


        public List<ComponenteHV> GetComponentesHV()
        {
            List<ComponenteHV> componentes = new List<ComponenteHV>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT TOP 100 [IdComponente], [Serial], [aniodefabricacion], [tipoComponente], [descargado], [Modelo], [fechaInicio], [Estado], [FechaFin] FROM [dbo].[registroHV] " +
                           "WHERE [tipoComponente] = 'puerta' AND [descargado] = 0 ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // ExecuteReader para obtener un conjunto de resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComponenteHV componente = new ComponenteHV
                                {
                                    IdComponente = reader.IsDBNull(reader.GetOrdinal("IdComponente")) ? null : reader.GetString(reader.GetOrdinal("IdComponente")),
                                    Serial = reader.IsDBNull(reader.GetOrdinal("Serial")) ? null : reader.GetString(reader.GetOrdinal("Serial")),
                                    AnioFabricacion = reader.IsDBNull(reader.GetOrdinal("aniodefabricacion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("aniodefabricacion")),
                                    Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? null : reader.GetString(reader.GetOrdinal("Modelo")),
                                    FechaInicio = reader.IsDBNull(reader.GetOrdinal("fechaInicio")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                    descargado = reader.IsDBNull(reader.GetOrdinal("descargado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("descargado")),
                                    Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Estado")), // Aquí mapeamos el campo bit
                                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")) // Aquí mapeamos el campo DateTime
                                };


                                componentes.Add(componente);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine("An error occurred: " + ex.Message);
                    throw; // Puedes lanzar la excepción nuevamente para propagarla hacia arriba.
                }
            }

            return componentes;
        }
        public List<ComponenteHV> GetComponentesHV(string modelo)
        {
            List<ComponenteHV> componentes = new List<ComponenteHV>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT TOP 100 [IdComponente], [Serial], [aniodefabricacion], [tipoComponente], [descargado], [Modelo], [fechaInicio] FROM [dbo].[registroHV] " +
                   "WHERE [tipoComponente] = 'puerta' AND [descargado] = 0 AND [Modelo] LIKE '%" + modelo + "%'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // ExecuteReader para obtener un conjunto de resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComponenteHV componente = new ComponenteHV
                                {
                                    IdComponente = reader.IsDBNull(reader.GetOrdinal("IdComponente")) ? null : reader.GetString(reader.GetOrdinal("IdComponente")),
                                    Serial = reader.IsDBNull(reader.GetOrdinal("Serial")) ? null : reader.GetString(reader.GetOrdinal("Serial")),
                                    AnioFabricacion = reader.IsDBNull(reader.GetOrdinal("aniodefabricacion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("aniodefabricacion")),
                                    Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? null : reader.GetString(reader.GetOrdinal("Modelo")),
                                    FechaInicio = reader.IsDBNull(reader.GetOrdinal("fechaInicio")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fechaInicio")),
                                    descargado = reader.IsDBNull(reader.GetOrdinal("descargado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("descargado"))
                                };


                                componentes.Add(componente);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine("An error occurred: " + ex.Message);
                    throw; // Puedes lanzar la excepción nuevamente para propagarla hacia arriba.
                }
            }

            return componentes;
        }


        public List<Evento> GetEventos(string peticion)
        {

            string query;

            query = PETICIONEVENTOS + peticion;
            List<Evento> eventos = new List<Evento>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            command.CommandTimeout = 6000;


                            while (reader.Read())
                            {
                                Evento evento = new Evento();

                                evento.versionTrama = reader.IsDBNull(1) ? null : reader.GetString(1);
                                evento.idRegistro = reader.IsDBNull(2) ? null : reader.GetString(2);
                                evento.idOperador = reader.IsDBNull(3) ? null : reader.GetString(3);
                                evento.idEstacion = reader.IsDBNull(4) ? null : reader.GetString(4);
                                evento.idVagon = reader.IsDBNull(5) ? null : reader.GetString(5);
                                evento.idPuerta = reader.IsDBNull(6) ? null : reader.GetString(6);
                                evento.codigoPuerta = reader.IsDBNull(7) ? null : reader.GetString(7);
                                evento.fechaHoraLecturaDato = reader.IsDBNull(8) ? null : reader.GetDateTime(8);
                                evento.fechaHoraEnvioDato = reader.IsDBNull(9) ? null : reader.GetDateTime(9);
                                evento.tipoTrama = reader.IsDBNull(10) ? null : (int?)reader.GetInt32(10);
                                evento.tramaRetransmitida = reader.IsDBNull(11) ? null : reader.GetString(11);
                                evento.codigoEvento = reader.IsDBNull(12) ? null : reader.GetString(12);
                                evento.estadoAperturaCierrePuertas = reader.IsDBNull(13) ? null : reader.GetString(13);
                                evento.usoBotonmanual = reader.IsDBNull(14) ? null : reader.GetInt32(14);
                                evento.estadoBotonManual = reader.IsDBNull(15) ? null : reader.GetBoolean(15);
                                evento.estadoErrorCritico = reader.IsDBNull(16) ? null : reader.GetBoolean(16);
                                evento.porcentajeCargaBaterias = reader.IsDBNull(17) ? null : reader.GetFloat(17);

                                evento.ciclosApertura = reader.IsDBNull(18) ? null : reader.GetInt32(18);
                                evento.horasServicios = reader.IsDBNull(19) ? null : reader.GetInt32(19);
                                evento.tipoEnergizacion = reader.IsDBNull(20) ? null : reader.GetInt32(20);
                                evento.velocidadMotor = reader.IsDBNull(21) ? null : reader.GetFloat(21);
                                evento.fuerzaMotor = reader.IsDBNull(22) ? null : reader.GetFloat(22);
                                evento.modoOperacion = reader.IsDBNull(23) ? null : reader.GetInt32(23);
                                evento.numeroEventoBusEstacion = reader.IsDBNull(24) ? null : reader.GetInt32(24);
                                evento.idVehiculo = reader.IsDBNull(25) ? null : reader.GetString(25);
                                evento.placaVehiculo = reader.IsDBNull(26) ? null : reader.GetString(26);
                                evento.tipologiaVehiculo = reader.IsDBNull(27) ? null : reader.GetString(27);
                                evento.numeroParada = reader.IsDBNull(28) ? null : reader.GetString(28);
                                evento.nombreEstacion = reader.IsDBNull(29) ? null : reader.GetString(29);
                                evento.nombreVagon = reader.IsDBNull(30) ? null : reader.GetString(30);
                                evento.tipoTramaBusEstacion = reader.IsDBNull(31) ? null : reader.GetString(31);
                                evento.codigoAlarma = reader.IsDBNull(32) ? null : reader.GetString(32);
                                evento.codigoNivelAlarma = reader.IsDBNull(33) ? null : reader.GetString(33);
                                evento.tiempoApertura = reader.IsDBNull(34) ? null : reader.GetInt32(34);

                                //Console.WriteLine("****************************************************************");
                                eventos.Add(evento);
                            }



                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return eventos;
        }






        public DataTable GetMessagesIORevp9()
        {
            DataTable messagesTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to retrieve the tb.Messages table
                    string query = "SELECT [Id]\r\n, [idEstacion]\r\n, [codigoEvento]\r\n FROM [Operation].[tbMessages]\r\n WHERE codigoEvento = 'EVP9' ANd fechaHoraEnvioDato BETWEEN '2023-01-01' AND '2023-02-01' ORDER BY fechaHoraEnvioDato ASC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill the DataTable with the data from the query
                            adapter.Fill(messagesTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return messagesTable;
        }





        public List<Evento> setEvento(DateTime fechaInicio, DateTime fechaFin, string codigoEvento)
        {
            List<Evento> eventos = new List<Evento>();


            return eventos;
        }


        public string GetMessagesAsJson()
        {

            DataTable messagesTable = GetMessages();
            // Convert DataTable to JSON
            string json = JsonConvert.SerializeObject(messagesTable, Formatting.Indented);

            return json;
        }


        public string GetMessagesAsString()
        {
            DataTable messagesTable = GetMessages();

            // Convert DataTable to a string representation
            string tableString = string.Empty;

            foreach (DataRow row in messagesTable.Rows)
            {
                foreach (DataColumn col in messagesTable.Columns)
                {
                    tableString += row[col].ToString() + "\t";
                }

                tableString += Environment.NewLine;
            }

            return tableString;
        }

        internal void CambiarDescargado(string idComponente, int v)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Actualizar el estado de descargado a verdadero
                    string updateQuery = "UPDATE [dbo].[registroHV] SET [descargado] = " + v + " WHERE [IdComponente] = @IdComponente";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@IdComponente", idComponente);

                        // Ejecutar la consulta de actualización
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("El componente con IdComponente {0} ha sido marcado como 3.", idComponente);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró ningún componente con IdComponente {0}.", idComponente);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}