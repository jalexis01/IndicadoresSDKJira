using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Services
{
    public class DbConnector
    {
        private string connectionString;

        public DbConnector()
        {
            // Set the connection string
            connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3";
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