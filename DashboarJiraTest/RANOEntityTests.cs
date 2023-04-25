using System.Collections.Generic;
using NUnit.Framework;
using DashboarJira.Model;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DashboarJiraTest
{
	[TestFixture]
	public class RANOEntityTests
	{
		private List<Ticket> tickets;


		[SetUp]
		public void setUpScenario1()
		{
			List<Ticket> ticketsList = new List<Ticket>();

			for (int i = 1; i <= 10; i++)
			{
				var ticket = new Ticket
				{
					id_ticket = $"TICKET-{i}",
					id_estacion = "9117",
					id_vagon = "WA",
					id_puerta = "9117-WA-OR-5",
					tipoComponente = "Puerta",
					identificacion = "N1T-0017",
					tipo_mantenimiento = "Mantenimiento Correctivo",
					nivel_falla = "AIO",
					codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
					fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
					fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
					fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
					componente_Parte = null,
					tipo_reparacion = "Ajuste",
					tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
					descripcion_reparacion = null,
					diagnostico_causa = "A cargo del contratista",
					estado_ticket = "Cerrado"
				};
				ticketsList.Add(ticket);
			}


		}
		[Test]
		public void InitializationTest()
		{
			// Arrange
			var ticketTAN = new List<Ticket>();
			var ticketTCN = new List<Ticket>();

			// Act
			var entity = new RANOEntity(ticketTAN, ticketTCN);

			// Assert
			Assert.IsNotNull(entity);
			Assert.IsNotNull(entity.TicketTAN);
			Assert.IsNotNull(entity.TicketTCN);
			Assert.That(entity.TicketTAN, Is.EqualTo(ticketTAN));
			Assert.That(entity.TicketTCN, Is.EqualTo(ticketTCN));
		}

		[Test]
		public void TestAgregarTicket()
		{
			// Arrange
			var ticket = new Ticket
			{
				id_ticket = "TICKET-86",
				id_estacion = "9117",
				id_vagon = "WA",
				id_puerta = "9117-WA-OR-5",
				tipoComponente = "Puerta",
				identificacion = "N1T-0017",
				tipo_mantenimiento = "Mantenimiento Correctivo",
				nivel_falla = "AIO",
				codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
				fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
				fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
				fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
				componente_Parte = null,
				tipo_reparacion = "Ajuste",
				tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
				descripcion_reparacion = null,
				diagnostico_causa = "A cargo del contratista",
				estado_ticket = "Cerrado"
			};
			var listaTicketTAN = new List<Ticket>();
			var listaTicketTCN = new List<Ticket>();
			var entity = new RANOEntity(listaTicketTAN, listaTicketTCN);

			// Act
			entity.TicketTAN.Add(ticket);

			// Assert
			Assert.That(entity.TicketTAN.Count, Is.EqualTo(1));
			Assert.That(entity.TicketTCN.Count, Is.EqualTo(0));
			Assert.That(entity.TicketTAN[0], Is.EqualTo(ticket));
		}
		[Test]
		public void CalcularIndicadorSameDataAllRANOTest()
		{
			var listaTicketTAN = new List<Ticket>();
			var listaTicketTCN = new List<Ticket>();
			var rano = new RANOEntity(listaTicketTAN, listaTicketTCN);
			// Agregar los tickets a la lista de tickets
			for (int i = 1; i <= 10; i++)
			{
				var ticket = new Ticket
				{
					id_ticket = $"TICKET-{i}",
					id_estacion = "9117",
					id_vagon = "WA",
					id_puerta = "9117-WA-OR-5",
					tipoComponente = "Puerta",
					identificacion = "N1T-0017",
					tipo_mantenimiento = "Mantenimiento Correctivo",
					nivel_falla = "AIO",
					codigo_falla = "No reproducción de mensajes de audio FLL-N-0001",
					fecha_apertura = DateTime.Parse("2023-03-01T14:35:17.505"),
					fecha_cierre = DateTime.Parse("2023-03-03T13:27:05.61"),
					fecha_arribo_locacion = DateTime.Parse("2023-03-01T14:35:26.59"),
					componente_Parte = null,
					tipo_reparacion = "Ajuste",
					tipo_ajuste_configuracion = "Ajuste cables de datos AJS-P-0001",
					descripcion_reparacion = null,
					diagnostico_causa = "A cargo del contratista",
					estado_ticket = "Cerrado"
				};
				rano.TicketTAN.Add(ticket);
				rano.TicketTCN.Add(ticket);
			}

			// Calcular el indicador RANO
			var indicadorRANO = rano.CalcularIndicadorRANO();

			// Verificar que el resultado sea el esperado
			Assert.That(indicadorRANO, Is.EqualTo(100.0));
		}

        [Test]
        public void CalcularIndicadorRANOCorrecto()
        {
            // Cadena de texto con los datos de los tickets
            string cadena_ticketsTCI = "[{\"id_ticket\":\"TICKET-75\",\"id_estacion\":\"9117\",\"id_vagon\":\"WB\",\"id_puerta\":\"9117-WB-OR-1\",\"id_componente\":\"9117-WB-OR-1\",\"tipoComponente\":\"Puerta\",\"identificacion\":\"N1T-0021\",\"tipo_mantenimiento\":\"Correctivo\",\"nivel_falla\":\"ANIO\",\"codigo_falla\":\"La puerta intenta cerrar pero no logra el cierre total FLL-N-0009\",\"fecha_apertura\":\"2023-02-27T18:06:08.046-05:00\",\"fecha_cierre\":\"2023-02-27T18:07:20\",\"fecha_arribo_locacion\":\"2023-02-27T18:06:17\",\"componente_Parte\":\" \",\"tipo_reparacion\":\"Ajuste\",\"tipo_ajuste_configuracion\":\"Ajuste sensor final de carrera AJS-P-0002\\n\",\"descripcion_reparacion\":\"null\",\"diagnostico_causa\":\"A cargo del contratista\",\"tipo_causa\":\"A cargo del contratista\",\"estado_ticket\":\"Cerrado\"}]";

			string cadena_ticketsTAI = "[\r\n  {\r\n    \"id_ticket\": \"TICKET-75\",\r\n    \"id_estacion\": \"9117\",\r\n    \"id_vagon\": \"WB\",\r\n    \"id_puerta\": \"9117-WB-OR-1\",\r\n    \"id_componente\": \"9117-WB-OR-1\",\r\n    \"tipoComponente\": \"Puerta\",\r\n    \"identificacion\": \"N1T-0021\",\r\n    \"tipo_mantenimiento\": \"Correctivo\",\r\n    \"nivel_falla\": \"ANIO\",\r\n    \"codigo_falla\": \"La puerta intenta cerrar pero no logra el cierre total FLL-N-0009\",\r\n    \"fecha_apertura\": \"2023-02-27T18:06:08.046-05:00\",\r\n    \"fecha_cierre\": \"2023-02-27T18:07:20\",\r\n    \"fecha_arribo_locacion\": \"2023-02-27T18:06:17\",\r\n    \"componente_Parte\": \" \",\r\n    \"tipo_reparacion\": \"Ajuste\",\r\n    \"tipo_ajuste_configuracion\": \"Ajuste sensor final de carrera AJS-P-0002\\n\",\r\n    \"descripcion_reparacion\": \"null\",\r\n    \"diagnostico_causa\": \"A cargo del contratista\",\r\n    \"tipo_causa\": \"A cargo del contratista\",\r\n    \"estado_ticket\": \"Cerrado\"\r\n  },\r\n  {\r\n    \"id_ticket\": \"TICKET-61\",\r\n    \"id_estacion\": \"null\",\r\n    \"id_vagon\": \"null\",\r\n    \"id_puerta\": \"9110-WA-OR-1\",\r\n    \"id_componente\": \"9110-WA-OR-1\",\r\n    \"tipoComponente\": \"Puerta\",\r\n    \"identificacion\": \"N1S-0001\",\r\n    \"tipo_mantenimiento\": \"Correctivo\",\r\n    \"nivel_falla\": \"ANIO\",\r\n    \"codigo_falla\": \"No se reciben datos de un puerta FLL-N-0015\",\r\n    \"fecha_apertura\": \"2023-02-20T09:31:02.02-05:00\",\r\n    \"fecha_cierre\": \"2023-02-27T17:51:23\",\r\n    \"fecha_arribo_locacion\": \"2023-02-20T09:31:08\",\r\n    \"componente_Parte\": \" \",\r\n    \"tipo_reparacion\": \"Ajuste\",\r\n    \"tipo_ajuste_configuracion\": \"Ajuste cables de datos AJS-R-0001\\n\",\r\n    \"descripcion_reparacion\": \"null\",\r\n    \"diagnostico_causa\": \"A cargo del contratista\",\r\n    \"tipo_causa\": \"A cargo del contratista\",\r\n    \"estado_ticket\": \"Cerrado\"\r\n  },\r\n  {\r\n    \"id_ticket\": \"TICKET-58\",\r\n    \"id_estacion\": \"9115\",\r\n    \"id_vagon\": \"WA\",\r\n    \"id_puerta\": \"9115-WA-OR-1\",\r\n    \"id_componente\": \"9115-WA-OR-1\",\r\n    \"tipoComponente\": \"Puerta\",\r\n    \"identificacion\": \"N1T-0001\",\r\n    \"tipo_mantenimiento\": \"Correctivo\",\r\n    \"nivel_falla\": \"ANIO\",\r\n    \"codigo_falla\": \"La puerta tiene un componente suelto, pero no pone en riesgo a los usuarios FLL-N-0013\",\r\n    \"fecha_apertura\": \"2023-02-17T17:47:09.617-05:00\",\r\n    \"fecha_cierre\": null,\r\n    \"fecha_arribo_locacion\": \"2023-02-17T17:47:23\",\r\n    \"componente_Parte\": \" \",\r\n    \"tipo_reparacion\": \"Ajuste\",\r\n    \"tipo_ajuste_configuracion\": \"Otros ajustes de puertas AJS-P-0100\\n\",\r\n    \"descripcion_reparacion\": \"null\",\r\n    \"diagnostico_causa\": \"A cargo del contratista\",\r\n    \"tipo_causa\": \"A cargo del contratista\",\r\n    \"estado_ticket\": \"Cerrado\"\r\n  },\r\n  {\r\n    \"id_ticket\": \"TICKET-56\",\r\n    \"id_estacion\": \"null\",\r\n    \"id_vagon\": \"null\",\r\n    \"id_puerta\": \"null\",\r\n    \"id_componente\": \"9110-WA-OR-1\",\r\n    \"tipoComponente\": \"null\",\r\n    \"identificacion\": \"null\",\r\n    \"tipo_mantenimiento\": \"Correctivo\",\r\n    \"nivel_falla\": \"ANIO\",\r\n    \"codigo_falla\": \"No reproducción de mensajes de audio FLL-N-0001\",\r\n    \"fecha_apertura\": \"2023-02-17T11:26:05.026-05:00\",\r\n    \"fecha_cierre\": null,\r\n    \"fecha_arribo_locacion\": null,\r\n    \"componente_Parte\": \" \",\r\n    \"tipo_reparacion\": \"Configuracion\",\r\n    \"tipo_ajuste_configuracion\": \"Configuración de red CFG-I-0001\\n\",\r\n    \"descripcion_reparacion\": \"null\",\r\n    \"diagnostico_causa\": \"A cargo del contratista\",\r\n    \"tipo_causa\": \"A cargo del contratista\",\r\n    \"estado_ticket\": \"Cerrado\"\r\n  },\r\n  {\r\n    \"id_ticket\": \"TICKET-54\",\r\n    \"id_estacion\": \"null\",\r\n    \"id_vagon\": \"null\",\r\n    \"id_puerta\": \"Call1\",\r\n    \"id_componente\": \"Call1\",\r\n    \"tipoComponente\": \"Puerta\",\r\n    \"identificacion\": \"cra345\",\r\n    \"tipo_mantenimiento\": \"null\",\r\n    \"nivel_falla\": \"ANIO\",\r\n    \"codigo_falla\": \"No reproducción de mensajes de audio FLL-N-0001\",\r\n    \"fecha_apertura\": \"2023-02-16T18:43:15.695-05:00\",\r\n    \"fecha_cierre\": null,\r\n    \"fecha_arribo_locacion\": \"2023-02-17T11:26:59\",\r\n    \"componente_Parte\": \" \",\r\n    \"tipo_reparacion\": \" \",\r\n    \"tipo_ajuste_configuracion\": \"\",\r\n    \"descripcion_reparacion\": \"null\",\r\n    \"diagnostico_causa\": \"A cargo del contratista\",\r\n    \"tipo_causa\": \"A cargo del contratista\",\r\n    \"estado_ticket\": \"Cerrado\"\r\n  },\r\n  {\r\n    \"id_ticket\": \"TICKET-45\",\r\n    \"id_estacion\": \"null\",\r\n    \"id_vagon\": \"null\",\r\n    \"id_puerta\": \"9110-WA-OR-1\",\r\n    \"id_componente\": \"9110-WA-OR-1\",\r\n    \"tipoComponente\": \"Puerta\",\r\n    \"identificacion\": \"N01-953542\",\r\n    \"tipo_mantenimiento\": \"Correctivo\",\r\n    \"nivel_falla\": \"ANIO\",\r\n    \"codigo_falla\": \"No reproducción de mensajes de audio FLL-N-0001\",\r\n    \"fecha_apertura\": \"2023-02-09T17:32:11.002-05:00\",\r\n    \"fecha_cierre\": null,\r\n    \"fecha_arribo_locacion\": \"2023-02-13T07:48:50\",\r\n    \"componente_Parte\": \" \",\r\n    \"tipo_reparacion\": \"Configuracion\",\r\n    \"tipo_ajuste_configuracion\": \"Configuración parámetros de conexión CFG-I-0003\\n\",\r\n    \"descripcion_reparacion\": \"null\",\r\n    \"diagnostico_causa\": \"A cargo del contratista\",\r\n    \"tipo_causa\": \"A cargo del contratista\",\r\n    \"estado_ticket\": \"Cerrado\"\r\n  }\r\n]";
            // Cargar la cadena de texto como objeto JSON
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Ticket> ticketsTAI = JsonSerializer.Deserialize<List<Ticket>>(cadena_ticketsTAI, opciones);
            List<Ticket> ticketsTCI = JsonSerializer.Deserialize<List<Ticket>>(cadena_ticketsTCI, opciones);


            // List<Ticket> ticketsTCI = JsonConvert.DeserializeObject<List<Ticket>>(DataTest._dataTicketsTCI);
            var rano = new RANOEntity(ticketsTAI, ticketsTCI);

            // Calcular el indicador RANO
            var indicadorRANO = rano.CalcularIndicadorRANO();

            Assert.That(indicadorRANO.ToString(""), Is.EqualTo("16,7"));

        }

    }
}