using DashboarJira.Model;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace DashboarJira.Services
{



    public class DbConnector
    {
        const string PETICIONEVENTOS = "SELECT [Id], [versionTrama], [idRegistro], [idOperador], [idEstacion], [idVagon], [idPuerta], [codigoPuerta], [fechaHoraLecturaDato], [fechaHoraEnvioDato], [tipoTrama], [tramaRetransmitida], [codigoEvento], [estadoAperturaCierrePuertas], [usoBotonManual],[estadoBotonManual], [estadoErrorCritico], [porcentajeCargaBaterias], [ciclosApertura], [horasServicio], [tipoEnergizacion], [velocidaMotor], [fuerzaMotor], [modoOperacion], [numeroEventoBusEstacion], [idVehiculo], [placaVehiculo], [tipologiaVehiculo], [numeroParada], [nombreEstacion], [nombreVagon], [tipoTramaBusEstacion], [codigoAlarma], [codigoNivelAlarma], [tiempoApertura] FROM [Operation].[tbMessages] ";

        private string connectionString;


        public DbConnector()
        {
            // Set the connection string Manatee
            //connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3";

            // Set the connection string Assabloy
            connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDBAssaabloy;User Id=administrador;Password=2022/M4n4t334zur3";
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

        public ComponenteHV GetComponenteHV(string idComponente)
        {
            ComponenteHV componente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT  [IdComponente], [Serial],[aniodefabricacion] ,[Modelo] ,[fechaInicio]FROM[dbo].[registroHV] " +
                                   "WHERE [IdComponente] = @IdComponente";

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
                                    IdComponente = reader.GetString(reader.GetOrdinal("IdComponente")),
                                    Serial = reader.GetString(reader.GetOrdinal("Serial")),
                                    AnioFabricacion = reader.GetInt32(reader.GetOrdinal("aniodefabricacion")),
                                    Modelo = reader.GetString(reader.GetOrdinal("Modelo")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio"))
                                };

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

        public List<ComponenteHV> GetComponentesHV()
        {
            List<ComponenteHV> componentes = new List<ComponenteHV>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT [IdComponente], [Serial], [aniodefabricacion], [Modelo], [fechaInicio] FROM [dbo].[registroHV] ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // ExecuteReader para obtener un conjunto de resultados
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComponenteHV componente = new ComponenteHV
                                {
                                    IdComponente = reader.GetString(reader.GetOrdinal("IdComponente")),
                                    Serial = reader.GetString(reader.GetOrdinal("Serial")),
                                    AnioFabricacion = reader.GetInt32(reader.GetOrdinal("aniodefabricacion")),
                                    Modelo = reader.GetString(reader.GetOrdinal("Modelo")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fechaInicio"))
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
    }






}